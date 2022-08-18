namespace HC.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changerole
        : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Roles", "Name", c => c.String());
            DropColumn("dbo.Roles", "CreatedDate");
            DropColumn("dbo.Roles", "CreatedById");
            DropColumn("dbo.Roles", "DeletedDate");
            DropColumn("dbo.Roles", "DeletedById");
            DropColumn("dbo.Roles", "ModifiedDate");
            DropColumn("dbo.Roles", "ModifiedById");
            DropColumn("dbo.Roles", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ModifiedById", c => c.Int());
            AddColumn("dbo.Roles", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.Roles", "DeletedById", c => c.Int());
            AddColumn("dbo.Roles", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.Roles", "CreatedById", c => c.Int(nullable: false));
            AddColumn("dbo.Roles", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Roles", "Name", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
