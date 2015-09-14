namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeautyShop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BeautyShops",
                c => new
                    {
                        BeautyShopNo = c.Int(nullable: false, identity: true),
                        CompanyNo = c.Int(nullable: false),
                        BeautyShopName = c.String(),
                        BeautyShopAddr = c.String(),
                        FileName = c.String(),
                        FilePath = c.String(),
                        StartBeautyShop = c.String(),
                        EndBeautyShop = c.String(),
                        Introduction = c.String(),
                        Coordinate = c.Geography(),
                        Grade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReviewCount = c.Int(nullable: false),
                        Bookmark = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BeautyShopNo)
                .ForeignKey("dbo.Companies", t => t.CompanyNo, cascadeDelete: true)
                .Index(t => t.CompanyNo);
            
            CreateTable(
                "dbo.BeautyShopHolidays",
                c => new
                    {
                        BeautyShopHolidayNo = c.Int(nullable: false, identity: true),
                        BeautyShopNo = c.Int(nullable: false),
                        BeautyShopHolidayCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeautyShopHolidayNo)
                .ForeignKey("dbo.BeautyShops", t => t.BeautyShopNo, cascadeDelete: true)
                .Index(t => t.BeautyShopNo);
            
            CreateTable(
                "dbo.BeautyShopServices",
                c => new
                    {
                        BeautyShopServiceNo = c.Int(nullable: false, identity: true),
                        BeautyShopNo = c.Int(nullable: false),
                        BeautyShopServiceCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeautyShopServiceNo)
                .ForeignKey("dbo.BeautyShops", t => t.BeautyShopNo, cascadeDelete: true)
                .Index(t => t.BeautyShopNo);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyNo = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        CompanyAddr = c.String(),
                        FileName = c.String(),
                        FilePath = c.String(),
                        StartShopHours = c.String(),
                        EndShopHours = c.String(),
                        Holiday = c.String(),
                        Introduction = c.String(),
                        Geography = c.Geography(),
                        Grade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReviewCount = c.Int(nullable: false),
                        BookMark = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyNo);
            
            CreateIndex("dbo.Pensions", "CompanyNo");
            AddForeignKey("dbo.Pensions", "CompanyNo", "dbo.Companies", "CompanyNo", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pensions", "CompanyNo", "dbo.Companies");
            DropForeignKey("dbo.BeautyShops", "CompanyNo", "dbo.Companies");
            DropForeignKey("dbo.BeautyShopServices", "BeautyShopNo", "dbo.BeautyShops");
            DropForeignKey("dbo.BeautyShopHolidays", "BeautyShopNo", "dbo.BeautyShops");
            DropIndex("dbo.Pensions", new[] { "CompanyNo" });
            DropIndex("dbo.BeautyShopServices", new[] { "BeautyShopNo" });
            DropIndex("dbo.BeautyShopHolidays", new[] { "BeautyShopNo" });
            DropIndex("dbo.BeautyShops", new[] { "CompanyNo" });
            DropTable("dbo.Companies");
            DropTable("dbo.BeautyShopServices");
            DropTable("dbo.BeautyShopHolidays");
            DropTable("dbo.BeautyShops");
        }
    }
}
