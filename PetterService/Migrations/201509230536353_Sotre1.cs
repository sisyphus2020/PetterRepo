namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sotre1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "CodeID", c => c.String(maxLength: 6, fixedLength: true, unicode: false));
            CreateIndex("dbo.Stores", "CodeID", name: "IX_STORE_CODEID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Stores", "IX_STORE_CODEID");
            DropColumn("dbo.Stores", "CodeID");
        }
    }
}
