using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Utilities;

namespace ProyectoFinalLenguajes.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = StaticValues.RoleAdmin)]
    public class DishController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DishController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int id)
        {
            Dish dish = new Dish();
            if (id == 0)
            {
                return View(dish);
            }

            dish = _unitOfWork.Dish.Get(x => x.Id == id);

            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        [HttpPost]
        public IActionResult Upsert(Dish dish)
        {
            if (ModelState.IsValid)
            {
                if (dish.Id == 0)
                {
                    _unitOfWork.Dish.Add(dish);
                }
                else
                {
                    _unitOfWork.Dish.Update(dish);
                }
                _unitOfWork.Save();
                TempData["success"] = "Action completed successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The action couldn't be resolved";
            return View(dish);
        }

        #region api
        public IActionResult GetAll()
        {
            var dishList = _unitOfWork.Dish.GetAll();
            return Json( new {data = dishList});
        }

        public IActionResult Delete(int? id)
        {
            var dishDelete = _unitOfWork.Dish.Get(x => x.Id == id);

            if (dishDelete == null)
                return Json(new { success = false, message = "Error deleting the Admin" });

            _unitOfWork.Dish.Remove(dishDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Admin deleted successfully" });

        }
        #endregion
    }
}
