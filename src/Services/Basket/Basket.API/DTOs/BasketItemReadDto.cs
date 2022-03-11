namespace Basket.API.DTOs
{
    public class BasketItemReadDto
    {
        public string new_id { get; set; }
        public string new_itemid { get; set; }
        public int new_quantity { get; set; }
        public string new_basketid { get; set; }
        public string new_basketitemdid { get; set; } // CRM Guid
    }
}
