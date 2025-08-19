using Microsoft.AspNetCore.Mvc;
using MyStore.Models.DTOs;
using MyStore.Services.Categories;

namespace MyStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadCategoryData()
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

            var result = await categoryService.GetCategories(request);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm]CategoryAddModel model)
        {
            var data = await categoryService.AddCategory(model);
            if (data)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Unable to add category. Please try after sometime.");
        }
        
        public async Task<IActionResult> GetCategoryDetail([FromQuery]string categoryId)
        {
            var data = await categoryService.GetCategoryById(Guid.Parse(categoryId));
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory([FromForm]CategoryUpdateModel model)
        {
            var data = await categoryService.UpdateCategory(model);
            if (data)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Unable to update category. Please try after sometime.");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCategory([FromQuery] string categoryId)
        {
            try
            {
                var data = await categoryService.RemoveCategory(Guid.Parse(categoryId));
                if (data)
                {
                    return RedirectToAction("Index");
                }
                throw new Exception("Unable to update category. Please try after sometime.");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetAllCategory()
        {
            var data = await categoryService.GetAllCategeroy();
            return Json(data);
        }
    }
}
