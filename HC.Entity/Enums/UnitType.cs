using System.ComponentModel.DataAnnotations;

namespace HC.Entity.Enums
{
    public enum UnitType
    {
        [Display(Name = "Litre")]
        Liter = 1,
        [Display(Name = "Kilogram")]
        Kilogram = 2,
        [Display(Name = "Adet")]
        Piece = 3
    }
}
