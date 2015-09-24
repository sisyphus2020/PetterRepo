namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreQuestion1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreQuestions", "DateCreated", c => c.DateTime());
            AddColumn("dbo.StoreQuestions", "DateModified", c => c.DateTime());
            AddColumn("dbo.StoreQuestions", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.StoreQuestions", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreQuestions", "StateFlag");
            DropColumn("dbo.StoreQuestions", "DateDeleted");
            DropColumn("dbo.StoreQuestions", "DateModified");
            DropColumn("dbo.StoreQuestions", "DateCreated");
        }
    }
}
