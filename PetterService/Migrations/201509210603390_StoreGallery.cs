namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreGallery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreGalleries",
                c => new
                    {
                        StoreGalleryNo = c.Int(nullable: false, identity: true),
                        StoreNo = c.Int(nullable: false),
                        Content = c.String(maxLength: 200),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.StoreGalleryNo)
                .ForeignKey("dbo.Stores", t => t.StoreNo)
                .Index(t => t.StoreNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreGalleries", "StoreNo", "dbo.Stores");
            DropIndex("dbo.StoreGalleries", new[] { "StoreNo" });
            DropTable("dbo.StoreGalleries");
        }
    }
}
