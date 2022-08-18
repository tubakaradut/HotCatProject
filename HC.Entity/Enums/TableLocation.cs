using System.ComponentModel.DataAnnotations;

namespace HC.Entity.Enums
{
    public enum TableLocation
    {
        [Display(Name = "Teras")]
        Terrace = 1,
        [Display(Name = "Bahçe")]
        Garden = 2,
        [Display(Name = "İçeri")]
        Inside = 3
    }
}
