using Contracts;
using MassTransit;

namespace ItemsMicroservice.Consumers
{
    public class GetItemsConsumer : IConsumer<IGetItemsRequest>
    {
        public Task Consume(ConsumeContext<IGetItemsRequest> context)
        {
            return context.RespondAsync<IGetItemsResponse>(new { OrderId = context.Message.OrderId });
        }
    }
}
