namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class Company2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "FileName", c => c.String(maxLength: 100));
            AddColumn("dbo.Companies", "FilePath", c => c.String(maxLength: 100));
            AddColumn("dbo.Companies", "StartHours", c => c.String(maxLength: 4));
            AddColumn("dbo.Companies", "EndHours", c => c.String(maxLength: 4));
            AddColumn("dbo.Companies", "Introduction", c => c.String(maxLength: 200));
            AddColumn("dbo.Companies", "Coordinate", c => c.Geography());
            AddColumn("dbo.Companies", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Companies", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Companies", "Grade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Companies", "ReviewCount", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "Bookmark", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Companies", "DateModified", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Companies", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.Companies", "ReviewCount", name: "IDX_REVIEWCOUNT");
            CreateIndex("dbo.Companies", "Bookmark", name: "IDX_BOOKMARK");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Companies", "IDX_BOOKMARK");
            DropIndex("dbo.Companies", "IDX_REVIEWCOUNT");
            DropIndex("dbo.Companies", "IDX_CRADE");
            DropColumn("dbo.Companies", "DateModified");
            DropColumn("dbo.Companies", "DateCreated");
            DropColumn("dbo.Companies", "Bookmark");
            DropColumn("dbo.Companies", "ReviewCount");
            DropColumn("dbo.Companies", "Grade");
            DropColumn("dbo.Companies", "Longitude");
            DropColumn("dbo.Companies", "Latitude");
            DropColumn("dbo.Companies", "Coordinate");
            DropColumn("dbo.Companies", "Introduction");
            DropColumn("dbo.Companies", "EndHours");
            DropColumn("dbo.Companies", "StartHours");
            DropColumn("dbo.Companies", "FilePath");
            DropColumn("dbo.Companies", "FileName");
        }
    }
}
