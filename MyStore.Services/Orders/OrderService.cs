using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyStore.Data.Entity;
using MyStore.Data.Repository;
using MyStore.Models.DTOs;
using MyStore.Services.Item;

namespace MyStore.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> orderRepository;
        private readonly IGenericRepository<OrderedItem> orderedItemRepository;
        private readonly IItemService itemService;
        private readonly IMapper mapper;
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<OrderedItem> orderedItemRepository,IItemService itemService, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.orderedItemRepository = orderedItemRepository;
            this.itemService = itemService;
            this.mapper = mapper;
        }

        public async Task<DataTableResponse> GetOrderGridData(DataTableRequest request)
        {
            IQueryable<Order> data = null;

            //searching
            if (!string.IsNullOrEmpty(request.SearchValue))
            {
                data = (await orderRepository.GetAllAsync()).Where(x => !x.IsDeleted &&
                (x.OrderNumber.ToLower().Contains(request.SearchValue.ToLower()) ||
                x.Total.ToString() == request.SearchValue ||
                x.CustomerName.ToLower().Contains(request.SearchValue.ToLower())
                ));
            }
            else
            {
                data = (await orderRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            }

            //sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && !string.IsNullOrEmpty(request.SortDirection))
            {
                string columnName = char.ToUpper(request.SortColumn[0]) + request.SortColumn.Substring(1);
                if (request.SortDirection == "asc")
                    data = data.OrderBy(x => EF.Property<object>(x, columnName));
                else
                    data = data.OrderByDescending(y => EF.Property<object>(y, columnName));
            }

            //paging
            var totalRecords = data.Count();
            var result = await data.Skip(request.Start).Take(request.Length).ToListAsync();
            var finalResult = mapper.Map<List<OrderDTO>>(result);
            return new DataTableResponse
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords,
                Data = finalResult
            };
        }

        public async Task<bool> AddNewOrder(OrderRequestModel model)
        {
            //create new order
            var order = new Order();
            order.Id = Guid.NewGuid();
            order.CreatedOn = DateTime.Now;
            order.OrderNumber = await GenerateRandomCode();
            order.CustomerName = model.CustomerName;
            order.OrderDate = model.OrderDate;
            order.Total = await CalculateTotalPrice(model.Items);

            var orderStatus = (bool)await orderRepository.InsertAsync(order);
            if (orderStatus) //if order created successfully
            {
                var newItems = new List<OrderedItem>();
                foreach (var item in model.Items) 
                {
                    newItems.Add(new OrderedItem
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        OrderId = order.Id,
                        ItemId = item.ItemId,
                        CategoryId = item.CategoryId,
                        Quantity = item.Quantity,
                        Rate = item.Rate
                    });
                }

                var result =(bool)await orderedItemRepository.BulkInsertAsync(newItems);

            }
            return orderStatus;
        }

        public async Task<bool> RemoveOrder(Guid orderId)
        {
            var orderData = await orderRepository.GetByIdAsync(orderId);
            if (orderData != null)
            {
                orderData.IsDeleted = true;
                var result = (bool)await orderRepository.UpdateAsync(orderData);
                if (result)
                {
                    var orderedItems = (await orderedItemRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.OrderId == orderData.Id);
                    if(orderedItems.Count()> 0)
                    {
                        var data = orderedItems.ToList();
                        foreach(var item in data) { item.IsDeleted = true; }
                        var response = await orderedItemRepository.BulkUpdateAsync(data);
                    }
                }
                return result;
            }
            return false;
        }

        public async Task<OrderResponseDTO> LoadOrderData(Guid orderId)
        {
            var response = new OrderResponseDTO();
            var orderData = await orderRepository.GetByIdAsync(orderId);
            if (orderData != null) 
            {
                response.OrderId = orderData.Id;
                response.OrderNumber = orderData.OrderNumber;
                response.OrderDate = orderData.OrderDate;
                response.CustomerName = orderData.CustomerName;
                response.Total = orderData.Total;

                var itemsData = (await orderedItemRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.OrderId == orderData.Id);
                if(itemsData.Count()> 0)
                {
                    response.Items = mapper.Map<List<OrderedItemDTO>>(itemsData);
                }
            }
            return response;
        }

        public async Task<bool> UpdateOrderItems(Guid orderId, OrderEditRequestModel model)
        {
            //update total in order
            var orderData = await orderRepository.GetByIdAsync(orderId);
            if (orderData != null)
            {
                bool response = false;
                var total = await CalculateTotalPrice(model.Items);
                if (total != orderData.Total)
                {
                    orderData.Total = total;
                    response = (bool) await orderRepository.UpdateAsync(orderData);
                }

                //update order items
                var existingItems = (await orderedItemRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.OrderId == orderId);
                var res = (bool)await orderedItemRepository.BulkDelete(existingItems);
                if (res && response)
                {
                    var newItems = new List<OrderedItem>();
                    foreach (var item in model.Items) 
                    {
                        newItems.Add(new OrderedItem
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            OrderId = orderData.Id,
                            ItemId = item.ItemId,
                            CategoryId = item.CategoryId,
                            Quantity = item.Quantity,
                            Rate = item.Rate
                        });
                    }
                    var result = (bool) await orderedItemRepository.BulkInsertAsync(newItems);
                }

                return response;
            }
            return false;
        }

        private async Task<string> GenerateRandomCode(int length = 6)
        {
            Random random = new Random();

            while (true)
            {
                char[] code = new char[length];
                for (int i = 0; i < length; i++)
                {
                    code[i] = _chars[random.Next(_chars.Length)];
                }

                string newCode = new string(code);

                if (!await IsCodeAlreadyExist(newCode))
                    return newCode; // found unique code
            }
        }

        private async Task<bool> IsCodeAlreadyExist(string code)
        {
            var data = (await orderRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.OrderNumber == code);
            if (data.Count() > 0)
                return true;
            return false;
        }
        
        private async  Task<double> CalculateTotalPrice(List<ItemRequestModel> items)
        {
            double totalPrice = 0;
            foreach (var item in items) 
            {
                var itemData = await itemService.GetItemDetailsById(item.ItemId);
                totalPrice += (itemData.Price * item.Quantity);
            }
            return totalPrice;
        }
    }
}
