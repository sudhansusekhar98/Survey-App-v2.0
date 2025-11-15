using AnalyticaDocs.Repo;
using AnalyticaDocs.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SurveyApp.Models;
using SurveyApp.Repo;
using System;

namespace SurveyApp.Controllers
{
    public class SurveyCreationController : Controller
    {
        private readonly ISurvey _surveyRepository;
        private readonly ICommonUtil _util;
        private readonly IAdmin _adminRepository; 

        public SurveyCreationController(ISurvey surveyRepository, ICommonUtil util, IAdmin adminRepository) // <-- Add IAdmin parameter
        {
            _surveyRepository = surveyRepository;
            _util = util;
            _adminRepository = adminRepository; 
        }

            // GET: SurveyCreation/CameraDevices 
            public IActionResult CameraDevices()
            {
                return View();
            }
            
        // GET: SurveyCreation/Announcement
        public IActionResult Announcement()
            {
                return View();
            }
        public IActionResult NetworkSwitch()
        {
            return View();
        }
        public IActionResult PatchPanel()
        {
            return View();
        }
        public IActionResult Transreceiver()
        {
            return View();
        }
        public IActionResult Rack()
        {
            return View();
        }

        public IActionResult UPS()
        {
            return View();
        }
        public IActionResult Cable()
        {
            return View();
        }

        public IActionResult Traffic()
        {
            return View();
        }

