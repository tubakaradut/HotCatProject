using HC.Entity.Model;

namespace HC.Map.Options
{
    public  class EmployeeMap:BaseMap<Employee>
    {
        public EmployeeMap()
        {
            ToTable("dbo.Employees");
            Property(x => x.FirstName).HasMaxLength(255).IsRequired();
            Property(x => x.LastName).HasMaxLength(255).IsRequired();
            Property(x => x.Email).HasMaxLength(255).IsRequired();
            Property(x => x.Address).HasMaxLength(255).IsOptional();
            Property(x => x.PhoneNumber).HasMaxLength(255).IsOptional();

        }
    }
}
