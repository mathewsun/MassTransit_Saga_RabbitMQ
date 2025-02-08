namespace MoneyMicroservice.Models
{
    public interface IGetMoneyRequest
    {
        public Guid OrderId { get; }
    }
}
