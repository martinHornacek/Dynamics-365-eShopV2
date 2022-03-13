namespace Basket.Management.Basket.Domain.Messaging
{
    public interface IPublisher
    {
        void Publish(object notification);
        void Publish<TNotification>(TNotification notification) where TNotification : INotification;
    }
}
