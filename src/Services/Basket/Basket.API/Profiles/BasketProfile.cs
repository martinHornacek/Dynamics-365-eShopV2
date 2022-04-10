using AutoMapper;
using Basket.API.DTOs;
using Basket.API.Model;

namespace Basket.API.Profiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            // Source -> Target
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<BasketItem, BasketItemDto>();
            CreateMap<BasketCreateDto, Model.Basket>();
            CreateMap<Model.Basket, BasketReadDto>();
        }
    }
}
