using HC.Entity.Model;
using HC.Map.Options;
using System.Configuration;
using System.Data.Entity;

namespace HC.Dal.Context
{
    public class HCContext:DbContext
    {
        public HCContext()
        {
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

            Configuration.LazyLoadingEnabled = false;
            //Eager loading kulanmak için lazy loading özelliğini pasif hale getirildi.
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new AppUserMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new MaterialMap());
            modelBuilder.Configurations.Add(new CafeTableMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new PageMap());
         
         

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<CafeTable> CafeTables { get; set; }
        public DbSet<Material> Materials { get; set; }
      
        public DbSet<Company> Companies { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AppUserRole> AppUsersRoles { get; set; }
        public DbSet<PageRole> PagesRoles { get; set; }
        public DbSet<ProductMaterial> ProductsMaterials { get; set; }

    }
}
