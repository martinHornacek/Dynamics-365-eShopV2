using System.ComponentModel.DataAnnotations;

namespace Basket.API.Model
{
    public class BasketItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int BasketId { get; set; }

        public Basket Basket { get; set; }
    }
}
