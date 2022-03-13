using System;
namespace Basket.Management.Basket.Domain.Messaging
{
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
         TResponse Handle(TRequest request);
    }

    public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit> where TRequest : IRequest<Unit>
    {
    }

    public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest
    {
        Unit IRequestHandler<TRequest, Unit>.Handle(TRequest request)
        {
            Handle(request);
            return Unit.Value;
        }

        protected abstract void Handle(TRequest request);
    }

    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        TResponse IRequestHandler<TRequest, TResponse>.Handle(TRequest request) => Handle(request);

        protected abstract TResponse Handle(TRequest request);
    }
}
