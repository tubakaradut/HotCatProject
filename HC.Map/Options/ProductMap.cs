using HC.Entity.Model;

namespace HC.Map.Options
{
    public  class ProductMap:BaseMap<Product>
    {
        public ProductMap()
        {
            ToTable("dbo.Products");
            Property(x => x.ProductName).HasMaxLength(255).IsRequired();
            Property(x => x.Description).HasMaxLength(255).IsOptional();
            Property(x => x.ProductImagePath).HasMaxLength(255).IsOptional();
            Property(x => x.UnitPrice).IsRequired();
        }
    }
}
