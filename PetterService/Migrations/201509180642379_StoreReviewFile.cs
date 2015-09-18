namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreReviewFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreReviewFiles", "DateCreated", c => c.DateTime());
            AddColumn("dbo.StoreReviewFiles", "DateModified", c => c.DateTime());
            AddColumn("dbo.StoreReviewFiles", "DateDeleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreReviewFiles", "DateDeleted");
            DropColumn("dbo.StoreReviewFiles", "DateModified");
            DropColumn("dbo.StoreReviewFiles", "DateCreated");
        }
    }
}
