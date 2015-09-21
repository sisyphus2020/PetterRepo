namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Store : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "CommonCodeNo", c => c.Int(nullable: false));
            AddColumn("dbo.Stores", "StoreID", c => c.String(maxLength: 100));
            CreateIndex("dbo.Stores", "StoreID", name: "IX_STORE_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Stores", "IX_STORE_ID");
            DropColumn("dbo.Stores", "StoreID");
            DropColumn("dbo.Stores", "CommonCodeNo");
        }
    }
}
