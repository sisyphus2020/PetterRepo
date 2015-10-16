namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Store : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Stores", "IX_STORE_STOREID");
            DropColumn("dbo.Stores", "StoreID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stores", "StoreID", c => c.String(maxLength: 100));
            CreateIndex("dbo.Stores", "StoreID", name: "IX_STORE_STOREID");
        }
    }
}
