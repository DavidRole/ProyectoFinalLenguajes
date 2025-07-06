using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Utilities;

namespace ProyectoFinalLenguajes.Areas.Admins.Controllers
{
    [Area("Admins")]

    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditStatus(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var order = _unitOfWork.Order.Get(o => o.Id == id, includeProperties: "OrderDishes");

            if (order == null)
            {
                return NotFound();
            }

            var statusOptions = new List<string>
            {
                StaticValues.OnTimeOrder,    // "On Time"
                StaticValues.OvertimeOrder,  // "Overtime"
                StaticValues.LateOrder,      // "Late AF"
                StaticValues.DeliveredOrder, // "Delivered"
            };

            ViewBag.StatusOptions = new SelectList(statusOptions, order.Status); 

            return View(order);
        }

        [HttpGet]
        public IActionResult Details(int id) 
        {
            if (id == 0)
            {
                return NotFound(); 
            }

            var order = _unitOfWork.Order.Get(
                o => o.Id == id, 
                includeProperties: "Customer,OrderDishes.Dish" 
            );

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStatus(int id, Order order) 
        {

            if (id != order.Id)
            {
                return BadRequest("Mismatched IDs.");
            }

            var orderFromDb = _unitOfWork.Order.Get(o => o.Id == order.Id);

            if (orderFromDb == null)
            {
                TempData["error"] = "Order not found.";
                return NotFound();
            }

            orderFromDb.Status = order.Status; 

            _unitOfWork.Order.Update(orderFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Estado de la orden actualizado exitosamente.";
            return RedirectToAction("Details", new { id = order.Id });
        }

        [HttpGet]
        public IActionResult Upsert(int Id)
        {
            Order order = new Order();
            if (Id == 0)
            {
                return View(order);
            }

            order = _unitOfWork.Order.Get(x => x.Id == Id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult Upsert(Order order)
        {
            if (ModelState.IsValid)
            {
                if (order.Id == 0)
                {
                    _unitOfWork.Order.Add(order);
                }
                else
                {
                    _unitOfWork.Order.Update(order);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Action completed successfully";
                return RedirectToAction("Index");

            }
            TempData["error"] = "The action couldn't be resolved";
            return View(order);
        }

        #region API
        public IActionResult GetAll()
        {
            var orderList = _unitOfWork.Order.GetAll("Customer,OrderDishes.Dish");
            
            return Json(new { data = orderList });
        }

        [HttpPut]
        public IActionResult Update(int id, string status)
        {

            if (id != 0)
            {
                var order = _unitOfWork.Order.Get(o => o.Id == id);

                if (order == null)
                    return Json(new { success = false, message = "Error updating the order." });

                order.Status = status;
                _unitOfWork.Order.Update(order);
            }
            _unitOfWork.Save();

            return Json(new { success = true, message = "Order updated successfully" });

        }
        #endregion
    }
}
