using Microsoft.AspNetCore.Mvc;
using SurveyApp.Models;
using SurveyApp.Repo;

namespace SurveyApp.Controllers
{
    public class ItemTypeController : Controller
    {
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IItemRepository _itemRepository;

        public ItemTypeController(IItemTypeRepository itemTypeRepository, IItemRepository itemRepository)
        {
            _itemTypeRepository = itemTypeRepository;
            _itemRepository = itemRepository;
        }

        // GET: ItemType/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var itemTypes = await _itemTypeRepository.GetAllAsync();
                return View(itemTypes);
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View(new List<ItemTypeMasterModel>());
            }
        }

        // GET: ItemType/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var itemType = await _itemTypeRepository.GetByIdAsync(id);
            if (itemType == null)
            {
                TempData["ResultMessage"] = "<strong>Not Found!</strong> Item type not found.";
                TempData["ResultType"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            return View(itemType);
        }

        // GET: ItemType/Create
        public IActionResult Create()
        {
            return View(new ItemTypeMasterModel());
        }

        // POST: ItemType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemTypeMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ResultMessage"] = "<strong>Validation Error!</strong> Please check all required fields.";
                    TempData["ResultType"] = "warning";
                    return View(model);
                }

                model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "0");
                
                await _itemTypeRepository.CreateAsync(model);

                TempData["ResultMessage"] = "<strong>Success!</strong> Item type created successfully.";
                TempData["ResultType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View(model);
            }
        }

        // GET: ItemType/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var itemType = await _itemTypeRepository.GetByIdAsync(id);
            if (itemType == null)
            {
                TempData["ResultMessage"] = "<strong>Not Found!</strong> Item type not found.";
                TempData["ResultType"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            return View(itemType);
        }

        // POST: ItemType/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemTypeMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ResultMessage"] = "<strong>Validation Error!</strong> Please check all required fields.";
                    TempData["ResultType"] = "warning";
                    return View(model);
                }

                model.ModifiedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "0");

                await _itemTypeRepository.UpdateAsync(model);

                TempData["ResultMessage"] = "<strong>Success!</strong> Item type updated successfully.";
                TempData["ResultType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View(model);
            }
        }

        // POST: ItemType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _itemTypeRepository.DeleteAsync(id);

                if (result)
                {
                    return Json(new { success = true, message = "Item type deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete item type." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: ItemType/Items/5 - Get items by type
        public async Task<IActionResult> Items(int id)
        {
            try
            {
                var itemType = await _itemTypeRepository.GetByIdAsync(id);
                if (itemType == null)
                {
                    TempData["ResultMessage"] = "<strong>Not Found!</strong> Item type not found.";
                    TempData["ResultType"] = "warning";
                    return RedirectToAction(nameof(Index));
                }

                var items = await _itemRepository.GetByTypeAsync(id);
                ViewBag.ItemType = itemType;
                return View(items);
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
