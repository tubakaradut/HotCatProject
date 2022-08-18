using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class Product:BaseEntity
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductImagePath { get; set; }
        public string Description { get; set; }


        //Relational Properties
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public virtual List<ProductMaterial> ProductMaterials { get; set; }
    }
}
