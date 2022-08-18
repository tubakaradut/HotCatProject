namespace HC.Entity.Model
{
    public class AppUserRole : BaseEntity
    {
        public int AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
