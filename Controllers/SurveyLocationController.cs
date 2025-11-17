using Microsoft.AspNetCore.Mvc;
using SurveyApp.Models;
using SurveyApp.Repo;

namespace SurveyApp.Controllers
{
    public class SurveyLocationController : Controller
    {
        private readonly ISurveyLocation _locationRepo;

        public SurveyLocationController(ISurveyLocation locationRepo)
        {
            _locationRepo = locationRepo;
        }

        public IActionResult Index()
        {
            var locations = new List<SurveyLocationModel>();
            return View(locations);
        }

        public IActionResult Edit(int id)
        {
            var location = _locationRepo.GetLocationById(id);
            if (location == null)
                return RedirectToAction("Index");
            return View("SurveyLocationEdit", location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SurveyLocationModel model)
        {
            if (!ModelState.IsValid)
                return View("SurveyLocationEdit", model);

            _locationRepo.UpdateLocation(model);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View("SurveyLocationCreate", new SurveyLocationModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SurveyLocationModel model)
        {
            if (!ModelState.IsValid)
                return View("SurveyLocationCreate", model);

            _locationRepo.AddLocation(model);
            return RedirectToAction("Index");       
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _locationRepo.DeleteLocation(id);
            return RedirectToAction("Index");
        }
    }
}
