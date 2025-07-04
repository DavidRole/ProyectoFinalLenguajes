using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ProyectoFinalLenguajes.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalLenguajes.Utilities;
using ProyectoFinalLenguajes.Data.Repository.Interface;

namespace ProyectoFinalLenguajes.Areas.Customer.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        public readonly IUnitOfWork _unitOfwork;
        private readonly UserManager<AppUser> _userManager;

        public TestController(IUnitOfWork unitOfwork, UserManager<AppUser> userManager)
        {
            _unitOfwork = unitOfwork; 
            _userManager = userManager;
        }
        

        [HttpGet("dishes")]
        public IActionResult GetEnabledDishes()
        {
            var allDishes = _unitOfwork.Dish.GetAll();
            var enabledDishes = allDishes.Where(x => x.isAble);
            return Ok(enabledDishes);
        }

        
        [HttpPost("order")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderModel model)
        {
            if (model == null || model.Items == null || !model.Items.Any())
                return BadRequest("Debe enviar al menos un plato con cantidad.");

            // 1. Obtener el usuario autenticado
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // 2. Crear la cabecera de la orden
            var order = new Order
            {
                CustomerId = user.Id,
                Date = DateTime.UtcNow,
                Status = model.Status ?? "Pending"
            };

            // 3. Agregar detalles
            foreach (var item in model.Items)
            {
                order.OrderDishes.Add(new OrderDetail
                {
                    DishId = item.DishId,
                    Quantity = item.Quantity
                });
            }

            // 4. Persistir con UnitOfWork
            _unitOfwork.Order.Add(order);
            _unitOfwork.Save();

            // 5. Devolver la orden creada
            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                new { order.Id, order.Date, order.Status }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var orders = _unitOfwork.Order
                .GetAll("OrderDishes,Dish,Customer"
                );

            var order = orders.Where(o => o.Id == id);

            if (order == null) return NotFound();
            return Ok(order);
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
