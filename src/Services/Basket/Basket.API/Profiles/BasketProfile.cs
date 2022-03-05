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
            CreateMap<BasketItemCreateDto, Model.BasketItem>();
            CreateMap<BasketItemDeleteDto, Model.BasketItem>();
            CreateMap<BasketItem, BasketItemReadDto>();
            CreateMap<BasketCreateDto, Model.Basket>();
            CreateMap<Model.Basket, BasketReadDto>();
        }
    }
}
