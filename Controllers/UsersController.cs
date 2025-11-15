using AnalyticaDocs.Models;
using AnalyticaDocs.Repo;
using AnalyticaDocs.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace AnalyticaDocs.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAdmin _repository;
        private readonly ICommonUtil _util;

        public UsersController(IAdmin repository, ICommonUtil util)
        {
            _repository = repository;
            _util = util;
        }
        public IActionResult Index()
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return result;

            ViewBag.DataForGrid = _repository.GetAllDetails();
            return View("Users", new UserModel());

        }

        public IActionResult GetUserModal(int id)
        {
            var user = _repository.GetUserById(id);
            return PartialView("~/Views/Users/_UserDetailModal.cshtml", user);
        }

        public IActionResult Create()
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return result;
            return View("Create", new UserModel()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel user)
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return result;

            if (!ModelState.IsValid)
            {
                return View("Create", user);
            }

            user.CreateBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            bool isSaved = _repository.AddUser(user);

            if (isSaved)
            {
                TempData["ResultType"] = "success";
                TempData["ResultMessage"] = "<strong>Success!</strong> Record Save successfully.";
                return RedirectToAction("Create");
            }
            else
            {
                TempData["ResultType"] = "danger";
                TempData["ResultMessage"] = "<strong>Error!</strong> Record Not Save.";
                return View("Create", user);
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return result;

            if (!id.HasValue)
                return RedirectToAction("Index");

            var user =  _repository.GetUserById(id.Value);
            if (user == null)
            {
                TempData["ResultMessage"] = "User not found.";
                return RedirectToAction("Index");
            }
            return View("Edit", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(UserModel user)
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return Json("unauthorized");

            if (!ModelState.IsValid)
            {
                return Json("invalid");
            }

            user.CreateBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            bool isSaved = _repository.UpdateUser(user);

            return Json(isSaved ? "success" : "fail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmUpdate()
        {
            // Optional: log confirmation, trigger workflow, etc.
            return Json("OK"); // Or "done", "ok", etc.
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
