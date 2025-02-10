using MassTransit;
using MassTransit.EntityFrameworkIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SagasMicroservice.Saga
{
    public sealed class BuyItemsSagaStateMap : SagaClassMap<BuyItemsSagaState>
    {
        protected override void Configure(EntityTypeBuilder<BuyItemsSagaState> entity, ModelBuilder model)
        {
            base.Configure(entity, model);
            entity.Property(x => x.CurrentState).HasMaxLength(255);
        }
    }
}
