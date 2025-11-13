using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SurveyApp.Models;
using SurveyApp.Repo;

namespace SurveyApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemTypeRepository _itemTypeRepository;

        public ItemController(IItemRepository itemRepository, IItemTypeRepository itemTypeRepository)
        {
            _itemRepository = itemRepository;
            _itemTypeRepository = itemTypeRepository;
        }

        // GET: Item/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();
                return View(items);
            }
            catch (Exception ex)
            {
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View(new List<ItemMasterModel>());
            }
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                TempData["ResultMessage"] = "<strong>Not Found!</strong> Item not found.";
                TempData["ResultType"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        // GET: Item/Create
        public async Task<IActionResult> Create()
        {
            await PopulateItemTypesDropdown();
            return View(new ItemMasterModel());
        }

        // POST: Item/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateItemTypesDropdown(model.TypeId);
                    TempData["ResultMessage"] = "<strong>Validation Error!</strong> Please check all required fields.";
                    TempData["ResultType"] = "warning";
                    return View(model);
                }

                model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID") ?? "0");

                await _itemRepository.CreateAsync(model);

                TempData["ResultMessage"] = "<strong>Success!</strong> Item created successfully.";
                TempData["ResultType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await PopulateItemTypesDropdown(model.TypeId);
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View(model);
            }
        }

        // GET: Item/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                TempData["ResultMessage"] = "<strong>Not Found!</strong> Item not found.";
                TempData["ResultType"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            await PopulateItemTypesDropdown(item.TypeId);
            return View(item);
        }

        // POST: Item/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateItemTypesDropdown(model.TypeId);
                    TempData["ResultMessage"] = "<strong>Validation Error!</strong> Please check all required fields.";
                    TempData["ResultType"] = "warning";
                    return View(model);
                }

                await _itemRepository.UpdateAsync(model);

                TempData["ResultMessage"] = "<strong>Success!</strong> Item updated successfully.";
                TempData["ResultType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await PopulateItemTypesDropdown(model.TypeId);
                TempData["ResultMessage"] = $"<strong>Error!</strong> {ex.Message}";
                TempData["ResultType"] = "danger";
                return View(model);
            }
        }

        // POST: Item/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _itemRepository.DeleteAsync(id);

                if (result)
                {
                    return Json(new { success = true, message = "Item deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete item." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task PopulateItemTypesDropdown(int? selectedTypeId = null)
        {
            var itemTypes = await _itemTypeRepository.GetAllAsync();
            ViewBag.ItemTypes = new SelectList(itemTypes, "Id", "TypeName", selectedTypeId);
        }
    }
}
