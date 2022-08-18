using HC.Entity.Model;

namespace HC.Map.Options
{
    public class PageMap : BaseMap<Page>
    {
        public PageMap()
        {
            ToTable("dbo.Pages");
            Property(x => x.Name).HasMaxLength(255).IsRequired();
            Property(x => x.Path).HasMaxLength(255).IsRequired();
            Property(x => x.ParentId).IsOptional();
            Property(x => x.IsExistSubMenu).IsRequired();
        }
    }
}
