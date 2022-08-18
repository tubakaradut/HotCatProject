using HC.Entity.Enums;
using System;
using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class Material:BaseEntity
    {
        public string MaterialName { get; set; }
        public UnitType UnitType { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public decimal MinStock { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MaxStock { get; set; }


        //Relational Properties
        public virtual List<ProductMaterial> ProductMaterials { get; set; }
    }
}
