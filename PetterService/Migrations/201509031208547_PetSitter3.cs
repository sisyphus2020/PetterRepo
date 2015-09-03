namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetSitter3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PetSitters",
                c => new
                    {
                        PetSitterNo = c.Int(nullable: false, identity: true),
                        PetSitterName = c.String(),
                        PetSitterAddr = c.String(),
                        CompanyNo = c.Int(nullable: false),
                        PictureName = c.String(),
                        PicturePath = c.String(),
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
                .PrimaryKey(t => t.PetSitterNo)
                .ForeignKey("dbo.Companies", t => t.CompanyNo, cascadeDelete: true)
                .Index(t => t.CompanyNo);
            
            CreateIndex("dbo.PetSitterHolidays", "PetSitterNo");
            CreateIndex("dbo.PetSitterServices", "PetSitterNo");
            AddForeignKey("dbo.PetSitterHolidays", "PetSitterNo", "dbo.PetSitters", "PetSitterNo", cascadeDelete: true);
            AddForeignKey("dbo.PetSitterServices", "PetSitterNo", "dbo.PetSitters", "PetSitterNo", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetSitterServices", "PetSitterNo", "dbo.PetSitters");
            DropForeignKey("dbo.PetSitterHolidays", "PetSitterNo", "dbo.PetSitters");
            DropForeignKey("dbo.PetSitters", "CompanyNo", "dbo.Companies");
            DropIndex("dbo.PetSitterServices", new[] { "PetSitterNo" });
            DropIndex("dbo.PetSitters", new[] { "CompanyNo" });
            DropIndex("dbo.PetSitterHolidays", new[] { "PetSitterNo" });
            DropTable("dbo.PetSitters");
        }
    }
}
