namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pension1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BeautyShops", "BeautyShopName", c => c.String(maxLength: 100));
            AlterColumn("dbo.BeautyShops", "BeautyShopAddr", c => c.String(maxLength: 200));
            AlterColumn("dbo.BeautyShops", "PictureName", c => c.String(maxLength: 100));
            AlterColumn("dbo.BeautyShops", "PicturePath", c => c.String(maxLength: 100));
            AlterColumn("dbo.BeautyShops", "StartHours", c => c.String(maxLength: 4));
            AlterColumn("dbo.BeautyShops", "EndHours", c => c.String(maxLength: 4));
            AlterColumn("dbo.BeautyShops", "Introduction", c => c.String(maxLength: 200));
            AlterColumn("dbo.Members", "Password", c => c.String(maxLength: 50));
            AlterColumn("dbo.Members", "PictureName", c => c.String(maxLength: 200));
            AlterColumn("dbo.Members", "PicturePath", c => c.String(maxLength: 200));
            AlterColumn("dbo.Pensions", "PensionName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Pensions", "PensionAddr", c => c.String(maxLength: 200));
            AlterColumn("dbo.Pensions", "PictureName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Pensions", "PicturePath", c => c.String(maxLength: 100));
            AlterColumn("dbo.Pensions", "StartHours", c => c.String(maxLength: 4));
            AlterColumn("dbo.Pensions", "EndHours", c => c.String(maxLength: 4));
            AlterColumn("dbo.Pensions", "Introduction", c => c.String(maxLength: 200));
            AlterColumn("dbo.PetSitters", "PetSitterName", c => c.String(maxLength: 100));
            AlterColumn("dbo.PetSitters", "PetSitterAddr", c => c.String(maxLength: 200));
            AlterColumn("dbo.PetSitters", "PictureName", c => c.String(maxLength: 100));
            AlterColumn("dbo.PetSitters", "PicturePath", c => c.String(maxLength: 100));
            AlterColumn("dbo.PetSitters", "StartHours", c => c.String(maxLength: 4));
            AlterColumn("dbo.PetSitters", "EndHours", c => c.String(maxLength: 4));
            AlterColumn("dbo.PetSitters", "Introduction", c => c.String(maxLength: 200));
            CreateIndex("dbo.BeautyShops", "BeautyShopName", name: "IDX_BEAUTYSHOP_BEAUTYSHOPNAME");
            CreateIndex("dbo.BeautyShops", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.BeautyShops", "ReviewCount", name: "IDX_REVIEWCOUNT");
            CreateIndex("dbo.BeautyShops", "Bookmark", name: "IDX_BOOKMARK");
            CreateIndex("dbo.Pensions", "PensionName", name: "IDX_PENSION_PENSIONNAME");
            CreateIndex("dbo.Pensions", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.Pensions", "ReviewCount", name: "IDX_REVIEWCOUNT");
            CreateIndex("dbo.Pensions", "Bookmark", name: "IDX_BOOKMARK");
            CreateIndex("dbo.PetSitters", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.PetSitters", "ReviewCount", name: "IDX_REVIEWCOUNT");
            CreateIndex("dbo.PetSitters", "Bookmark", name: "IDX_BOOKMARK");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PetSitters", "IDX_BOOKMARK");
            DropIndex("dbo.PetSitters", "IDX_REVIEWCOUNT");
            DropIndex("dbo.PetSitters", "IDX_CRADE");
            DropIndex("dbo.Pensions", "IDX_BOOKMARK");
            DropIndex("dbo.Pensions", "IDX_REVIEWCOUNT");
            DropIndex("dbo.Pensions", "IDX_CRADE");
            DropIndex("dbo.Pensions", "IDX_PENSION_PENSIONNAME");
            DropIndex("dbo.BeautyShops", "IDX_BOOKMARK");
            DropIndex("dbo.BeautyShops", "IDX_REVIEWCOUNT");
            DropIndex("dbo.BeautyShops", "IDX_CRADE");
            DropIndex("dbo.BeautyShops", "IDX_BEAUTYSHOP_BEAUTYSHOPNAME");
            AlterColumn("dbo.PetSitters", "Introduction", c => c.String());
            AlterColumn("dbo.PetSitters", "EndHours", c => c.String());
            AlterColumn("dbo.PetSitters", "StartHours", c => c.String());
            AlterColumn("dbo.PetSitters", "PicturePath", c => c.String());
            AlterColumn("dbo.PetSitters", "PictureName", c => c.String());
            AlterColumn("dbo.PetSitters", "PetSitterAddr", c => c.String());
            AlterColumn("dbo.PetSitters", "PetSitterName", c => c.String());
            AlterColumn("dbo.Pensions", "Introduction", c => c.String());
            AlterColumn("dbo.Pensions", "EndHours", c => c.String());
            AlterColumn("dbo.Pensions", "StartHours", c => c.String());
            AlterColumn("dbo.Pensions", "PicturePath", c => c.String());
            AlterColumn("dbo.Pensions", "PictureName", c => c.String());
            AlterColumn("dbo.Pensions", "PensionAddr", c => c.String());
            AlterColumn("dbo.Pensions", "PensionName", c => c.String());
            AlterColumn("dbo.Members", "PicturePath", c => c.String());
            AlterColumn("dbo.Members", "PictureName", c => c.String());
            AlterColumn("dbo.Members", "Password", c => c.String());
            AlterColumn("dbo.BeautyShops", "Introduction", c => c.String());
            AlterColumn("dbo.BeautyShops", "EndHours", c => c.String());
            AlterColumn("dbo.BeautyShops", "StartHours", c => c.String());
            AlterColumn("dbo.BeautyShops", "PicturePath", c => c.String());
            AlterColumn("dbo.BeautyShops", "PictureName", c => c.String());
            AlterColumn("dbo.BeautyShops", "BeautyShopAddr", c => c.String());
            AlterColumn("dbo.BeautyShops", "BeautyShopName", c => c.String());
        }
    }
}
