using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOs
{
    public class BasketCreateDto
    {
        [Required]
        public string new_id { get; set; }
        [Required]
        public string new_name { get; set; }
        [Required]
        public string new_description { get; set; }
        [Required]
        public decimal new_totalvalue { get; set; }
    }
}
