namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreNews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreNews", "CodeID", c => c.String(maxLength: 6, fixedLength: true, unicode: false));
            CreateIndex("dbo.StoreNews", "CodeID", name: "IX_STORE_CODEID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StoreNews", "IX_STORE_CODEID");
            DropColumn("dbo.StoreNews", "CodeID");
        }
    }
}
