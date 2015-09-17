namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoardStats1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventBoardStats", "DateCreated", c => c.DateTime());
            AddColumn("dbo.EventBoardStats", "DateModified", c => c.DateTime());
            AddColumn("dbo.EventBoardStats", "DateDeleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventBoardStats", "DateDeleted");
            DropColumn("dbo.EventBoardStats", "DateModified");
            DropColumn("dbo.EventBoardStats", "DateCreated");
        }
    }
}
