using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyStore.Data.Entity;
using MyStore.Data.Repository;
using MyStore.Models.DTOs;

namespace MyStore.Services.Item
{
    public class ItemService : IItemService
    {
        private readonly IGenericRepository<Items> itemRepository;
        private readonly IMapper mapper;

        public ItemService(IGenericRepository<Items> itemRepository, IMapper mapper)
        {
            this.itemRepository = itemRepository;
            this.mapper = mapper;
        }

        public async Task<DataTableResponse> GetItemGridData(DataTableRequest dataTableRequest)
        {
            IQueryable<Items> data = null;

            //searching
            if (!string.IsNullOrEmpty(dataTableRequest.SearchValue))
            {
                data = (await itemRepository.GetAllAsync()).Include(x=>x.Category).Where(x => !x.IsDeleted && (
                    x.ItemName.ToLower().Contains(dataTableRequest.SearchValue.ToLower()) ||
                    x.Category.CategoryName.ToLower().Contains(dataTableRequest.SearchValue.ToLower())
                ));
            }
            else
            {
                data = (await itemRepository.GetAllAsync()).Include(x=>x.Category).Where(x => !x.IsDeleted);
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
            var finalResult = mapper.Map<List<ItemViewModel>>(result);
            return new DataTableResponse
            {
                Draw = dataTableRequest.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords,
                Data = finalResult
            };
        }

        public async Task<ItemDTO> GetItemDetailsById(Guid itemId)
        {
            var item = await itemRepository.GetByIdAsync(itemId);
            return mapper.Map<ItemDTO>(item);
        }

        public async Task<IEnumerable<ItemDTO>> GetItemsByCategoryId(Guid categoryId)
        {
            var items = (await itemRepository.GetAllAsync()).Where(x=> !x.IsDeleted && x.CategoryId == categoryId);
            return mapper.Map<List<ItemDTO>>(items);
        }

        public async Task<IEnumerable<ItemDTO>> GetAllItem()
        {
            var data = (await itemRepository.GetAllAsync()).Where(x=>!x.IsDeleted);
            return mapper.Map<List<ItemDTO>>(data);
        }

        public async Task<bool> AddNewItem(ItemAddRequestModel model)
        {
            var item = new Items();
            item.Id = Guid.NewGuid();
            item.CreatedOn = DateTime.Now;
            item.CategoryId = model.CategoryId;
            item.ItemName = model.ItemName;
            item.Price = model.Price;

            return (bool)await itemRepository.InsertAsync(item);
        }

        public async Task<bool> UpdateItem(Guid itemId, ItemAddRequestModel model)
        {
            var itemData = await itemRepository.GetByIdAsync(itemId);
            if(itemData != null)
            {
                bool isUpdate = false;
                if(model.CategoryId != Guid.Empty && itemData.CategoryId != model.CategoryId)
                {
                    itemData.CategoryId = model.CategoryId;
                    isUpdate = true;
                }
                if(!string.IsNullOrEmpty(model.ItemName) && itemData.ItemName.ToLower() != model.ItemName.ToLower())
                {
                    itemData.ItemName = model.ItemName;
                    isUpdate = true;
                }

                if(model.Price > 0 && model.Price != itemData.Price)
                {
                    itemData.Price = model.Price;
                    isUpdate = true;
                }
                if (isUpdate)
                {
                    itemData.UpdatedOn = DateTime.Now;
                    return (bool)await itemRepository.UpdateAsync(itemData);
                }
            }
            return false;
        }

        public async Task<bool> RemoveItem(Guid itemId)
        {
            var itemData = await itemRepository.GetByIdAsync(itemId);
            if (itemData != null)
            {
                itemData.IsDeleted = true;
                return (bool)await itemRepository.UpdateAsync(itemData,x=>x.IsDeleted);
                
            }
            return false;
        }
    }
}
