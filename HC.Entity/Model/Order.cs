using HC.Entity.Enums;
using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class Order : BaseEntity
    {
        public string OrderName { get; set; }
        public string OrderNumber { get; set; }

        public OrderStatus OrderStatus { get; set; }


        //Relational Properties
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public int? CafeTableId { get; set; }
        public virtual CafeTable CafeTable { get; set; }
    }
}
