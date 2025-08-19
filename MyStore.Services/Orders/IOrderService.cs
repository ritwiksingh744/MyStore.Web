using MyStore.Models.DTOs;

namespace MyStore.Services.Orders
{
    public interface IOrderService
    {
        Task<DataTableResponse> GetOrderGridData(DataTableRequest request);
        Task<bool> AddNewOrder(OrderRequestModel model);
        Task<bool> RemoveOrder(Guid orderId);
        Task<OrderResponseDTO> LoadOrderData(Guid orderId);
        Task<bool> UpdateOrderItems(Guid orderId, OrderEditRequestModel model);
    }
}