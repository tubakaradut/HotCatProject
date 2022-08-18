using HC.Entity.Model;

namespace HC.Map.Options
{
    public class DepartmentMap:BaseMap<Department>
    {
        public DepartmentMap()
        {
            ToTable("dbo.Departments");
            Property(x => x.DepartmentName).HasMaxLength(255).IsRequired();
          
        }
    }
}
