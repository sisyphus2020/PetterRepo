namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoardRelpy6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventBoardReplies", "MemberNo", "dbo.Members");
            DropIndex("dbo.EventBoardReplies", new[] { "MemberNo" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.EventBoardReplies", "MemberNo");
            AddForeignKey("dbo.EventBoardReplies", "MemberNo", "dbo.Members", "MemberNo", cascadeDelete: true);
        }
    }
}
