using HC.Entity.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Entity.Model
{
    public class Receipt : BaseEntity
    {
        public string ReceiptNumber { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentType PaymentType { get; set; }


        //Relational Properties
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }


    }
}
