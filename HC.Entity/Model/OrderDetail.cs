namespace HC.Entity.Model
{
    public class OrderDetail : BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public decimal Quantity { get; set; }



        //Relational Properties
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int? AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}