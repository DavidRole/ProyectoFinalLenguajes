using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Utilities;

namespace ProyectoFinalLenguajes.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = StaticValues.RoleAdmin)]
    public class StatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var minutes = _unitOfWork.OrderMinutes.Get(x => x.Id == 1);
            return View(minutes);
        }

        [HttpPost]
        public IActionResult Index(int id, OrderMinutes minutes)
        {
            if (id != minutes.Id)
            {
                return BadRequest("Mismatched IDs.");
            }

            var minutesFromDb = _unitOfWork.OrderMinutes.Get(x => x.Id == minutes.Id);

            if (minutesFromDb == null)
            {
                TempData["error"] = "Not found.";
                return NotFound();
            }

            minutesFromDb.Overtime = minutes.Overtime;
            minutesFromDb.Late = minutes.Late;

            _unitOfWork.OrderMinutes.Update(minutesFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Minutes updated successfully";

            return View();

        }
    }
}
