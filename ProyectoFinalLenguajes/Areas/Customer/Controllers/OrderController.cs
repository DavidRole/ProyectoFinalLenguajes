using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Utilities;
using System.Security.Claims;

namespace ProyectoFinalLenguajes.Areas.Customer.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        public readonly IUnitOfWork _unitOfwork;
        private readonly UserManager<AppUser> _userManager;
        public OrderController(IUnitOfWork unitOfwork, UserManager<AppUser> userManager)
        {
            _unitOfwork = unitOfwork;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] AddOrderModel model)
        {
            if (model == null || model.Items == null || !model.Items.Any())
                return BadRequest("Debe enviar al menos un plato con cantidad.");


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();


            var order = new Order
            {
                CustomerId = user.Id,
                Date = DateTime.UtcNow,
                Status = model.Status
            };


            foreach (var item in model.Items)
            {
                order.OrderDishes.Add(new OrderDetail
                {
                    DishId = item.DishId,
                    Quantity = item.Quantity
                });
            }


            _unitOfwork.Order.Add(order);
            _unitOfwork.Save();


            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                new { order.Id, order.Date, order.Status }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = _unitOfwork.Order.Get(o => o.Id == id,"Customer,OrderDishes.Dish");
            

            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetByCustomer(string id)
        {
            var all = _unitOfwork.Order.GetAll("Customer,OrderDishes.Dish");
            var filtered = all.Where(o => o.CustomerId == id).ToList();

            if (filtered.Count == 0)
                return NotFound();

            return Ok(filtered);
        }


        public class AddOrderModel
        {
            public List<AddOrderItemModel> Items { get; set; }
            public string? Status { get; set; } = StaticValues.ActiveOrder;
        }

        public class AddOrderItemModel
        {
            public int DishId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
