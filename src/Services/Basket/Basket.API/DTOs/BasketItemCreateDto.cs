using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOs
{
    public class BasketItemCreateDto
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
