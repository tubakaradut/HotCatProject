using System.ComponentModel.DataAnnotations;

namespace HC.Entity.Enums
{
    public enum TableStatus
    {
        [Display(Name = "Boş")]
        Empty = 1,
        [Display(Name = "Arızalı")]
        NotAvailable = 2,
        [Display(Name = "Dolu")]
        Filled = 3,
    }
}
