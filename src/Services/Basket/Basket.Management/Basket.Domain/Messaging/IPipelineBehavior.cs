namespace Basket.Management.Basket.Domain.Messaging
{
    public delegate TResponse RequestHandlerDelegate<TResponse>();

    public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest request, RequestHandlerDelegate<TResponse> next);
    }
}
