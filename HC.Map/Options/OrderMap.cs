using HC.Entity.Model;

namespace HC.Map.Options
{
    public class OrderMap:BaseMap<Order>
    {
        public OrderMap()
        {
            ToTable("dbo.Orders");
            Property(x => x.OrderName).HasMaxLength(255).IsRequired();
            Property(x => x.OrderNumber).HasMaxLength(255).IsRequired();
            Property(x => x.OrderStatus).IsRequired();
        }
    }
}
