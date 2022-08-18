using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class Department:BaseEntity
    {
        public string DepartmentName { get; set; }


        //Relational Properties
        public virtual List<Employee> Employees { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
