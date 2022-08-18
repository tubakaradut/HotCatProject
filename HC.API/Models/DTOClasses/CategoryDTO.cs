using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.API.Models.DTOClasses
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori Adı Zorunlu")]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public bool IsExistSubCategory { get; set; }

        public int? ParentId { get; set; }
        public string ParentCategoryName { get; set; }


        public virtual List<ProductDTO> Products { get; set; }

    }
}