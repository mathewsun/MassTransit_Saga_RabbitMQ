using ApiGateway.Models;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/v1/items")]
    public class ItemsController : ControllerBase
    {
        //Интерфейс MassTransit через который идет работа с сообщениями
        private readonly IBus _bus;
        private readonly ILogger<ItemsController> logger;

        public ItemsController(IBus bus, ILogger<ItemsController> logger)
        {
            _bus = bus;
            this.logger = logger;
        }

        [HttpPost("buy")]
        public async Task<BuyItemsResponse> BuyAsync(BuyItemsRequstModel model)
        {
            //Делаем запрос в шину и ждем ответа от саги. 
            //Ответ придёт из RabbitMq или словим ошибку таймаута запроса
            logger.LogInformation("Start!");
            var response = await _bus.Request<BuyItemsRequest, BuyItemsResponse>(model);
            logger.LogInformation("End!");
            //Возвращаем сообщение что было в ответе
            return response.Message;
        }
    }
}
