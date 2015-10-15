namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Board2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boards", "MemberID", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boards", "MemberID");
        }
    }
}
