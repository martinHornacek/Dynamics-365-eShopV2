using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Management.Basket.Domain.Messaging
{
    public abstract class StreamRequestHandlerBase : HandlerBase
    {
        public abstract IEnumerable<object> Handle(object request, ServiceFactory serviceFactory);
    }

    public abstract class StreamRequestHandlerWrapper<TResponse> : StreamRequestHandlerBase
    {
        public abstract IEnumerable<TResponse> Handle(IStreamRequest<TResponse> request, ServiceFactory serviceFactory);
    }

    public class StreamRequestHandlerWrapperImpl<TRequest, TResponse> : StreamRequestHandlerWrapper<TResponse> where TRequest : IStreamRequest<TResponse>
    {
        public override IEnumerable<object> Handle(object request, ServiceFactory serviceFactory)
        {
            foreach (var item in Handle((IStreamRequest<TResponse>)request, serviceFactory))
            {
                yield return item;
            }
        }

        public override IEnumerable<TResponse> Handle(IStreamRequest<TResponse> request, ServiceFactory serviceFactory)
        {
            IEnumerable<TResponse> Handler() => GetHandler<IStreamRequestHandler<TRequest, TResponse>>(serviceFactory).Handle((TRequest)request);

            var items = serviceFactory
                .GetInstances<IStreamPipelineBehavior<TRequest, TResponse>>()
                .Reverse()
                .Aggregate(
                    (StreamHandlerDelegate<TResponse>)Handler,
                    (next, pipeline) => () => pipeline.Handle(
                        (TRequest)request,
                        () => NextWrapper(next())
                    )
                )();

            foreach (var item in items)
            {
                yield return item;
            }
        }

        private static IEnumerable<T> NextWrapper<T>(IEnumerable<T> items)
        { 
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }

    public interface IStreamRequestHandler<in TRequest, out TResponse> where TRequest : IStreamRequest<TResponse>
    {
        IEnumerable<TResponse> Handle(TRequest request);
    }

    public delegate IEnumerable<TResponse> StreamHandlerDelegate<out TResponse>();

    public interface IStreamPipelineBehavior<in TRequest, TResponse> where TRequest : IStreamRequest<TResponse>
    {
        IEnumerable<TResponse> Handle(TRequest request, StreamHandlerDelegate<TResponse> next);
    }
}
