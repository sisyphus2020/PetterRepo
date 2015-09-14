namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pensions",
                c => new
                    {
                        PensionNo = c.Int(nullable: false, identity: true),
                        CompanyNo = c.Int(nullable: false),
                        PensionName = c.String(),
                        PensionAddr = c.String(),
                        FileName = c.String(),
                        FilePath = c.String(),
                        StartPension = c.String(),
                        EndPension = c.String(),
                        Introduction = c.String(),
                        Coordinate = c.Geography(),
                        Grade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReviewCount = c.Int(nullable: false),
                        Bookmark = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PensionNo);
            
            CreateTable(
                "dbo.PensionHolidays",
                c => new
                    {
                        PensionHolidayNo = c.Int(nullable: false, identity: true),
                        PensionNo = c.Int(nullable: false),
                        PensionHolidayCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PensionHolidayNo)
                .ForeignKey("dbo.Pensions", t => t.PensionNo, cascadeDelete: true)
                .Index(t => t.PensionNo);
            
            CreateTable(
                "dbo.PensionServices",
                c => new
                    {
                        PensionServiceNo = c.Int(nullable: false, identity: true),
                        PensionNo = c.Int(nullable: false),
                        PensionServiceCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PensionServiceNo)
                .ForeignKey("dbo.Pensions", t => t.PensionNo, cascadeDelete: true)
                .Index(t => t.PensionNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PensionServices", "PensionNo", "dbo.Pensions");
            DropForeignKey("dbo.PensionHolidays", "PensionNo", "dbo.Pensions");
            DropIndex("dbo.PensionServices", new[] { "PensionNo" });
            DropIndex("dbo.PensionHolidays", new[] { "PensionNo" });
            DropTable("dbo.PensionServices");
            DropTable("dbo.PensionHolidays");
            DropTable("dbo.Pensions");
        }
    }
}
