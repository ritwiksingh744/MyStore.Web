using MyStore.Models.DTOs;

namespace MyStore.Services.Item
{
    public interface IItemService
    {
        Task<DataTableResponse> GetItemGridData(DataTableRequest dataTableRequest);
        Task<ItemDTO> GetItemDetailsById(Guid itemId);
        Task<IEnumerable<ItemDTO>> GetItemsByCategoryId(Guid categoryId);
        Task<IEnumerable<ItemDTO>> GetAllItem();
        Task<bool> AddNewItem(ItemAddRequestModel model);
        Task<bool> UpdateItem(Guid itemId, ItemAddRequestModel model);
        Task<bool> RemoveItem(Guid itemId);
    }
}