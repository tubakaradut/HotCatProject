using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string TaxNo { get; set; }
        public string PhoneNumber { get; set; }


        //Relational Properties
        public virtual List<Receipt> Receipts { get; set; }
        public virtual List<Department> Departments { get; set; }

    }
}
