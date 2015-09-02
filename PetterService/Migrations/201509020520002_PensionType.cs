namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PensionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeautyShops", "StartBeautyShopHours", c => c.String());
            AddColumn("dbo.BeautyShops", "EndBeautyShopHours", c => c.String());
            DropColumn("dbo.BeautyShops", "StartBeautyShop");
            DropColumn("dbo.BeautyShops", "EndBeautyShop");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BeautyShops", "EndBeautyShop", c => c.String());
            AddColumn("dbo.BeautyShops", "StartBeautyShop", c => c.String());
            DropColumn("dbo.BeautyShops", "EndBeautyShopHours");
            DropColumn("dbo.BeautyShops", "StartBeautyShopHours");
        }
    }
}
