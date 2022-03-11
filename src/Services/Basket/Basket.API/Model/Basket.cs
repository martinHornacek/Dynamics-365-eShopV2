using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Model
{
    public class Basket
    {
        [Key]
        [Required]
        public string new_id { get; set; }
        [Required]
        public string new_name { get; set; }
        [Required]
        public string new_description { get; set; }
        [Required]
        public decimal new_totalvalue { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    }
}
