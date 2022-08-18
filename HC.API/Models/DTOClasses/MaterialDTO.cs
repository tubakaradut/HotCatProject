using HC.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.API.Models.DTOClasses
{
    public class MaterialDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Malzeme Adı Zorunlu")]
        public string MaterialName { get; set; }
        public UnitType UnitType { get; set; }
        public string UnitTypeName { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public decimal MinStock { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MaxStock { get; set; }


        public virtual List<ProductMaterialDTO> ProductMaterials { get; set; }

    }
}