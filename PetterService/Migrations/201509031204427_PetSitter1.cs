namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetSitter1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PetSitters", "CompanyNo", "dbo.Companies");
            DropForeignKey("dbo.PetSitterHolidays", "PetSitterNo", "dbo.PetSitters");
            DropForeignKey("dbo.PetSitterServices", "PetSitterNo", "dbo.PetSitters");
            DropIndex("dbo.PetSitterHolidays", new[] { "PetSitterNo" });
            DropIndex("dbo.PetSitters", new[] { "CompanyNo" });
            DropIndex("dbo.PetSitterServices", new[] { "PetSitterNo" });
            DropTable("dbo.PetSitters");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PetSitters",
                c => new
                    {
                        PetSitterNo = c.Int(nullable: false, identity: true),
                        PetSitterName = c.String(),
                        PetSitterAddr = c.String(),
                        CompanyNo = c.Int(nullable: false),
                        FileName = c.String(),
                        FilePath = c.String(),
                        StartHours = c.String(),
                        EndHours = c.String(),
                        Introduction = c.String(),
                        Coordinate = c.Geography(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Grade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReviewCount = c.Int(nullable: false),
                        Bookmark = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PetSitterNo);
            
            CreateIndex("dbo.PetSitterServices", "PetSitterNo");
            CreateIndex("dbo.PetSitters", "CompanyNo");
            CreateIndex("dbo.PetSitterHolidays", "PetSitterNo");
            AddForeignKey("dbo.PetSitterServices", "PetSitterNo", "dbo.PetSitters", "PetSitterNo", cascadeDelete: true);
            AddForeignKey("dbo.PetSitterHolidays", "PetSitterNo", "dbo.PetSitters", "PetSitterNo", cascadeDelete: true);
            AddForeignKey("dbo.PetSitters", "CompanyNo", "dbo.Companies", "CompanyNo", cascadeDelete: true);
        }
    }
}
