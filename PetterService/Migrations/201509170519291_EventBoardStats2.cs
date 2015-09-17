namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoardStats2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventBoardStats", "ReplyCount", c => c.Int(nullable: false));
            DropColumn("dbo.EventBoardStats", "ReviewCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventBoardStats", "ReviewCount", c => c.Int(nullable: false));
            DropColumn("dbo.EventBoardStats", "ReplyCount");
        }
    }
}
