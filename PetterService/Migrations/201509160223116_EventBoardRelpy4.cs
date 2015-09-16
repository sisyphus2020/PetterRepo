namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoardRelpy4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventBoards", "ReviewCount", c => c.Int(nullable: false));
            CreateIndex("dbo.EventBoards", "ReviewCount", name: "IDX_REVIEWCOUNT");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EventBoards", "IDX_REVIEWCOUNT");
            DropColumn("dbo.EventBoards", "ReviewCount");
        }
    }
}
