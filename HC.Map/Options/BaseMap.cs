using HC.Entity.Model;
using System.Data.Entity.ModelConfiguration;

namespace HC.Map.Options
{
    public abstract class BaseMap<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseMap()
        {
            Property(x => x.CreatedDate).IsOptional();
            Property(x => x.ModifiedDate).IsOptional();
            Property(x => x.DeletedDate).IsOptional();
        }
    }
}
