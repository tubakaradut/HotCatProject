using HC.Entity.Enums;
using System.Collections.Generic;

namespace HC.API.Models.DTOClasses
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string OrderName { get; set; }
        public string OrderNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }


        public virtual List<OrderDetailDTO> OrderDetails { get; set; }
        public int? CafeTableId { get; set; }
        public virtual CafeTableDTO CafeTable { get; set; }

        public int? ReceiptId { get; set; }
        public virtual ReceiptDTO Receipt { get; set; }
        


    }
}