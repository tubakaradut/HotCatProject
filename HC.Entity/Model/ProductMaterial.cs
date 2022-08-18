namespace HC.Entity.Model
{
    public class ProductMaterial:BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public decimal MaterialQuantity { get; set; }

    }
}
