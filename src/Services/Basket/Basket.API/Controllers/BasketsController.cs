using AutoMapper;
using Basket.API.Data;
using Basket.API.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("BasketApiAllowSpecificOrigins")]
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
        public async Task<ActionResult<IEnumerable<BasketReadDto>>> GetBaskets()
        {
            Console.WriteLine("--> Getting Baskets....");

            var baskets = await _repository.GetAllBaskets();

            return Ok(_mapper.Map<IEnumerable<BasketReadDto>>(baskets));
        }

        [HttpGet("{id}", Name = "GetBasketById")]
        public async Task<ActionResult<BasketReadDto>> GetBasketById(string id)
        {
            var basket = await _repository.GetBasketById(id);
            if (basket == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<BasketReadDto>(basket));
        }

        [HttpPost]
        public async Task<ActionResult<BasketReadDto>> CreateBasket(BasketCreateDto basketCreateDto)
        {
            var basketModel = _mapper.Map<Model.Basket>(basketCreateDto);
            await _repository.CreateBasket(basketModel);

            var basketReadDto = _mapper.Map<BasketReadDto>(basketModel);
            return CreatedAtRoute(nameof(GetBasketById), new { Id = basketReadDto.new_id }, basketReadDto);
        }
    }
}
