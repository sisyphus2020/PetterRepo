namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Board1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boards", "Title", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boards", "Title");
        }
    }
}
