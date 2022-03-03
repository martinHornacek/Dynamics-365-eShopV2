using System.ComponentModel.DataAnnotations;

namespace Basket.API.Model
{
    public class Item
    { 
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
