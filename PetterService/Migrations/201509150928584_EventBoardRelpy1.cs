namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoardRelpy1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventBoardReplies",
                c => new
                    {
                        EventBoardReplyNo = c.Int(nullable: false, identity: true),
                        MemberNo = c.Int(nullable: false),
                        Reply = c.String(),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.EventBoardReplyNo)
                .ForeignKey("dbo.Members", t => t.MemberNo, cascadeDelete: true)
                .Index(t => t.MemberNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventBoardReplies", "MemberNo", "dbo.Members");
            DropIndex("dbo.EventBoardReplies", new[] { "MemberNo" });
            DropTable("dbo.EventBoardReplies");
        }
    }
}
