namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreGallery1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreGalleryFiles",
                c => new
                    {
                        StoreGalleryFileNo = c.Int(nullable: false, identity: true),
                        StoreGalleryNo = c.Int(nullable: false),
                        FileName = c.String(maxLength: 100),
                        FilePath = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.StoreGalleryFileNo)
                .ForeignKey("dbo.StoreGalleries", t => t.StoreGalleryNo)
                .Index(t => t.StoreGalleryNo);
            
            CreateTable(
                "dbo.StoreGalleryLikes",
                c => new
                    {
                        StoreGalleryLikeNo = c.Int(nullable: false, identity: true),
                        StoreGalleryNo = c.Int(nullable: false),
                        MemberNo = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.StoreGalleryLikeNo)
                .ForeignKey("dbo.Members", t => t.MemberNo)
                .ForeignKey("dbo.StoreGalleries", t => t.StoreGalleryNo)
                .Index(t => t.StoreGalleryNo)
                .Index(t => t.MemberNo);
            
            CreateTable(
                "dbo.StoreGalleryReplies",
                c => new
                    {
                        StoreGalleryReplyNo = c.Int(nullable: false, identity: true),
                        StoreGalleryNo = c.Int(nullable: false),
                        MemberNo = c.Int(nullable: false),
                        Reply = c.String(maxLength: 200),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.StoreGalleryReplyNo)
                .ForeignKey("dbo.Members", t => t.MemberNo)
                .ForeignKey("dbo.StoreGalleries", t => t.StoreGalleryNo)
                .Index(t => t.StoreGalleryNo)
                .Index(t => t.MemberNo);
            
            CreateTable(
                "dbo.StoreGalleryStats",
                c => new
                    {
                        StoreGalleryStatsNo = c.Int(nullable: false, identity: true),
                        StoreGalleryNo = c.Int(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.StoreGalleryStatsNo)
                .ForeignKey("dbo.StoreGalleries", t => t.StoreGalleryNo)
                .Index(t => t.StoreGalleryNo);
            
            AlterColumn("dbo.EventBoardReplies", "Reply", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreGalleryStats", "StoreGalleryNo", "dbo.StoreGalleries");
            DropForeignKey("dbo.StoreGalleryReplies", "StoreGalleryNo", "dbo.StoreGalleries");
            DropForeignKey("dbo.StoreGalleryReplies", "MemberNo", "dbo.Members");
            DropForeignKey("dbo.StoreGalleryLikes", "StoreGalleryNo", "dbo.StoreGalleries");
            DropForeignKey("dbo.StoreGalleryLikes", "MemberNo", "dbo.Members");
            DropForeignKey("dbo.StoreGalleryFiles", "StoreGalleryNo", "dbo.StoreGalleries");
            DropIndex("dbo.StoreGalleryStats", new[] { "StoreGalleryNo" });
            DropIndex("dbo.StoreGalleryReplies", new[] { "MemberNo" });
            DropIndex("dbo.StoreGalleryReplies", new[] { "StoreGalleryNo" });
            DropIndex("dbo.StoreGalleryLikes", new[] { "MemberNo" });
            DropIndex("dbo.StoreGalleryLikes", new[] { "StoreGalleryNo" });
            DropIndex("dbo.StoreGalleryFiles", new[] { "StoreGalleryNo" });
            AlterColumn("dbo.EventBoardReplies", "Reply", c => c.String());
            DropTable("dbo.StoreGalleryStats");
            DropTable("dbo.StoreGalleryReplies");
            DropTable("dbo.StoreGalleryLikes");
            DropTable("dbo.StoreGalleryFiles");
        }
    }
}
