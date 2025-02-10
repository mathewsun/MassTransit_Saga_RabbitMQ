using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

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
