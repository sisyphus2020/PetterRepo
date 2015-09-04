namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Member2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Members", "Longitude", c => c.Double(nullable: false));
            DropColumn("dbo.Companies", "Holiday");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "Holiday", c => c.String());
            DropColumn("dbo.Members", "Longitude");
            DropColumn("dbo.Members", "Latitude");
        }
    }
}
