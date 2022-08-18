using HC.Entity.Model;

namespace HC.Map.Options
{
    public class OrderDetailMap : BaseMap<OrderDetail>
    {
        public OrderDetailMap()
        {
            ToTable("dbo.OrderDetails");
            Property(x => x.TotalPrice).IsRequired();
            Property(x => x.Quantity).IsRequired();
        }

    }
}
