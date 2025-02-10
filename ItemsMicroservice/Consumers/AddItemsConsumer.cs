using Contracts;
using MassTransit;

namespace ItemsMicroservice.Consumers
{
    public class AddItemsConsumer : IConsumer<IAddItemsRequest>
    {
        public Task Consume(ConsumeContext<IAddItemsRequest> context)
        {
            return context.RespondAsync<IAddItemsResponse>(new { context.Message.OrderId });
        }
    }
}
