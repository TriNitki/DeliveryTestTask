using Delivery.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Delivery.DataAccess.Cfg;

internal class OrderCfg : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(x => x.District)
            .WithMany();

        builder.HasMany(x => x.DeliveryOrders)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId);
    }
}