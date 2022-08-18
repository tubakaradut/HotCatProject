using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.API.Models.DTOClasses
{
    public class EmployeeDTO
    {
        public EmployeeDTO()
        {
            AppUserRoles = new List<int>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim Zorunlu")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Soyisim Zorunlu")]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email Zorunlu")]
        public string Email { get; set; }
        public bool IsCreateAppUser { get; set; }

        public List<int> AppUserRoles { get; set; }


        public int? DepartmentId { get; set; }
        public virtual DepartmentDTO Department { get; set; }
    }
}