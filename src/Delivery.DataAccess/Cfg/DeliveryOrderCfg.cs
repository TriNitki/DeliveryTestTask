using Microsoft.EntityFrameworkCore;
using Delivery.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delivery.DataAccess.Cfg;

internal class DeliveryOrderCfg : IEntityTypeConfiguration<DeliveryOrder>
{
    public void Configure(EntityTypeBuilder<DeliveryOrder> builder)
    {
        builder.HasMany(x => x.Orders)
            .WithOne(x => x.DeliveryOrder)
            .HasForeignKey(x => x.DeliveryOrderId);

        builder.HasOne(x => x.District)
            .WithMany(x => x.DeliveryOrders)
            .HasForeignKey(x => x.DistrictId);
    }
}