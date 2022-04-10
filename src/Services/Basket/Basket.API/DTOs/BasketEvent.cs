namespace Basket.API.DTOs
{
    public class BasketEvent
    {
        public string EventName { get; set; }
        public BasketItemDto EventArgument { get; set; }
    }
}
