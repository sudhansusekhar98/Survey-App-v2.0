using Microsoft.AspNetCore.Mvc;
using SurveyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SurveyApp.Controllers
{
    public class PoleInfrastructureController : Controller
    {
        // Mock base data for Poles (TypeId = 109)
        private static readonly List<ItemMasterModel> Poles = new List<ItemMasterModel>
        {
            new ItemMasterModel { ItemId = 1000034, TypeId = 109, ItemName = "Iron Pole", ItemCode = "IRON-STEEL", ItemDesc = "Iron infrastructure pole", IsActive = 'Y', CreatedOn = DateTime.Now, CreatedBy = 10001 },
            new ItemMasterModel { ItemId = 1000035, TypeId = 109, ItemName = "Concrete Pole", ItemCode = "POLE-CONCRETE", ItemDesc = "Concrete infrastructure pole", IsActive = 'Y', CreatedOn = DateTime.Now, CreatedBy = 10001 },
            new ItemMasterModel { ItemId = 1000036, TypeId = 109, ItemName = "Wooden Pole", ItemCode = "POLE-WOOD", ItemDesc = "Wooden infrastructure pole", IsActive = 'Y', CreatedOn = DateTime.Now, CreatedBy = 10001 },
            new ItemMasterModel { ItemId = 1000037, TypeId = 109, ItemName = "Gantry", ItemCode = "POLE-GANTRY", ItemDesc = "Overhead gantry structure", IsActive = 'Y', CreatedOn = DateTime.Now, CreatedBy = 10001 }
        };

        // Mock Cantilever Data (TypeId = 110)
        private static readonly List<CantileverItem> Cantilevers = new List<CantileverItem>
        {
            new CantileverItem { Name = "Cantilever 1m", Length = "1 Meter", Quantity = 0 },
            new CantileverItem { Name = "Cantilever 2m", Length = "2 Meter", Quantity = 0 },
            new CantileverItem { Name = "Cantilever 3m", Length = "3 Meter", Quantity = 0 },
            new CantileverItem { Name = "Cantilever 4m", Length = "4 Meter", Quantity = 0 }
        };

        // In-memory data store for session-like persistence
        private static PoleCantileverViewModel _savedModel = new PoleCantileverViewModel();

        [HttpGet]
        public IActionResult Index()
        {
            // Return the saved model or create a fresh one
            var viewModel = _savedModel ?? new PoleCantileverViewModel();

            // Preload the pole defaults (Iron/Concrete/etc.)
            if (!viewModel.SurveillancePoles.Any())
            {
                viewModel.SurveillancePoles = new List<PoleInstance>();
            }
            if (!viewModel.ALPRPoles.Any())
            {
                viewModel.ALPRPoles = new List<PoleInstance>();
            }
            if (!viewModel.TrafficPoles.Any())
            {
                viewModel.TrafficPoles = new List<PoleInstance>();
            }

            // Initialize Gantry if null
            viewModel.Gantry ??= new Gantry { Quantity = 0 };

            return View("~/Views/SurveyCreation/PoleInfrastructure_new.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(PoleCantileverViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save current model in memory
                _savedModel = model;

                // Optional: add logic — e.g., update cantilever quantities automatically
                foreach (var poleGroup in model.SurveillancePoles)
                {
                    foreach (var c in poleGroup.Cantilevers)
                    {
                        // example rule: set each cantilever quantity to pole count if blank
                        if (c.Quantity == 0)
                            c.Quantity = 1;
                    }
                }

                TempData["SubmitStatus"] = "success";
                return RedirectToAction(nameof(Index));
            }

            TempData["SubmitStatus"] = "error";
            return View("~/Views/SurveyCreation/PoleInfrastructure_new.cshtml", model);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int poleId, int quantity)
        {
            // optional: handle quick ajax updates if you add JS later
            return Json(new { poleId, quantity, status = "updated" });
        }
    }
}
