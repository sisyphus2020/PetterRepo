namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreGallery2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreGalleries", "FileName", c => c.String(maxLength: 100));
            AddColumn("dbo.StoreGalleries", "FilePath", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreGalleries", "FilePath");
            DropColumn("dbo.StoreGalleries", "FileName");
        }
    }
}
