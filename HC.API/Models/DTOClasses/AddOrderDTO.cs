using System.Collections.Generic;

namespace HC.API.Models.DTOClasses
{
    public class AddOrderDTO
    {
        public int? CafeTableId { get; set; }
        public virtual List<AddOrderDetailDTO> OrderDetails { get; set; }
    }
}