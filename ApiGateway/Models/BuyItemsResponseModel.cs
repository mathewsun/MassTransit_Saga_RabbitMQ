using Contracts;

namespace ApiGateway.Models
{
    public class BuyItemsResponseModel : BuyItemsResponse
    {
        public Guid OrderId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
