using AnalyticaDocs.Models;
using AnalyticaDocs.Repo;
using AnalyticaDocs.Repository;
using Microsoft.AspNetCore.Mvc;
using SurveyApp.Models;
using SurveyApp.Repo;
using System.Diagnostics;

namespace SurveyApp.Controllers
{
    public class SurveyDetailsController : Controller
    {
        private readonly ISurvey _repository;
        private readonly ICommonUtil _util;

        public SurveyDetailsController(ISurvey repository, ICommonUtil util)
        {
            _repository = repository;
            _util = util;
        }
        


        public IActionResult Index()
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return result;

            long surveyId = 20251104001;
            int locId = 101;

            // Get the list of types/locations assigned
            var deviceTypes = _repository.GetAssignedTypeList(surveyId, locId)
                              ?? new List<SurveyDetailsLocationModel>();

            var modelList = new List<SurveyDetailsLocationModel>();

            foreach (var dt in deviceTypes)
            {
                // Load item list for this type/location
                var items = _repository.GetAssignedItemList(dt.SurveyID, dt.LocID, dt.ItemTypeID)
                            ?? new List<SurveyDetailsModel>();

                // Create a new instance so we keep any extra properties from dt
                modelList.Add(new SurveyDetailsLocationModel
                {
                    SurveyID = dt.SurveyID,
                    LocID = dt.LocID,
                    ItemTypeID = dt.ItemTypeID,
                    LocName = dt.LocName,
                    SurveyName = dt.SurveyName,
                    TypeName = dt.TypeName,
                    TypeDesc = dt.TypeDesc,
                    GroupName = dt.GroupName,
                    CreatedBy = dt.CreatedBy,
                    ItemLists = items
                });
            }

            // Pass the list as the model to the view
            return View("SurveyDetails", modelList);
        }


        public IActionResult UpdateItem(Int64 surveyId,int locId, int itemTypeID, int itemId)
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return result;

            if (surveyId <= 0 && itemTypeID <= 0&& locId <= 0)
                return RedirectToAction("Index");


            var formModel = new SurveyDetailsUpdate
            {
                SurveyID = surveyId,
                LocID = locId,
                ItemTypeID = itemTypeID,
                ItemLists = _repository.GetSurveyUpdateItemList(surveyId,locId,itemTypeID) ?? new List<SurveyDetailsUpdatelist>()
            };
            return View("CameraDevicesView", formModel);
            //switch (itemTypeID)
            //{
            //    case 100:
                    
                  

            //    case 101:
            //        return View("NetworkSwitchView");
            //    case 102:
            //        return View("UPSView");
            //    case 103:
            //        return View("RackView");
            //    case 104:
            //        return View("PatchPanelView");
            //    case 105:
            //        return View("TransceiverView");
            //    case 106:
            //        return View("CableView");
            //    case 107:
            //        return View("AnnouncementView");
            //    case 108:
            //        return View("ImplementationTypeView");
            //    case 109:
            //        return View("PoleInfrastructureView");
            //    case 110:
            //        return View("CantileverView");
            //    default:
            //        return RedirectToAction("Index");
                    
            //}

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateItem(SurveyDetailsUpdate model)
        {
            var result = _util.CheckAuthorization(this, "101");
            if (result != null) return Json("unauthorized");

            model.CreateBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
           

            if (!ModelState.IsValid)
            {
                return Json("invalid");
            }

            bool isSaved = _repository.UpdateSurveyDetails(model);

            TempData["ResultMessage"] = "User rights updated successfully.";
            TempData["ResultType"] = "success";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
