using MyStore.Models.DTOs;

namespace MyStore.Services.Categories
{
    public interface ICategoryService
    {
        Task<DataTableResponse> GetCategories(DataTableRequest dataTableRequest);
        Task<IEnumerable<CategoryDTO>> GetAllCategeroy();
        Task<CategoryDTO> GetCategoryById(Guid categoryId);
        Task<bool> AddCategory(CategoryAddModel model);
        Task<bool> UpdateCategory(CategoryUpdateModel model);
        Task<bool> RemoveCategory(Guid categoryId);
    }
}