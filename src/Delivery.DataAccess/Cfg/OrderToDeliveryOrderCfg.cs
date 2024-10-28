using Delivery.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delivery.DataAccess.Cfg;

internal class OrderToDeliveryOrderCfg : IEntityTypeConfiguration<OrderToDeliveryOrder>
{
    public void Configure(EntityTypeBuilder<OrderToDeliveryOrder> builder)
    {
        builder.HasKey(x => new { x.OrderId, x.DeliveryOrderId });
    }
}