using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOs
{
    public class BasketCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
