using Microsoft.AspNetCore.Mvc;

namespace SurveyApp.Controllers
{
    public class SurveyDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            // Logic to get survey details by id can be added here
            return View();
        }
    }
}
