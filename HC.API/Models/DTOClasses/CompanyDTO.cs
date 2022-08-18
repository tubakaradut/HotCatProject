using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.API.Models.DTOClasses
{
    public class CompanyDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Şirket Adı Zorunlu")]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string TaxNo { get; set; }
        public string PhoneNumber { get; set; }


        public virtual List<ReceiptDTO> Receipts { get; set; }

    }
}