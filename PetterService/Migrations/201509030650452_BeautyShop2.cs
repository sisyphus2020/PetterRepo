namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeautyShop2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeautyShops", "StartHours", c => c.String());
            AddColumn("dbo.BeautyShops", "EndHours", c => c.String());
            AddColumn("dbo.Pensions", "StartHours", c => c.String());
            AddColumn("dbo.Pensions", "EndHours", c => c.String());
            DropColumn("dbo.BeautyShops", "StartBeautyShopHours");
            DropColumn("dbo.BeautyShops", "EndBeautyShopHours");
            DropColumn("dbo.Pensions", "StartPensionHours");
            DropColumn("dbo.Pensions", "EndPensionHours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pensions", "EndPensionHours", c => c.String());
            AddColumn("dbo.Pensions", "StartPensionHours", c => c.String());
            AddColumn("dbo.BeautyShops", "EndBeautyShopHours", c => c.String());
            AddColumn("dbo.BeautyShops", "StartBeautyShopHours", c => c.String());
            DropColumn("dbo.Pensions", "EndHours");
            DropColumn("dbo.Pensions", "StartHours");
            DropColumn("dbo.BeautyShops", "EndHours");
            DropColumn("dbo.BeautyShops", "StartHours");
        }
    }
}
