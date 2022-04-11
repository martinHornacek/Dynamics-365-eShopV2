using System.ComponentModel.DataAnnotations;

namespace Item.API.Model
{
    public class Item
    {
        [Key]
        [Required]
        public string new_id { get; set; }
        [Required]
        public string new_name { get; set; }
        [Required]
        public decimal new_price { get; set; }
        [Required]
        public string new_category { get; set; }
        [Required]
        public string new_description { get; set; }
    }
}
