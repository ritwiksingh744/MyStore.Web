using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyStore.Data.Entity;
using MyStore.Data.Repository;
using MyStore.Models.DTOs;

namespace MyStore.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> categoryRepository;
        private readonly IGenericRepository<Items> itemRepository;
        private readonly IMapper mapper;

        public CategoryService(IGenericRepository<Category> categoryRepository, IGenericRepository<Items> itemRepository,IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.itemRepository = itemRepository;
            this.mapper = mapper;
        }

        public async Task<DataTableResponse> GetCategories(DataTableRequest dataTableRequest)
        {
            IQueryable<Category> data = null;

            //searching
            if (!string.IsNullOrEmpty(dataTableRequest.SearchValue))
            {
                data = (await categoryRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.CategoryName.ToLower().Contains(dataTableRequest.SearchValue.ToLower()));
            }
            else
            {
                data = (await categoryRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            }


            //sorting
            if (!string.IsNullOrEmpty(dataTableRequest.SortColumn) && !string.IsNullOrEmpty(dataTableRequest.SortDirection))
            {
                string columnName = char.ToUpper(dataTableRequest.SortColumn[0]) + dataTableRequest.SortColumn.Substring(1);
                if (dataTableRequest.SortDirection == "asc")
                    data = data.OrderBy(x => EF.Property<object>(x, columnName));
                else
                    data = data.OrderByDescending(y => EF.Property<object>(y, columnName));
            }

            //paging
            var totalRecords = data.Count();
            var result = await data.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToListAsync();
            var finalResult = mapper.Map<List<CategoryDTO>>(result);
            return new DataTableResponse
            {
                Draw = dataTableRequest.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords,
                Data = finalResult
            };
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategeroy()
        {
            var data = (await categoryRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            return mapper.Map<List<CategoryDTO>>(data);
        }

        public async Task<CategoryDTO> GetCategoryById(Guid categoryId)
        {
            var data = await categoryRepository.GetByIdAsync(categoryId);
            return mapper.Map<CategoryDTO>(data);
        }

        public async Task<bool> AddCategory(CategoryAddModel model)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.CategoryName = model.CategoryName;
            category.CreatedOn = DateTime.Now;

            return (bool)await categoryRepository.InsertAsync(category);
        }

        public async Task<bool> UpdateCategory(CategoryUpdateModel model)
        {
            var category = await categoryRepository.GetByIdAsync(Guid.Parse(model.Id));
            if (category != null)
            {
                if (!string.IsNullOrEmpty(category.CategoryName) && category.CategoryName != model.CategoryName) 
                {
                    category.CategoryName = model.CategoryName;
                    category.UpdatedOn = DateTime.Now;
                    return (bool)await categoryRepository.UpdateAsync(category, x => x.CategoryName, x => x.UpdatedOn);
                }

            }
            return false;
        }

        public async Task<bool> RemoveCategory(Guid categoryId)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category != null && !category.IsDeleted) 
            {
                if (await IsCategoryRemovable(categoryId))
                {
                    category.IsDeleted = true;
                    return (bool)await categoryRepository.UpdateAsync(category, x => x.IsDeleted);
                }
                else
                    throw new Exception("Category already in use. Can't be deleted.");
            }
            return false;
        }

        private async Task<bool> IsCategoryRemovable(Guid categoryId)
        {
            var items = (await itemRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.CategoryId == categoryId);

            if (items.Count() > 0) 
            {
                return false;
            }
            return true;
        }
    }
}
