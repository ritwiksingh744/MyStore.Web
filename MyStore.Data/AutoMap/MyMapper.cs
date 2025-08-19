using AutoMapper;
using MyStore.Data.Entity;
using MyStore.Models.DTOs;

namespace MyStore.Data.AutoMap
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Items, ItemDTO>().ReverseMap();
            CreateMap<Items, ItemViewModel>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderedItem, OrderedItemDTO>().ReverseMap();
        }
    }
}
