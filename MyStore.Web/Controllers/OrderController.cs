using Microsoft.AspNetCore.Mvc;
using MyStore.Models.DTOs;
using MyStore.Services.Categories;
using MyStore.Services.Item;
using MyStore.Services.Orders;

namespace MyStore.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly IItemService itemService;

        public OrderController(IOrderService orderService, ICategoryService categoryService, IItemService itemService)
        {
            this.orderService = orderService;
            this.categoryService = categoryService;
            this.itemService = itemService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadOrderData()
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

            var result = await orderService.GetOrderGridData(request);
            return Json(result);
        }

        public async Task<IActionResult> OrderAddView()
        {
            var categoryList = await categoryService.GetAllCategeroy();
            return View(categoryList);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder([FromBody]OrderRequestModel model)
        {
            var result = await orderService.AddNewOrder(model);
            if(result)
                return RedirectToAction("Index");

            return RedirectToAction("OrderAddView");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveOrder([FromQuery]string orderId)
        {
            var response = await orderService.RemoveOrder(Guid.Parse(orderId));
            if (response)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Unable to remove order. Please try after sometime.");
        }

        
        public async Task<IActionResult> OrderEditView([FromQuery]string orderId)
        {
            var data = await orderService.LoadOrderData(Guid.Parse(orderId));
            data.Categories = await categoryService.GetAllCategeroy();
            data.CategoryItems = await itemService.GetAllItem();
            return View(data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderItems(string orderId, [FromBody] OrderEditRequestModel model)
        {
            var response = await orderService.UpdateOrderItems(Guid.Parse(orderId), model);
            if (response)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Unable to update order items. Please try after sometime.");
        }


    }
}
