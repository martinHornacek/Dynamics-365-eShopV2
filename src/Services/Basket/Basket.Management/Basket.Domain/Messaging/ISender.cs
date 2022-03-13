using System.Collections.Generic;

namespace Basket.Management.Basket.Domain.Messaging
{
    public interface ISender
    {
        TResponse Send<TResponse>(IRequest<TResponse> request);
        object Send(object request);
        IEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request);
        IEnumerable<object> CreateStream(object request);
    }
}
