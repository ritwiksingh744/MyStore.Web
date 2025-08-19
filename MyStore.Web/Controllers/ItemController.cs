using Microsoft.AspNetCore.Mvc;
using MyStore.Models.DTOs;
using MyStore.Services.Categories;
using MyStore.Services.Item;

namespace MyStore.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService itemService;
        private readonly ICategoryService categoryService;

        public ItemController(IItemService itemService, ICategoryService categoryService)
        {
            this.itemService = itemService;
            this.categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await categoryService.GetAllCategeroy();

            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> LoadItemGridData()
        {
            var request = new DataTableRequest
            {
                Draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0"),
                Start = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0"),
                Length = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "10"),
                SearchValue = Request.Form["search[value]"].FirstOrDefault(),
                SortColumn = Request.Form[$"columns[{Request.Form["order[0][column]"].FirstOrDefault()}][data]"].FirstOrDefault(),
                SortDirection = Request.Form["order[0][dir]"].FirstOrDefault()
            };

            var result = await itemService.GetItemGridData(request);

            return Json(result);
        }

        public async Task<IActionResult> GetItemDataById([FromQuery]string itemId)
        {
            var data = await itemService.GetItemDetailsById(Guid.Parse(itemId));
            return Json(data);
        }
        public async Task<IActionResult> GetItemsByCategoryId([FromQuery] string categoryId)
        {
            var data = await itemService.GetItemsByCategoryId(Guid.Parse(categoryId));
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewItem([FromForm] ItemAddRequestModel model)
        {
            var result = await itemService.AddNewItem(model);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Unable to add new item. Please try again after sometime.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem(string itemId, [FromForm]ItemAddRequestModel model)
        {
            var response = await itemService.UpdateItem(Guid.Parse(itemId), model);
            if (response)
            {
                return Ok("Succesfully upated.");
            }
            return BadRequest("No changes done.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem([FromQuery]string itemId)
        {
            var response = await itemService.RemoveItem(Guid.Parse(itemId));
            if (response)
            {
                return Ok("Successfully removed.");
            }
            return BadRequest("Unable to delete item. Please try again after sometime.");
        }


    }
}
