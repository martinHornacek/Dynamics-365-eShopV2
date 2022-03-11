using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOs
{
    public class BasketItemCreateDto
    {
        [Required]
        public string new_id { get; set; }
        [Required]
        public string new_name { get; set; }
        [Required]
        public string new_itemid { get; set; }
        [Required]
        public int new_quantity { get; set; }
        [Required]
        public string new_basketid { get; set; }
    }
}
