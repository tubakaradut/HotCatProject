using System;
using System.Collections.Generic;

namespace HC.Entity.Model
{
    public class AppUser:BaseEntity
    {
        public AppUser()
        {
            ActivationCode = Guid.NewGuid();
            AppUserRoles = new List<AppUserRole>();
            OrderDetails = new List<OrderDetail>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid? ActivationCode { get; set; }


        //Relational Properties
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual List<AppUserRole> AppUserRoles { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}