        public IActionResult PoleInfrastructure()
        {
            return View();
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

        //GET: SurveyCreation/SurveyAssignment
        public IActionResult SurveyAssignment(Int64 surveyId)
        {
            var assignments = _surveyRepository.GetSurveyAssignments(surveyId) ?? new List<SurveyAssignmentModel>();
            var survey = _surveyRepository.GetSurveyById(surveyId); 
            ViewBag.SurveyName = survey?.SurveyName;
            
            if (assignments == null || assignments.Count == 0)
            {
                TempData["ResultMessage"] = $"<strong>Info!</strong> No assignments found for this survey {surveyId}.";
                TempData["ResultType"] = "danger";
            }
            
            ViewBag.SurveyID = surveyId;
            return View("SurveyAssignment", assignments);
        }

        // GET: SurveyCreation/CreateSurveyAssignment
        public IActionResult CreateSurveyAssignment(Int64 surveyId)
        {
            var model = new SurveyAssignmentModel { SurveyID = surveyId };
            ViewBag.SurveyID = surveyId;
            
            // TODO: Populate employee dropdown
            ViewBag.Employees = new SelectList(_adminRepository.GetEmpMaster(), "EmpID", "EmpName");            
            return View(model);        
        }

        //POST: SurveyCreation/SurveyAssignment/Create
                [HttpPost]
                [ValidateAntiForgeryToken]
                public IActionResult CreateSurveyAssignment(SurveyAssignmentModel model)
                {
                    try
                    {
                        if (!ModelState.IsValid)
                        {
                            ViewBag.Employees = new SelectList(_adminRepository.GetEmpMaster(), "EmpID", "EmpName");
                            foreach (var key in ModelState.Keys)
                            {
                                var errors = ModelState[key].Errors;
                                foreach (var error in errors)
                                {
                                    Console.WriteLine($"{key}: {error.ErrorMessage}");
                                }
                            }
                            TempData["ResultMessage"] = "<strong>Validation Error!</strong> Please check all required fields.";
                            TempData["ResultType"] = "warning";
                            return View("CreateSurveyAssignment", model);
                        }
                        int createdBy = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "101");
                        if (model.SelectedEmpIDs != null && model.SelectedEmpIDs.Count > 0)
                        {
                            foreach (var empId in model.SelectedEmpIDs)
                            {
                                var assignment = new SurveyAssignmentModel
                                {
                                    SurveyID = model.SurveyID,
                                    EmpID = empId,
                                    DueDate = model.DueDate,
                                    CreateBy = createdBy,
                                };
                                _surveyRepository.AddSurveyAssignment(assignment);
                            }
                            TempData["ResultMessage"] = "<strong>Success!</strong> Assignments created successfully.";
                            TempData["ResultType"] = "success";
                            return RedirectToAction("SurveyAssignment", new { surveyId = model.SurveyID });
                        }
                        else
                        {
                            ViewBag.Employees = new SelectList(_adminRepository.GetEmpMaster(), "EmpID", "EmpName");
                            TempData["ResultMessage"] = "<strong>Error!</strong> No employees selected.";
                            TempData["ResultType"] = "danger";
                            return View("CreateSurveyAssignment", model); // <-- Fix here
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Employees = new SelectList(_adminRepository.GetEmpMaster(), "EmpID", "EmpName");
                        TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                        TempData["ResultType"] = "danger";
                        return View("CreateSurveyAssignment", model); // <-- Fix here
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
        public IActionResult SurveyLocation(SurveyLocationModel model, bool Isactive = false)
        {
            try
            {
                // Explicitly set Isactive from the parameter
                model.Isactive = Isactive;
                
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

        // Delete Survey sub-locations
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSurveyLocation(int locId, int surveyId, string surveyName = "")
        {
            try
            {
                bool result = _surveyRepository.DeleteSurveyLocation(locId);
                TempData["ResultMessage"] = result
                    ? "<strong>Success!</strong> Location deleted successfully."
                    : "<strong>Error!</strong> Failed to delete location.";
                TempData["ResultType"] = result ? "success" : "danger";
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
            }

            // Try to retain surveyName
            surveyName = !string.IsNullOrWhiteSpace(surveyName)
                ? surveyName
                : Request.Form["surveyName"].ToString()
                ?? Request.Query["surveyName"].ToString()
                ?? TempData["SelectedSurveyName"] as string
                ?? "";

            TempData["SelectedSurveyName"] = surveyName;

            return RedirectToAction("SurveyLocation", new { surveyId, surveyName });
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

        // GET: SurveyCreation/ItemTypeMaster
        public IActionResult ItemTypeMaster(int locId, string SurveyName, Int64 surveyId)
        {
            try
            {
                var itemTypes = _surveyRepository.GetItemTypeMaster(locId) ?? new List<ItemTypeMasterModel>();
                var selectedItemTypes = _surveyRepository.GetSelectedItemTypesForLocation(locId) ?? new List<ItemTypeMasterModel>();

                // Mark items that are already assigned
                var selectedIds = selectedItemTypes.Select(x => x.Id).ToList();
                foreach (var item in itemTypes)
                {
                    item.IsAssigned = selectedIds.Contains(item.Id);
                }

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
        public IActionResult ItemTypeMaster(int locId, Int64 surveyId)
        {

            var formModel = new AssignedItemsModel
            {
                SurveyId = surveyId,
                LocID = locId,

                AssignItemList = _surveyRepository.GetItemTypebySurveyLoc(locId, surveyId) ?? new List<AssignedItemsListModel>()
            };


            return View("ItemTypeMaster", formModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItemType(AssignedItemsModel model)
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return Json("unauthorized");

            if (!ModelState.IsValid)
            {
                return Json("invalid");
            }

            model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            bool isSaved = _surveyRepository.UpdateAssignedItems(model);

            TempData["ResultMessage"] = "Device Updated for Survey";
            TempData["ResultType"] = "success";

            var locations = _surveyRepository.GetSurveyLocationById(model.SurveyId) ?? new List<SurveyLocationModel>();
            ViewBag.SelectedSurveyId = model.SurveyId;
            //ViewBag.SelectedSurveyName = SurveyName;
            return View("SurveyLocation", locations);
            //return RedirectToAction("Index");
        }

        //GET: SurveyCreation/ViewSelectedItemTypes 
        public IActionResult ViewSelectedItemTypes(int locId, Int64 surveyId, string surveyName)
        {
            try
            {
                var selectedItemTypes = _surveyRepository.GetSelectedItemTypesForLocation(locId);
                var location = _surveyRepository.GetSurveyLocationByLocId(locId);

                var viewModel = new AssignedItemsModel
                {
                    LocID = locId,
                    SurveyId = surveyId,
                    SurveyName = surveyName ?? string.Empty,
                    AssignItemList = selectedItemTypes.Select
                    (
                        item => new AssignedItemsListModel
                        {
                            ItemTypeID = item.Id,
                            TypeName = item.TypeName,
                            TypeDesc = item.TypeDesc,
                            GroupName = item.GroupName,
                            IsAssigned = true,
                        }
                    ).ToList()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return RedirectToAction("Index");
            }
        }




    }
}
