using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class BuyItemsSagaStateMap : SagaClassMap<BuyItemsSagaState>
{
    protected override void Configure(EntityTypeBuilder<BuyItemsSagaState> entity, ModelBuilder model)
    {
        base.Configure(entity, model);
        entity.Property(x => x.CurrentState).HasMaxLength(255);
    }
}
