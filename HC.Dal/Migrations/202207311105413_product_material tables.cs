namespace HC.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_materialtables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Materials", "ProductId", "dbo.Products");
            DropIndex("dbo.Materials", new[] { "ProductId" });
            CreateTable(
                "dbo.ProductMaterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Materials", t => t.MaterialId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.MaterialId);
            
            DropColumn("dbo.Materials", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Materials", "ProductId", c => c.Int());
            DropForeignKey("dbo.ProductMaterials", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductMaterials", "MaterialId", "dbo.Materials");
            DropIndex("dbo.ProductMaterials", new[] { "MaterialId" });
            DropIndex("dbo.ProductMaterials", new[] { "ProductId" });
            DropTable("dbo.ProductMaterials");
            CreateIndex("dbo.Materials", "ProductId");
            AddForeignKey("dbo.Materials", "ProductId", "dbo.Products", "Id");
        }
    }
}
