using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ProyectoFinalLenguajes.Data.Repository.Interface;

namespace ProyectoFinalLenguajes.Areas.Customer.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public DishController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetEnabledDishes()
        {
            var allDishes = _unitOfWork.Dish.GetAll();
            var enabledDishes = allDishes.Where(x => x.isAble);
            return Ok(enabledDishes);
        }

        [HttpGet("{id}")]
        public IActionResult GetDish(int id)
        {
            var dish = _unitOfWork.Dish.Get(x => x.Id == id);
            return Ok(dish);
        }
    }
}
