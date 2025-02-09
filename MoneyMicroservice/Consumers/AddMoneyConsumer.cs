using Contracts;
using MassTransit;

namespace MoneyMicroservice.Consumers
{
    public class AddMoneyConsumer : IConsumer<IAddMoneyRequest>
    {
        public Task Consume(ConsumeContext<IAddMoneyRequest> context)
        {
            return context.RespondAsync<IAddMoneyResponse>(new { context.Message.OrderId });
        }
    }
}
