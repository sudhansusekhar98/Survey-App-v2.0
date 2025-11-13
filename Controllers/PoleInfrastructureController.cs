using Microsoft.AspNetCore.Mvc;
using SurveyApp.Models;
using SurveyApp.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SurveyApp.Controllers
{
    public class PoleInfrastructureController : Controller
    {
        private readonly IItemRepository _itemRepo;
        private readonly IItemTypeRepository _itemTypeRepo;
        private static PoleCantileverViewModel _savedModel = new PoleCantileverViewModel();

        public PoleInfrastructureController(IItemRepository itemRepo, IItemTypeRepository itemTypeRepo)
        {
            _itemRepo = itemRepo;
            _itemTypeRepo = itemTypeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = _savedModel ?? new PoleCantileverViewModel();

            // Fetch pole items from DB (TypeId = 109)
            var poleItems = (await _itemRepo.GetByTypeAsync(109)).ToList();
            // Fetch cantilever items from DB (TypeId = 110)
            var cantileverItems = (await _itemRepo.GetByTypeAsync(110)).ToList();

            // Optionally, fetch type info if needed
            var poleType = await _itemTypeRepo.GetByIdAsync(109);
            var cantileverType = await _itemTypeRepo.GetByIdAsync(110);

            // Populate viewModel with DB data
            viewModel.Cantilevers = cantileverItems.Select(x => new CantileverItem
            {
                Name = x.ItemName,
                Length = x.ItemDesc,
                Quantity = 0
            }).ToList();

            // Example: Add pole items to SurveillancePoles (customize as needed)
            viewModel.SurveillancePoles = poleItems.Select(x => new PoleInstance
            {
                ItemId = x.ItemId,
                Name = x.ItemName,
                Description = x.ItemDesc,
                Cantilevers = viewModel.Cantilevers
            }).ToList();

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
                _savedModel = model;
                TempData["SubmitStatus"] = "success";
                return RedirectToAction(nameof(Index));
            }
            TempData["SubmitStatus"] = "error";
            return View("~/Views/SurveyCreation/PoleInfrastructure_new.cshtml", model);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int poleId, int quantity)
        {
            return Json(new { poleId, quantity, status = "updated" });
        }
    }
}
