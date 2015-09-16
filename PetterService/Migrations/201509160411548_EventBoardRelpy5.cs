namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoardRelpy5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventBoardReplies", "EventBoardNo", c => c.Int(nullable: false));
            CreateIndex("dbo.EventBoardReplies", "EventBoardNo");
            AddForeignKey("dbo.EventBoardReplies", "EventBoardNo", "dbo.EventBoards", "EventBoardNo", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventBoardReplies", "EventBoardNo", "dbo.EventBoards");
            DropIndex("dbo.EventBoardReplies", new[] { "EventBoardNo" });
            DropColumn("dbo.EventBoardReplies", "EventBoardNo");
        }
    }
}
