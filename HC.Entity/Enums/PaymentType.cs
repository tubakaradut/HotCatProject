using System.ComponentModel.DataAnnotations;

namespace HC.Entity.Enums
{
    public enum PaymentType
    {
        [Display(Name = "Nakit")]
        Cash = 1,
        [Display(Name = "Kredi Kartı")]
        CreditCard = 2,
        [Display(Name = "Havale")]
        Transfer = 3
    }
}
