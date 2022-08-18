using HC.Entity.Enums;

namespace HC.API.Models.DTOClasses
{
    public class CompleteOrderDTO
    {
        public int CafeTableId { get; set; }
        public decimal Discount { get; set; }
        public int PaymentTypeId { get; set; }
    }
}