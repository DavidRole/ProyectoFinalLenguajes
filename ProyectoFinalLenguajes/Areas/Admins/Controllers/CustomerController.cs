using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Utilities;
using System.Net;

namespace ProyectoFinalLenguajes.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = StaticValues.RoleAdmin)]
    public class CustomerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public CustomerController(UserManager<AppUser> uow)
        {
            _userManager = uow;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(string? id)
        {

            AppUser user = new AppUser();

            if (String.IsNullOrEmpty(id))
            {
                return View(user);
            }

            user = _userManager.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(AppUser user)
        {
            if (ModelState.IsValid)
            {
                var existing = await _userManager.FindByIdAsync(user.Id);
                if (existing == null) return NotFound();

                existing.FirstName = user.FirstName;
                existing.Email = user.Email;
                existing.IsAble = user.IsAble;
                // …and so on for whatever you allow to change

                var result = await _userManager.UpdateAsync(existing);

                if (result.Succeeded)
                {
                    TempData["success"] = "Customer updated successfully";
                    return RedirectToAction("Index");
                }

                
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }

                TempData["error"] = "Failed to update customer";
            }
            TempData["error"] = "The action couldn't be resolved, model invalid";
            return View();
        }

        #region API
        public async Task<IActionResult> GetAll()
        {
            var customerList = await _userManager.GetUsersInRoleAsync(StaticValues.RoleCustomer);

            return Json(new { data = customerList });
        }
        #endregion
    }
}
