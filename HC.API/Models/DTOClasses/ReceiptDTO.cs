using HC.Entity.Enums;

namespace HC.API.Models.DTOClasses
{
    public class ReceiptDTO
    {
        public int Id { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentType PaymentType { get; set; }


        public int? OrderId { get; set; }
        public int? CompanyId { get; set; }
        public virtual OrderDTO Order { get; set; }
        public virtual CompanyDTO Company { get; set; }
    }
}