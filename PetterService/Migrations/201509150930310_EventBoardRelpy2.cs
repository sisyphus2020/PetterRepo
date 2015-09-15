namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoardRelpy2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventBoardReplies", "DateCreated", c => c.DateTime());
            AddColumn("dbo.EventBoardReplies", "DateModified", c => c.DateTime());
            AddColumn("dbo.EventBoardReplies", "DateDeleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventBoardReplies", "DateDeleted");
            DropColumn("dbo.EventBoardReplies", "DateModified");
            DropColumn("dbo.EventBoardReplies", "DateCreated");
        }
    }
}
