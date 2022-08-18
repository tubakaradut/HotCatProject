using HC.Entity.Model;

namespace HC.Map.Options
{
    public class CategoryMap:BaseMap<Category>
    {
        public CategoryMap()
        {
            ToTable("dbo.Categories");
            Property(x => x.CategoryName).HasMaxLength(255).IsRequired();
            Property(x => x.Description).HasMaxLength(255).IsOptional();

        }
    }
}
