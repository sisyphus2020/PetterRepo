namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeautyShop1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeautyShops", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.BeautyShops", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Pensions", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Pensions", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pensions", "Longitude");
            DropColumn("dbo.Pensions", "Latitude");
            DropColumn("dbo.BeautyShops", "Longitude");
            DropColumn("dbo.BeautyShops", "Latitude");
        }
    }
}
