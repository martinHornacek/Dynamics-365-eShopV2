namespace Basket.Management.Basket.Domain.Messaging
{
    public interface IRequest : IRequest<Unit> { }
    public interface IRequest<out TResponse> : IBaseRequest { }
    public interface IBaseRequest { }
}
