using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Management.Basket.Domain.Messaging
{
    public class Mediator : IMediator
    {
        private readonly ServiceFactory _serviceFactory;
        private static readonly ConcurrentDictionary<Type, RequestHandlerBase> _requestHandlers = new ConcurrentDictionary<Type, RequestHandlerBase>();
        private static readonly ConcurrentDictionary<Type, NotificationHandlerWrapper> _notificationHandlers = new ConcurrentDictionary<Type, NotificationHandlerWrapper>();
        private static readonly ConcurrentDictionary<Type, StreamRequestHandlerBase> _streamRequestHandlers = new ConcurrentDictionary<Type, StreamRequestHandlerBase>();

        public Mediator(ServiceFactory serviceFactory) => _serviceFactory = serviceFactory;

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestType = request.GetType();

            Func<Type, RequestHandlerBase> valueFactory = t =>
            {
                return (RequestHandlerBase)(Activator.CreateInstance(typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(t, typeof(TResponse)))
                                                                                 ?? throw new InvalidOperationException($"Could not create wrapper type for {t}"));
            };
            var handler = (RequestHandlerWrapper<TResponse>)_requestHandlers.GetOrAdd(requestType,
                valueFactory);

            return handler.Handle(request, _serviceFactory);
        }

        public object Send(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var requestType = request.GetType();
            Func<Type, RequestHandlerBase> valueFactory = requestTypeKey =>
                                {
                                    Func<Type, bool> predicate = i =>
                                                                {
                                                                    return i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>);
                                                                };
                                    var requestInterfaceType = requestTypeKey
                                        .GetInterfaces()
                                        .FirstOrDefault((Func<Type, bool>)predicate);

                                    if (requestInterfaceType is null)
                                    {
                                        throw new ArgumentException($"{requestTypeKey.Name} does not implement {nameof(Domain.Messaging.IRequest)}", (string)nameof(request));
                                    }

                                    var responseType = requestInterfaceType.GetGenericArguments()[(int)0];
                                    var wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType((Type)requestTypeKey, (Type)responseType);

                                    return (RequestHandlerBase)(RequestHandlerBase)(Activator.CreateInstance((Type)wrapperType)
                                                                ?? throw new InvalidOperationException((string)$"Could not create wrapper for type {wrapperType}"));
                                };
            var handler = _requestHandlers.GetOrAdd((Type)requestType,
                valueFactory);

            // call via dynamic dispatch to avoid calling through reflection for performance reasons
            return handler.Handle(request, _serviceFactory);
        }

        public void Publish<TNotification>(TNotification notification) where TNotification : INotification
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            PublishNotification(notification);
        }

        public void Publish(object notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            else if (notification is INotification)
            {
                PublishNotification((INotification)notification);
            }
            else
            {
                throw new ArgumentException($"{nameof(notification)} does not implement ${nameof(INotification)}");                    
            }
        }

        protected virtual void PublishCore(IEnumerable<Action<INotification>> allHandlers, INotification notification)
        {
            foreach (var handler in allHandlers)
            {
                handler(notification);
            }
        }

        private void PublishNotification(INotification notification)
        {
            var notificationType = notification.GetType();
            Func<Type, NotificationHandlerWrapper> valueFactory = t =>
                                {
                                    return (NotificationHandlerWrapper)(Activator.CreateInstance(typeof(NotificationHandlerWrapperImpl<>).MakeGenericType(t))
                                                                                              ?? throw new InvalidOperationException($"Could not create wrapper for type {t}"));
                                };
            var handler = _notificationHandlers.GetOrAdd(notificationType,
                valueFactory);

            handler.Handle(notification, _serviceFactory, PublishCore);
        }


        public IEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestType = request.GetType();

            var streamHandler = (StreamRequestHandlerWrapper<TResponse>)_streamRequestHandlers.GetOrAdd(requestType,
                t => (StreamRequestHandlerBase)Activator.CreateInstance(typeof(StreamRequestHandlerWrapperImpl<,>).MakeGenericType(requestType, typeof(TResponse))));

            var items = streamHandler.Handle(request, _serviceFactory);

            return items;
        }


        public IEnumerable<object> CreateStream(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestType = request.GetType();

            var handler = _streamRequestHandlers.GetOrAdd(requestType,
                requestTypeKey =>
                {
                    var requestInterfaceType = requestTypeKey
                        .GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStreamRequest<>));
                    var isValidRequest = requestInterfaceType != null;

                    if (!isValidRequest)
                    {
                        throw new ArgumentException($"{requestType.Name} does not implement IStreamRequest<TResponse>", nameof(requestTypeKey));
                    }

                    var responseType = requestInterfaceType.GetGenericArguments()[0];
                    return (StreamRequestHandlerBase)Activator.CreateInstance(typeof(StreamRequestHandlerWrapperImpl<,>).MakeGenericType(requestTypeKey, responseType));
                });

            // call via dynamic dispatch to avoid calling through reflection for performance reasons
            var items = handler.Handle(request, _serviceFactory);

            return items;
        }
    }
}
