namespace HC.Entity.Model
{
    public class PageRole : BaseEntity
    {
        public int PageId { get; set; }
        public virtual Page Page { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
