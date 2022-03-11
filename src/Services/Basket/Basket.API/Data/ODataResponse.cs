namespace Basket.API.Data
{
    public class ODataResponse<T>
    {
        public T[] Value { get; set; }
    }
}
