using HC.Entity.Model;

namespace HC.Map.Options
{
    public class ReceiptMap:BaseMap<Receipt>
    {
        public ReceiptMap()
        {

            ToTable("dbo.Receipts");
            Property(x => x.ReceiptNumber).IsRequired();
            Property(x => x.Discount).IsOptional();
            Property(x => x.TotalPrice).IsRequired();
            Property(x => x.PaymentType).IsRequired();
        }
    }
}
