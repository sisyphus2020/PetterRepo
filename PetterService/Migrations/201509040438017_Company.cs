namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class Company : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "StartHours", c => c.String(maxLength: 4));
            AddColumn("dbo.Companies", "EndHours", c => c.String(maxLength: 4));
            AddColumn("dbo.Companies", "Coordinate", c => c.Geography());
            AddColumn("dbo.Companies", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Companies", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Companies", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Companies", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Companies", "CompanyName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Companies", "CompanyAddr", c => c.String(maxLength: 200));
            AlterColumn("dbo.Companies", "PictureName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Companies", "PicturePath", c => c.String(maxLength: 100));
            AlterColumn("dbo.Companies", "Introduction", c => c.String(maxLength: 200));
            CreateIndex("dbo.Companies", "CompanyName", name: "IDX_COMPANY_COMPANYNAME");
            CreateIndex("dbo.Companies", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.Companies", "ReviewCount", name: "IDX_REVIEWCOUNT");
            CreateIndex("dbo.Companies", "Bookmark", name: "IDX_BOOKMARK");
            DropColumn("dbo.Companies", "StartShopHours");
            DropColumn("dbo.Companies", "EndShopHours");
            DropColumn("dbo.Companies", "Geography");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "Geography", c => c.Geography());
            AddColumn("dbo.Companies", "EndShopHours", c => c.String());
            AddColumn("dbo.Companies", "StartShopHours", c => c.String());
            DropIndex("dbo.Companies", "IDX_BOOKMARK");
            DropIndex("dbo.Companies", "IDX_REVIEWCOUNT");
            DropIndex("dbo.Companies", "IDX_CRADE");
            DropIndex("dbo.Companies", "IDX_COMPANY_COMPANYNAME");
            AlterColumn("dbo.Companies", "Introduction", c => c.String());
            AlterColumn("dbo.Companies", "PicturePath", c => c.String());
            AlterColumn("dbo.Companies", "PictureName", c => c.String());
            AlterColumn("dbo.Companies", "CompanyAddr", c => c.String());
            AlterColumn("dbo.Companies", "CompanyName", c => c.String());
            DropColumn("dbo.Companies", "DateModified");
            DropColumn("dbo.Companies", "DateCreated");
            DropColumn("dbo.Companies", "Longitude");
            DropColumn("dbo.Companies", "Latitude");
            DropColumn("dbo.Companies", "Coordinate");
            DropColumn("dbo.Companies", "EndHours");
            DropColumn("dbo.Companies", "StartHours");
        }
    }
}
