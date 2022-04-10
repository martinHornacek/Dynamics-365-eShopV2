using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<BasketReadDto>>> GetAllBasketItemsForBasket(string basketId)
        {
            Console.WriteLine($"--> Hit GetAllBasketItemsForBasket: {basketId}");

            if (await _repository.GetBasketById(basketId) == null)
            {
                return NotFound();
            }

            var basketItems = await _repository.GetAllBasketItemsForBasket(basketId);

            return Ok(_mapper.Map<IEnumerable<BasketItemDto>>(basketItems));
        }

        [HttpGet("{basketItemId}", Name = "GetBasketItemForBasket")]
        public async Task<ActionResult<BasketItemDto>> GetBasketItemForBasket(string basketId, string basketItemId)
        {
            Console.WriteLine($"--> Hit GetBasketItemForBasket: {basketId} / {basketItemId}");

            if (await _repository.GetBasketById(basketId) == null)
            {
                return NotFound();
            }

            var basketItem = await _repository.GetBasketItemForBasket(basketId, basketItemId);

            if (basketItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BasketItemDto>(basketItem));
        }

        [HttpPost]
        public async Task<ActionResult<BasketItemDto>> AddBasketItem(string basketId, BasketItemDto basketItemDto)
        {
            Console.WriteLine($"--> Hit AddBasketItem: {basketId}");

            var basket = await _repository.GetBasketById(basketId);

            if (basket == null)
            {
                return NotFound();
            }

            await _repository.AddBasketItem(basketItemDto);
            var basketItem = _mapper.Map<BasketItem>(basketItemDto);

            return Ok(_mapper.Map<BasketItemDto>(basketItem));
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(string basketId, BasketItemDto basketItemDto)
        {
            Console.WriteLine($"--> Hit RemoveBasketItem: {basketId}");

            var basketItem = await _repository.GetBasketItemForBasket(basketId, basketItemDto.new_itemid);

            if (basketItem == null)
            {
                return NotFound();
            }

            await _repository.RemoveBasketItem(basketItem.new_basketid, basketItem.new_itemid);

            return Ok();
        }
    }
}
