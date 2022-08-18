using HC.Entity.Enums;
using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class CafeTable : BaseEntity
    {
        public string TableName { get; set; }
        public TableLocation TableLocation { get; set; }
        public short Capacity { get; set; }
        public TableStatus TableStatus { get; set; }


        //Relational Properties
        public virtual List<Order> Orders { get; set; }
    }
}
