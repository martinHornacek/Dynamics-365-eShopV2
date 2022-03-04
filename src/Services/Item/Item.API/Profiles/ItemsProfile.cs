
using AutoMapper;
using Item.API.DTOs;

namespace Item.API.Profiles
{
    public class ItemsProfile : Profile
    {
        public ItemsProfile()
        {
            CreateMap<Model.Item, ItemReadDto>();
            CreateMap<ItemCreateDto, Model.Item>();
        }
    }
}
