using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsExistSubCategory { get; set; }

        public int? ParentId { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
