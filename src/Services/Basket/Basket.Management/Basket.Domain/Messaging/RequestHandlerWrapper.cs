using System;
using System.Linq;

namespace Basket.Management.Basket.Domain.Messaging
{
    public abstract class RequestHandlerBase : HandlerBase
    {
        public abstract object Handle(object request, ServiceFactory serviceFactory);

    }

    public abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
    {
        public abstract TResponse Handle(IRequest<TResponse> request, ServiceFactory serviceFactory);
    }

    public class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override object Handle(object request, ServiceFactory serviceFactory) => Handle((IRequest<TResponse>)request, serviceFactory);

        public override TResponse Handle(IRequest<TResponse> request, ServiceFactory serviceFactory)
        {
            TResponse Handler() => GetHandler<IRequestHandler<TRequest, TResponse>>(serviceFactory).Handle((TRequest)request);

            return serviceFactory
                .GetInstances<IPipelineBehavior<TRequest, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Handler, (next, pipeline) => () => pipeline.Handle((TRequest)request, next))();
        }
    }

    public abstract class HandlerBase
    {
        protected static THandler GetHandler<THandler>(ServiceFactory factory)
        {
            THandler handler;

            try
            {
                handler = factory.GetInstance<THandler>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error constructing handler for request of type {typeof(THandler)}. Register your handlers with the container. See the samples in GitHub for examples.", e);
            }

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler was not found for request of type {typeof(THandler)}. Register your handlers with the container. See the samples in GitHub for examples.");
            }

            return handler;
        }
    }
}
