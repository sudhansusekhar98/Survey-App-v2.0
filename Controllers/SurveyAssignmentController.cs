using Microsoft.AspNetCore.Mvc;

namespace SurveyApp.Controllers
{
    public class SurveyAssignmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
