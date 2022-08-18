using HC.Entity.Model;

namespace HC.Map.Options
{
    public class CafeTableMap : BaseMap<CafeTable>
    {
        public CafeTableMap()
        {
            ToTable("dbo.CafeTables");
            Property(x => x.TableName).HasMaxLength(255).IsRequired();
            Property(x => x.TableLocation).IsRequired();
            Property(x => x.Capacity).IsRequired();
            Property(x => x.TableStatus).IsRequired();
        }
    }
}
