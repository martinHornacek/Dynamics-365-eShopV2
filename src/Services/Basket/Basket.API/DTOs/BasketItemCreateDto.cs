using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public string new_itemid { get; set; }
        [Required]
        public string new_basketid { get; set; }
        public int? new_quantity { get; set; }
    }
}
