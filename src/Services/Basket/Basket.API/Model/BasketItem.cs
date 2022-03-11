using System.ComponentModel.DataAnnotations;

namespace Basket.API.Model
{
    public class BasketItem
    {
        [Key]
        [Required]
        public string new_id { get; set; }

        [Required]
        public string new_itemid { get; set; }

        [Required]
        public int new_quantity { get; set; }

        [Required]
        public string new_basketid { get; set; }

        public string new_basketitemid { get; set; } // CRM Guid

        public Basket Basket { get; set; }
    }
}
