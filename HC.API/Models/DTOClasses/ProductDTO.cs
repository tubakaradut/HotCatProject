using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.API.Models.DTOClasses
{
    public class ProductDTO
    {
        public ProductDTO()
        {
            IsActive = true;
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün Adı Zorunlu")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Ürün Fiyatı Zorunlu")]
        public decimal UnitPrice { get; set; }
        public string ProductImagePath { get; set; }
        public string Description { get; set; }
        public decimal ProductQuantity { get; set; }
        public bool IsActive { get; set; }


        public int? CategoryId { get; set; }
        public virtual CategoryDTO Category { get; set; }
        public virtual List<OrderDetailDTO> OrderDetails { get; set; }
        public virtual List<ProductMaterialDTO> ProductMaterials { get; set; }

    }
}