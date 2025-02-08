using MassTransit;
using MoneyMicroservice.Models;

namespace MoneyMicroservice
{
    public class GetMoneyConsumer : IConsumer<IGetMoneyRequest>
    {
        public Task Consume(ConsumeContext<IGetMoneyRequest> context)
        {
            return context.RespondAsync<IGetMoneyResponse>(new { context.Message.OrderId });
        }
    }
}
