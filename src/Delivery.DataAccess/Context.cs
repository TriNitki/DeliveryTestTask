using Delivery.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Delivery.DataAccess;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<District> Districts { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<DeliveryOrder> DeliveryOrders { get; set; }

    public DbSet<OrderToDeliveryOrder> OrderToDeliveryOrders { get; set; }

    public DbSet<Audit.Core.AuditLog> AuditLog { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}