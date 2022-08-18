using HC.Entity.Model;

namespace HC.Map.Options
{
    public class AppUserMap:BaseMap<AppUser>
    {
        public AppUserMap()
        {
            ToTable("dbo.AppUsers");
            Property(x => x.UserName).HasMaxLength(255).IsRequired();
            Property(x => x.Password).HasMaxLength(255).IsOptional();
            Property(x => x.ActivationCode).IsOptional();
        }
    }
}
