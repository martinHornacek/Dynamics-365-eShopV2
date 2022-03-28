namespace Basket.Management.Basket.CustomActions.Model
{
    public class BasketEvent
    {
        public string EventName { get; set; }
        public BasketItemDto EventArgument { get; set; }
    }
}
