using System.ComponentModel.DataAnnotations;

namespace Basket.API.Model
{
    public class BasketItem
    {
        [Required]
        public string new_itemid { get; set; }

        [Required]
        public int new_quantity { get; set; }

        [Required]
        public string new_basketid { get; set; }

        public Basket Basket { get; set; }
    }
}
