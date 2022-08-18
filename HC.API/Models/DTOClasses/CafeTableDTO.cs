using HC.Entity.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.API.Models.DTOClasses
{
    public class CafeTableDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Masa Adı Zorunlu")]
        public string TableName { get; set; }
        public TableLocation TableLocation { get; set; }
        public string TableLocationName { get; set; }
        public string TableLocationDisplayName { get; set; }

        [Required(ErrorMessage = "Kapasitesi Zorunlu")]
        public short Capacity { get; set; }
        public TableStatus TableStatus { get; set; }
        public string TableStatusDisplayName { get; set; }


        public virtual List<OrderDTO> Orders { get; set; }
    }
}