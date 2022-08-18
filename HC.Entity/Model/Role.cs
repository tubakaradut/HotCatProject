using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class Role
    {
        public Role()
        {
            AppUsersRole = new List<AppUserRole>();
            PagesRole = new List<PageRole>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        //Relational Properties
        public virtual List<AppUserRole> AppUsersRole { get; set; }
        public virtual List<PageRole> PagesRole { get; set; }
    }
}
