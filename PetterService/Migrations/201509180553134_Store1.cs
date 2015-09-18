namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Store1 : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.Stores", name: "IDX_BEAUTYSHOP_StoreName", newName: "IX_STORE_STORENAME");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Stores", name: "IX_STORE_STORENAME", newName: "IDX_BEAUTYSHOP_StoreName");
        }
    }
}
