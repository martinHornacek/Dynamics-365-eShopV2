using System;
using System.Collections.Generic;
using AutoMapper;
using Basket.API.Data;
using Basket.API.DTOs;
using Basket.API.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/baskets/{basketId}/[controller]")]
    [ApiController]
    [EnableCors("BasketApiAllowSpecificOrigins")]
    public class BasketItemsController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;

        public BasketItemsController(IBasketRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BasketReadDto>> GetAllBasketItemsForBasket(int basketId)
        {
            Console.WriteLine($"--> Hit GetAllBasketItemsForBasket: {basketId}");

            if (_repository.GetBasketById(basketId) == null)
            {
                return NotFound();
            }

            var basketItems = _repository.GetAllBasketItemsForBasket(basketId);

            return Ok(_mapper.Map<IEnumerable<BasketItemReadDto>>(basketItems));
        }

        [HttpGet("{basketItemId}", Name = "GetBasketItemForBasket")]
        public ActionResult<BasketItemReadDto> GetBasketItemForBasket(int basketId, int basketItemId)
        {
            Console.WriteLine($"--> Hit GetBasketItemForBasket: {basketId} / {basketItemId}");

            if (_repository.GetBasketById(basketId) == null)
            {
                return NotFound();
            }

            var basketItem = _repository.GetBasketItemForBasket(basketId, basketItemId);

            if (basketItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BasketItemReadDto>(basketItem));
        }

        [HttpPost]
        public ActionResult<BasketItemReadDto> AddBasketItem(int basketId, BasketItemCreateDto basketItemDto)
        {
            Console.WriteLine($"--> Hit AddBasketItem: {basketId}");

            var basket = _repository.GetBasketById(basketId);

            if (basket == null)
            {
                return NotFound();
            }

            var basketItem = _mapper.Map<BasketItem>(basketItemDto);

            basketItem.BasketId = basketId;
            basketItem.Basket = basket;

            _repository.AddBasketItem(basketId, basketItem);
            _repository.SaveChanges();

            return Ok(_mapper.Map<BasketItemReadDto>(basketItem));
        }

        [HttpDelete]
        public ActionResult RemoveBasketItem(int basketId, BasketItemDeleteDto basketItemDto)
        {
            Console.WriteLine($"--> Hit RemoveBasketItem: {basketId}");

            var basket = _repository.GetBasketById(basketId);

            if (basket == null)
            {
                return NotFound();
            }

            var basketItem = _mapper.Map<BasketItem>(basketItemDto);

            _repository.RemoveBasketItem(basketId, basketItem.Id);
            _repository.SaveChanges();

            return Ok();
        }
    }
}
