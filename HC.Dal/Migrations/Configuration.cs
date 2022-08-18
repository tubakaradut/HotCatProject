namespace HC.Dal.Migrations
{
    using HC.Entity.Model;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HC.Dal.Context.HCContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HC.Dal.Context.HCContext context)
        {

            List<string> roles = new List<string>() {
                "Admin","Manager","Waiter","Cashier","Boss","Salesman","Guest"
            };

            List<Role> roleList = context.Roles.Where(x => roles.Contains(x.Name)).ToList();

            if (roleList.Count <= 0)
            {
                foreach (var roleName in roles)
                {
                    Role role = new Role();
                    role.Name = roleName;
                    context.Roles.Add(role);
                    context.SaveChanges();
                }
            }

            
            base.Seed(context);
        }
    }
}
