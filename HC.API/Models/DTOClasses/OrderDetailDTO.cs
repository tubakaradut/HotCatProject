namespace HC.API.Models.DTOClasses
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Quantity { get; set; }


        public int? ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }
        public int? OrderId { get; set; }
        public virtual OrderDTO Order { get; set; }
        public int? AppUserId { get; set; }
        public virtual AppUserDTO AppUser { get; set; }
    }
}