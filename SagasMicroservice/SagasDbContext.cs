using MassTransit.EntityFrameworkIntegration;
using Microsoft.EntityFrameworkCore;

namespace SagasMicroservice
{
    public sealed class SagasDbContext : SagaDbContext
    {
        public SagasDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations => new ISagaClassMap[]
        {
        new BuyItemsSagaStateMap()
        };
    }

}
