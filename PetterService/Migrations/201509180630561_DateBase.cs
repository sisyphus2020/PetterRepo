namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateBase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreBookmarks", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AddColumn("dbo.StoreStats", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AddColumn("dbo.EventBoardStats", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AddColumn("dbo.NoticeStats", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AddColumn("dbo.StoreReviewStats", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreReviewStats", "StateFlag");
            DropColumn("dbo.NoticeStats", "StateFlag");
            DropColumn("dbo.EventBoardStats", "StateFlag");
            DropColumn("dbo.StoreStats", "StateFlag");
            DropColumn("dbo.StoreBookmarks", "StateFlag");
        }
    }
}
