namespace HC.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class classrevizyon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "AppUserId", c => c.Int());
            DropColumn("dbo.Employees", "EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "EmployeeId", c => c.Int());
            DropColumn("dbo.Employees", "AppUserId");
        }
    }
}
