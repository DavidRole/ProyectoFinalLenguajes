using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalLenguajes.Data.Repository;
using ProyectoFinalLenguajes.Data.Repository.Interface;

namespace ProyectoFinalLenguajes.Areas.Customer.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        public readonly IUnitOfWork _unitOfwork;

        public TestController(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public IActionResult Index()
        {
            return Ok("Hello from TestController.Index");
        }

        [HttpGet("dishes")]
        public IActionResult GetEnabledDishes()
        {
            var allDishes = _unitOfwork.Dish.GetAll();
            var enabledDishes = allDishes.Where(x => x.isAble);
            return Ok(enabledDishes);
        }
    }
}
