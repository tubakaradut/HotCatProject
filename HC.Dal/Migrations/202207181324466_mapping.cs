namespace HC.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapping : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Employees");
            AddColumn("dbo.AppUsers", "EmployeeId", c => c.Int());
            AddColumn("dbo.Employees", "EmployeeId", c => c.Int());
            AlterColumn("dbo.Employees", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Employees", "Id");
            CreateIndex("dbo.Employees", "Id");
            AddForeignKey("dbo.Employees", "Id", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "Id", "dbo.AppUsers");
            DropIndex("dbo.Employees", new[] { "Id" });
            DropPrimaryKey("dbo.Employees");
            AlterColumn("dbo.Employees", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Employees", "EmployeeId");
            DropColumn("dbo.AppUsers", "EmployeeId");
            AddPrimaryKey("dbo.Employees", "Id");
        }
    }
}
