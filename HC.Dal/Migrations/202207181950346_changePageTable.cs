namespace HC.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePageTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "IsExistSubMenu", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "IsExistSubMenu");
        }
    }
}
