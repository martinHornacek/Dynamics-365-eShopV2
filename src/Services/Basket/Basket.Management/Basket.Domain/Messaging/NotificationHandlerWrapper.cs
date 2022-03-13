using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Management.Basket.Domain.Messaging
{
    public abstract class NotificationHandlerWrapper
    {
        public abstract void Handle(INotification notification, ServiceFactory serviceFactory,
            Action<IEnumerable<Action<INotification>>, INotification> publish);
    }

    public class NotificationHandlerWrapperImpl<TNotification> : NotificationHandlerWrapper where TNotification : INotification
    {
        public override void Handle(INotification notification, ServiceFactory serviceFactory,
            Action<IEnumerable<Action<INotification>>, INotification> publish)
        {
            var handlers = serviceFactory
                .GetInstances<INotificationHandler<TNotification>>()
                .Select(x => new Action<INotification>((theNotification) => x.Handle((TNotification)theNotification)));

            publish(handlers, notification);
        }
    }

    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        void Handle(TNotification notification);
    }

    public abstract class NotificationHandler<TNotification> : INotificationHandler<TNotification> where TNotification : INotification
    {
        void INotificationHandler<TNotification>.Handle(TNotification notification)
        {
            Handle(notification);
        }

        protected abstract void Handle(TNotification notification);
    }
}
