using System.ComponentModel.DataAnnotations;

namespace Item.API.DTOs
{
    public class ItemCreateDto
    {
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
