using HC.Entity.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.API.Models.DTOClasses
{
    public class AppUserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı Zorunlu")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre Zorunlu")]
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public Guid? ActivationCode { get; set; }
        public List<string> RoleNames { get; set; }
        public List<Page> Pages { get; set; }


        public int? EmployeeId { get; set; }
        public virtual EmployeeDTO Employee { get; set; }
        public virtual List<OrderDetailDTO> OrderDetails { get; set; }
        public virtual List<AppUserRole> AppUserRoles { get; set; }

    }
}