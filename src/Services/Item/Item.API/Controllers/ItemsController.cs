using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Item.API.Data;
using Item.API.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Item.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("BasketApiAllowSpecificOrigins")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;

        public ItemsController(IItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<ItemPayload> GetItems([FromQuery] QueryStringParameters queryParameters)
        {
            var items = _repository.GetAllItems();
            IEnumerable<ItemReadDto> returnItems = _mapper.Map<IEnumerable<ItemReadDto>>(items).OrderBy(on => on.Id);

            if (queryParameters.Name != null && !queryParameters.Name.Trim().Equals(string.Empty))
                returnItems = returnItems.Where(item => item.Name.ToLower().Contains(queryParameters.Name.Trim().ToLower()));

            if (queryParameters.Category != null && !queryParameters.Category.Trim().Equals(string.Empty))
            {
                string[] categories = queryParameters.Category.Split('#');
                returnItems = returnItems.Where(item => categories.Contains(item.Category));
            }

            //get total count before paging
            int count = returnItems.Count();

            returnItems = returnItems
                            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                            .Take(queryParameters.PageSize);

            List<ItemReadDto> list = returnItems.ToList();

            return Ok(new ItemPayload(list, count));
        }

        [HttpGet("{id}", Name = "GetItemById")]
        public ActionResult<ItemReadDto> GetItemById(int id)
        {
            var item = _repository.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ItemReadDto>(item));
        }

        [HttpPost]
        public ActionResult<ItemReadDto> CreateItem(ItemCreateDto itemCreateDto)
        {
            var itemModel = _mapper.Map<Model.Item>(itemCreateDto);
            _repository.CreateItem(itemModel);
            _repository.SaveChanges();

            var itemReadDto = _mapper.Map<ItemReadDto>(itemModel);

            return CreatedAtRoute(nameof(GetItemById), new { Id = itemReadDto.Id }, itemReadDto);
        }
    }

}
