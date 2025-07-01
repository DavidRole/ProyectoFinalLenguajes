using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Utilities;

namespace ProyectoFinalLenguajes.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = StaticValues.RoleAdmin)]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(UserManager<IdentityUser> uow)
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

            AppUser admin = new AppUser();

            if (String.IsNullOrEmpty(id))
            {
                return View(admin);
            }

            admin = (AppUser)_userManager.Users.ToList().Select(X => X.Id.Equals(id));

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        [HttpPost]
        public IActionResult Upsert(AppUser admin)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(admin.Id))
                {
                    _userManager.UpdateAsync(admin);

                    TempData["success"] = "Action Completed successfully";
                    return RedirectToAction("Index");
                }
                TempData["error"] = "The action couldn't be resolved";
                return View(admin);
            }
            TempData["error"] = "The action couldn't be resolved";
            return View();
        }

        #region API
        public IActionResult GetAll()
        {
            var adminList = _userManager.Users.ToList();
            return Json(new { data = adminList });
        }

        public IActionResult Delete(string? id)
        {
            var adminDelete = (AppUser) _userManager.Users.ToList().Select(X => X.Id.Equals(id));

            if (adminDelete == null)
                return Json(new { success = false, message = "Error deleting the AppUser" });

            _userManager.DeleteAsync(adminDelete);

            return Json(new { success = true, message = "AppUser deleted successfully" });

        }
        #endregion
    }
}
