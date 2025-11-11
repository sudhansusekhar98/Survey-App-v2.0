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
        public IActionResult SurveyLocation(Int64 surveyId, string SurveyName, int? editId)
        {
            // Fetch locations for the selected survey
            var locations = _surveyRepository.GetSurveyLocationById(surveyId) ?? new List<SurveyLocationModel>();
            ViewBag.SelectedSurveyId = surveyId;
            ViewBag.SelectedSurveyName = SurveyName;
            return View("SurveyLocation", locations);
        }

        // POST: SurveyCreation/SurveyLocation - Handle inline create/update form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SurveyLocation(SurveyLocationModel model)
        {
            try
            {
                // Get current user from session
                int createdBy = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "0");
                model.CreateBy = createdBy;

                bool result;
                
                // Check if this is an update or create operation
                if (model.LocID > 0)
                {
                    // Update existing location
                    result = _surveyRepository.UpdateSurveyLocation(model);
                    
                    if (result)
                    {
                        TempData["ResultMessage"] = "<strong>Success!</strong> Location updated successfully.";
                        TempData["ResultType"] = "success";
                    }
                    else
                    {
                        TempData["ResultMessage"] = "<strong>Error!</strong> Failed to update location.";
                        TempData["ResultType"] = "danger";
                    }
                }
                else
                {
                    // Create new location
                    result = _surveyRepository.AddSurveyLocation(model);
                    
                    if (result)
                    {
                        TempData["ResultMessage"] = "<strong>Success!</strong> Location added successfully.";
                        TempData["ResultType"] = "success";
                    }
                    else
                    {
                        TempData["ResultMessage"] = "<strong>Error!</strong> Failed to add location.";
                        TempData["ResultType"] = "danger";
                    }
                }

                return RedirectToAction("SurveyLocation", new { surveyId = model.SurveyID, SurveyName = ViewBag.SelectedSurveyName });
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return RedirectToAction("SurveyLocation", new { surveyId = model.SurveyID });
            }
        }

        //Delete Survey sub-locations
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSurveyLocation(int locId, int surveyId)
        {
            try
            {
                bool result = _surveyRepository.DeleteSurveyLocation(locId);
                if (result)
                {
                    return Json(new { success = true, message = "Location deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete location." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // GET: SurveyCreation/ItemTypeMaster
        public IActionResult ItemTypeMaster(int locId, string SurveyName, Int64 surveyId)
        {
            try
            {
                var itemTypes = _surveyRepository.GetItemTypeMaster(locId) ?? new List<ItemTypeMasterModel>();
                ViewBag.LocId = locId; // Pass locId to the view if needed
                ViewBag.SelectedSurveyId = surveyId;
                ViewBag.SelectedSurveyName = SurveyName;
                return View("ItemTypeMaster", itemTypes);
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View("ItemTypeMaster", new List<ItemTypeMasterModel>());
            }
        }

        // GET: SurveyCreation/SaveItemType
        [HttpGet]
        public IActionResult SaveItemType(int locId)
        {
            try
            {
                var itemTypes = _surveyRepository.GetItemTypeMaster(locId) ?? new List<ItemTypeMasterModel>();
                return View("ItemTypeMaster", itemTypes);
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View("ItemTypeMaster", new List<ItemTypeMasterModel>());
            }
        }

        // POST: SurveyCreation/SaveItemType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItemType(Int64 surveyId, string surveyName, int locId, List<int> selectedTypeIds)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "0");

            // Save selected item types for the survey and location
            _surveyRepository.SaveItemTypesForLocation(surveyId, surveyName, locId, selectedTypeIds);

            TempData["ResultMessage"] = "<strong>Success!</strong> Device types saved successfully.";
            TempData["ResultType"] = "success";
            return RedirectToAction("ItemTypeMaster", new { locId, surveyId, surveyName });
        }

        // POST: SurveyCreation/AddSurveyLocations
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSurveyLocations(Int64 surveyId, List<SurveyLocationModel> locations)
        {
            int createdBy = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "0");
            if (locations == null || locations.Count == 0)
            {
                TempData["ResultMessage"] = "<strong>Error!</strong> No locations provided.";
                TempData["ResultType"] = "danger";
                return RedirectToAction("SurveyLocation", new { surveyId });
            }

            bool result = _surveyRepository.CreateLocationsBySurveyId(surveyId, locations, createdBy);

            if (result)
            {
                TempData["ResultMessage"] = "<strong>Success!</strong> Locations added successfully.";
                TempData["ResultType"] = "success";
            }
            else
            {
                TempData["ResultMessage"] = "<strong>Error!</strong> Failed to add locations.";
                TempData["ResultType"] = "danger";
            }
            return RedirectToAction("SurveyLocation", new { surveyId });
        }
    }
}
