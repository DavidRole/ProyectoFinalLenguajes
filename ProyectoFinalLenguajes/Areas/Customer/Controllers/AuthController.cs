using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProyectoFinalLenguajes.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProyectoFinalLenguajes.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<AppUser> userManager, IConfiguration config, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        #region profile
        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> GetProfile()
        {
            // 1) Obtener el Id del usuario desde la claim "sub"
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // 2) Consultar en la base de datos
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            // 3) Devolver los datos solicitados
            return Ok(new
            {
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                address = user.Address
            });
        }



        public class UpdateProfileModel
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
        }

        [HttpPost("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel m)
        {
            // obtiene el usuario actual
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.Email = m.Email;
            user.FirstName = m.FirstName;
            user.LastName = m.LastName;
            user.Address = m.Address;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var (token, expiration) = GenerateJwtToken(user);
            return Ok(new { token, expiration });
        }
        #endregion


        #region register
        public class RegisterModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel m)
        {

            if (await _userManager.FindByNameAsync(m.Email) != null)
                return BadRequest(new { message = "El correo ya está registrado." });

            var user = new AppUser
            {
                UserName = m.Email,
                Email = m.Email,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Address = m.Address,
                IsAble = true
            };

            var result = await _userManager.CreateAsync(user, m.Password);
            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            await _userManager.AddToRoleAsync(user, "Customer");

            var (token, expiration) = GenerateJwtToken(user);
            return Ok(new { token, expiration });
        }
        #endregion


        #region login
        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid credentials");

            if (!user.IsAble)
                return Unauthorized("The user has been blocked");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Customer"))
                return Forbid("Only customers may login via API");


            var (token, expiration) = GenerateJwtToken(user);
            return Ok(new { token, expiration });
        }
        #endregion


        #region passwordChange
        public class ChangePasswordModel
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel m)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(
                user, m.OldPassword, m.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var (token, expiration) = GenerateJwtToken(user);
            return Ok(new { token, expiration });
        }
        #endregion

        private (string token, DateTime expiration) GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.Role, "Customer"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);

            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
        }
    }

}
