using AnalyticaDocs.Repository;
using Microsoft.AspNetCore.Mvc;
using SurveyApp.Models;
using SurveyApp.Repo;
using System;

namespace SurveyApp.Controllers
{
    public class SurveyCreationController : Controller
    {
        private readonly ISurvey _surveyRepository;

        public SurveyCreationController(ISurvey surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        // GET: SurveyCreation/Index - List all surveys
        public IActionResult Index()
        {
            try
            {
                var surveys = _surveyRepository.GetAllSurveys() ?? new List<SurveyModel>();

                return View(surveys);
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View("Index", new List<SurveyModel>());
            }
        }

        // GET: SurveyCreation/Create
        public IActionResult Create()
        {
            return View("SurveyCreation", new SurveyModel());
        }

        // POST: SurveyCreation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SurveyModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ResultMessage"] = "<strong>Validation Error!</strong> Please check all required fields.";
                    TempData["ResultType"] = "warning";
                    return View("SurveyCreation", model);
                }
               

                // Set CreatedBy from session
                model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "0");

                bool result = _surveyRepository.AddSurvey(model);

                if (result)
                {
                    TempData["ResultMessage"] = "<strong>Success!</strong> Survey created successfully.";
                    TempData["ResultType"] = "success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ResultMessage"] = "<strong>Error!</strong> Failed to create survey.";
                    TempData["ResultType"] = "danger";
                    return View("SurveyCreation", model);
                }
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View("SurveyCreation", model);
            }
        }

        // GET: SurveyCreation/Edit/5
        public IActionResult Edit(Int64? id)
        {
            if (!id.HasValue || id.Value == 0)
            {
                TempData["ResultMessage"] = "<strong>Info!</strong> Record Not Found.";
                TempData["ResultType"] = "warning";
                return RedirectToAction("Index");
            }
            var survey = _surveyRepository.GetSurveyById(id.Value);
            if (survey == null) 
            {
                TempData["ResultMessage"] = "<strong>Not Found!</strong> Survey not found.";
                TempData["ResultType"] = "warning";
                return RedirectToAction("Index"); // Render full page for normal navigation
            }
            return View("SurveyEdit", survey); // Render full page for normal navigation

        }


        // POST: SurveyCreation/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SurveyModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ResultMessage"] = "<strong>Validation Error!</strong> Please check all required fields.";
                    TempData["ResultType"] = "warning";
                    return View("SurveyEdit", model);
                }

                // Set CreatedBy from session
                model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "0");

                bool result = _surveyRepository.UpdateSurvey(model);

                if (result)
                {
                    TempData["ResultMessage"] = "<strong>Success!</strong> Survey updated successfully.";
                    TempData["ResultType"] = "success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ResultMessage"] = "<strong>Error!</strong> Failed to update survey.";
                    TempData["ResultType"] = "danger";
                    return View("SurveyEdit", model);
                }
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View("SurveyEdit", model);
            }
        }

        // POST: SurveyCreation/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Int64 id)
        {
            try
            {
                bool result = _surveyRepository.DeleteSurvey(id);

                if (result)
                {
                    return Json(new { success = true, message = "Survey deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete survey." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        // GET: SurveyCreation/SurveyLocation
        public IActionResult SurveyLocation()
        {
            // Render the view directly, do NOT redirect
            var locations = new List<SurveyLocationModel>();
            return View("SurveyLocation", locations);
        }
        // GET: SurveyCreation/SurveyLocationView
        [HttpGet]
        public IActionResult SurveyLocationView()
        {
            var model = new List<SurveyLocationModel>();
            return View("SurveyLocation", model);
        }
        // GET: SurveyCreation/SurveyLocationCreate
        [HttpGet]
        public IActionResult SurveyLocationCreate()
        {
            var model = new SurveyLocationModel();
            return View("SurveyLocationCreate", model);
        }
        // POST: SurveyCreation/SurveyLocationCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SurveyLocationCreate(SurveyLocationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SurveyLocationCreate", model);
            }

            // Save the model
            // ...

            return RedirectToAction("SurveyLocation");
        }

        // GET: SurveyCreation/SurveyLocationEdit
        [HttpGet]
        public IActionResult SurveyLocationEdit(int id)
        {
            var model = new SurveyLocationModel
            {
                LocID = id,
                LocName = "Sample Location",
                LocLat = 12.345678M,
                LocLog = 98.765432M,
                Isactive = true
            };
            return View("SurveyLocationEdit", model);
        }
        // POST: SurveyCreation/SurveyLocationEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SurveyLocationEdit(SurveyLocationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SurveyLocationEdit", model);
            }

            // Update the model
            // ...

            return RedirectToAction("SurveyLocation");
        }

        // POST: SurveyCreation/SurveyLocationDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SurveyLocationDelete(int id)
        {
            // Delete the location by id
            // ...

            return RedirectToAction("SurveyLocation");
        }


    // GET: SurveyCreation/ItemTypeMaster
    public IActionResult ItemTypeMaster()
    {
        // Redirect to SaveItemType for correct model type
        return RedirectToAction("SaveItemType");
        }

        // GET: SurveyCreation/SaveItemType
        [HttpGet]
        public IActionResult SaveItemType()
        {
            // Example data for testing
            var model = new List<ItemTypeMasterModel>
            {
                new ItemTypeMasterModel { TypeName = "Camera", TypeDesc = "Surveillance Camera", GroupName = "Security", IsActive = 'Y' },
                new ItemTypeMasterModel { TypeName = "Network Switch", TypeDesc = "Ethernet Switch", GroupName = "Networking", IsActive = 'Y' },
                new ItemTypeMasterModel { TypeName = "UPS", TypeDesc = "Uninterruptible Power Supply", GroupName = "Power", IsActive = 'Y' }
                // Add more items as needed
            };
            return View("ItemTypeMaster", model);   
        }

        // POST: SurveyCreation/SaveItemType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItemType(List<ItemTypeMasterModel> model)
        {
            // TODO: Handle selected items
            TempData["ResultMessage"] = "<strong>Success!</strong> Device types saved successfully.";
            TempData["ResultType"] = "success";
            return RedirectToAction("SaveItemType");
        }
    }
}
