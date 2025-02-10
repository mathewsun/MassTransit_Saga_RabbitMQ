using System.Linq.Expressions;
using Automatonymous;
using Contracts;
using MassTransit;

public sealed class BuyItemsSaga : MassTransitStateMachine<BuyItemsSagaState>
{
    private readonly ILogger<BuyItemsSaga> _logger;

    public BuyItemsSaga(ILogger<BuyItemsSaga> logger)
    {
        _logger = logger;
        InstanceState(x => x.CurrentState);
        Event<BuyItemsRequest>(() => BuyItems, x => x.CorrelateById(y => y.Message.OrderId));
        Request(
            () => GetMoney
            );
        Request(
         () => GetItems
         );

        Initially(

            When(BuyItems)
            .Then(x =>
            {
                if (!x.TryGetPayload(out SagaConsumeContext<BuyItemsSagaState, BuyItemsRequest> payload))
                    throw new Exception("Unable to retrieve required payload for callback data.");
                x.Saga.RequestId = payload.RequestId;
                x.Saga.ResponseAddress = payload.ResponseAddress;
            })
            .Request(GetMoney, x => x.Init<IGetMoneyRequest>(new { OrderId = x.Data.OrderId }))
            .TransitionTo(GetMoney.Pending)

            );

        During(GetMoney.Pending,

            When(GetMoney.Completed)
            .Request(GetItems, x => x.Init<IGetItemsRequest>(new { OrderId = x.Data.OrderId }))
            .TransitionTo(GetItems.Pending),

            When(GetMoney.Faulted)
              .ThenAsync(async context =>
              {
                  await RespondFromSaga(context, "Faulted On Get Money " + string.Join("; ", context.Data.Exceptions.Select(x => x.Message)));
              })
            .TransitionTo(Failed),

            When(GetMoney.TimeoutExpired)
               .ThenAsync(async context =>
               {
                   await RespondFromSaga(context, "Timeout Expired On Get Money");
               })
            .TransitionTo(Failed)

             );

        During(GetItems.Pending,

            When(GetItems.Completed)
              .ThenAsync(async context =>
              {
                  await RespondFromSaga(context, null);
              })
            .Finalize(),

            When(GetItems.Faulted)
              .ThenAsync(async context =>
              {
                  await RespondFromSaga(context, "Faulted On Get Items " + string.Join("; ", context.Data.Exceptions.Select(x => x.Message)));
              })
            .TransitionTo(Failed),

            When(GetItems.TimeoutExpired)
               .ThenAsync(async context =>
               {
                   await RespondFromSaga(context, "Timeout Expired On Get Items");
               })
            .TransitionTo(Failed)

            );
    }

    public Request<BuyItemsSagaState, IGetMoneyRequest, IGetMoneyResponse> GetMoney { get; set; }
    public Request<BuyItemsSagaState, IGetItemsRequest, IGetItemsResponse> GetItems { get; set; }
    public Event<BuyItemsRequest> BuyItems { get; set; }
    public State Failed { get; set; }

    private static async Task RespondFromSaga<T>(BehaviorContext<BuyItemsSagaState, T> context, string error) where T : class
    {
        var endpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress);
        await endpoint.Send(new BuyItemsResponse
        {
            OrderId = context.Saga.CorrelationId,
            ErrorMessage = error
        }, r => r.RequestId = context.Saga.RequestId);
    }
}
