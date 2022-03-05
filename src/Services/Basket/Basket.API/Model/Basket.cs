using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Model
{
    public class Basket
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    }
}
