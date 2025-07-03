using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Utilities;

namespace ProyectoFinalLenguajes.Areas.Kitchen.Controllers
{
    [Area("Kitchen")]

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
            var orderList = _unitOfWork.Order.GetAll("OrderDetails, Customer");

            return Json(new {data = orderList});
        }
        #endregion
    }
}
