using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class Page:BaseEntity
    {
        public Page()
        {
            PageRoles = new List<PageRole>();
        }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? ParentId { get; set; }
        public bool IsExistSubMenu { get; set; }
        public bool IsMainPage { get; set; }


        //Relational Properties
        public virtual List<PageRole> PageRoles { get; set; }
    }
}
