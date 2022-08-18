namespace HC.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubCategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Products", "SubCategoryId", "dbo.SubCategories");
            DropIndex("dbo.Products", new[] { "SubCategoryId" });
            DropIndex("dbo.SubCategories", new[] { "CategoryId" });
            AddColumn("dbo.Products", "CategoryId", c => c.Int());
            AddColumn("dbo.Categories", "IsExistSubCategory", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categories", "ParentId", c => c.Int());
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id");
            DropColumn("dbo.Products", "SubCategoryId");
            DropTable("dbo.SubCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubCategoryName = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 255),
                        CategoryId = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedById = c.Int(nullable: false),
                        DeletedDate = c.DateTime(),
                        DeletedById = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedById = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "SubCategoryId", c => c.Int());
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropColumn("dbo.Categories", "ParentId");
            DropColumn("dbo.Categories", "IsExistSubCategory");
            DropColumn("dbo.Products", "CategoryId");
            CreateIndex("dbo.SubCategories", "CategoryId");
            CreateIndex("dbo.Products", "SubCategoryId");
            AddForeignKey("dbo.Products", "SubCategoryId", "dbo.SubCategories", "Id");
            AddForeignKey("dbo.SubCategories", "CategoryId", "dbo.Categories", "Id");
        }
    }
}
