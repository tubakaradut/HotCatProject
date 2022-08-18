namespace HC.Entity.Model
{
    public class Employee:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }



        //Relational Properties
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
