namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeautyShopBookmark : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BeautyShopBookmarks",
                c => new
                    {
                        BeautyShopBookmarkNo = c.Int(nullable: false, identity: true),
                        BeautyShopNo = c.Int(nullable: false),
                        MemberNo = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.BeautyShopBookmarkNo)
                .ForeignKey("dbo.BeautyShops", t => t.BeautyShopNo, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.MemberNo, cascadeDelete: true)
                .Index(t => t.BeautyShopNo)
                .Index(t => t.MemberNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BeautyShopBookmarks", "MemberNo", "dbo.Members");
            DropForeignKey("dbo.BeautyShopBookmarks", "BeautyShopNo", "dbo.BeautyShops");
            DropIndex("dbo.BeautyShopBookmarks", new[] { "MemberNo" });
            DropIndex("dbo.BeautyShopBookmarks", new[] { "BeautyShopNo" });
            DropTable("dbo.BeautyShopBookmarks");
        }
    }
}
