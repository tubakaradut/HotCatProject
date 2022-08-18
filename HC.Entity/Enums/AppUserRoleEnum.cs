using System.ComponentModel.DataAnnotations;

namespace HC.Entity.Enums
{
    public enum AppUserRoleEnum
    {
        [Display(Name = "Admin")]
        Admin = 1,
        [Display(Name = "Müdür")]
        Manager = 2,  
        [Display(Name = "Garson")]
        Waiter = 3,   
        [Display(Name = "Kasiyer")]
        Cashier = 4,  
        [Display(Name = "Patron")]
        Boss = 5,     
        [Display(Name = "Pazarlamacı")]
        Salesman = 6, 
        [Display(Name = "Üye")]
        Guest = 7 
    }
}
