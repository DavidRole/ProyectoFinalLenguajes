using Microsoft.AspNetCore.Mvc;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProyectoFinalLenguajes.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {

            Admin admin = new Admin();

            if (id == 0 || id == null)
            {
                return View(admin);
            }

            admin = _unitOfWork.Admin.Get(x => x.Id == id );

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        [HttpPost]
        public IActionResult Upsert(Admin admin)
        {
            if (ModelState.IsValid)
            {
                if (admin.Id == 0)
                {
                    _unitOfWork.Admin.Add(admin);
                }
                else
                {
                    _unitOfWork.Admin.Update(admin);
                }
                _unitOfWork.Save();
                TempData["success"] = "Action Completed successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The action couldn't be resolved";
            return View(admin);
        }

        #region API
        public IActionResult GetAll()
        {
            var adminList = _unitOfWork.Admin.GetAll();
            return Json(new { data = adminList });
        }

        public IActionResult Delete(int? id)
        {
            var adminDelete = _unitOfWork.Admin.Get(x => x.Id == id);

            if (adminDelete == null)
                return Json(new { success = false, message = "Error deleting the Admin" });

            _unitOfWork.Admin.Remove(adminDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Admin deleted successfully" });

        }
        #endregion
    }
}
