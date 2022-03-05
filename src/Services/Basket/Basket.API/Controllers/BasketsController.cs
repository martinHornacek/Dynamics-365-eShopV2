using System;
using System.Collections.Generic;
using AutoMapper;
using Basket.API.Data;
using Basket.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BasketReadDto>> GetBaskets()
        {
            Console.WriteLine("--> Getting Baskets....");

            var baskets = _repository.GetAllBaskets();

            return Ok(_mapper.Map<IEnumerable<BasketReadDto>>(baskets));
        }

        [HttpGet("{id}", Name = "GetBasketById")]
        public ActionResult<BasketReadDto> GetBasketById(int id)
        {
            var basket = _repository.GetBasketById(id);
            if (basket == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<BasketReadDto>(basket));
        }

        [HttpPost]
        public ActionResult<BasketReadDto> CreateBasket(BasketCreateDto basketCreateDto)
        {
            var basketModel = _mapper.Map<Model.Basket>(basketCreateDto);
            _repository.CreateBasket(basketModel);
            _repository.SaveChanges();

            var basketReadDto = _mapper.Map<BasketReadDto>(basketModel);

            return CreatedAtRoute(nameof(GetBasketById), new { Id = basketReadDto.Id }, basketReadDto);
        }
    }
}
