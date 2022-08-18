using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.API.Models.DTOClasses
{
    public class DepartmentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Departman Adı Zorunlu")]
        public string DepartmentName { get; set; }

        public int? CompanyId { get; set; }
        public virtual CompanyDTO Company { get; set; }

        public virtual List<EmployeeDTO> Employee { get; set; }
    }
}