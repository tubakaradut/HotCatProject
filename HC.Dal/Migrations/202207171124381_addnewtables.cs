namespace HC.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppUserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.AppUserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        CreatedDate = c.DateTime(),
                        CreatedById = c.Int(nullable: false),
                        DeletedDate = c.DateTime(),
                        DeletedById = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedById = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PageRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pages", t => t.PageId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.PageId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Path = c.String(nullable: false, maxLength: 255),
                        ParentId = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedById = c.Int(nullable: false),
                        DeletedDate = c.DateTime(),
                        DeletedById = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedById = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AppUsers", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "Role", c => c.Int(nullable: false));
            DropForeignKey("dbo.PageRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.PageRoles", "PageId", "dbo.Pages");
            DropForeignKey("dbo.AppUserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.AppUserRoles", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.PageRoles", new[] { "RoleId" });
            DropIndex("dbo.PageRoles", new[] { "PageId" });
            DropIndex("dbo.AppUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AppUserRoles", new[] { "AppUserId" });
            DropTable("dbo.Pages");
            DropTable("dbo.PageRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.AppUserRoles");
        }
    }
}
