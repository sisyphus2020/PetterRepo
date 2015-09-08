namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetInformation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PetInfomations",
                c => new
                    {
                        PetInfomationNo = c.Int(nullable: false, identity: true),
                        PetCategory = c.String(maxLength: 20),
                        PetCode = c.String(maxLength: 4),
                        MemberNo = c.Int(nullable: false),
                        Name = c.String(maxLength: 20),
                        Age = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        Gender = c.String(maxLength: 1),
                        Marking = c.String(maxLength: 1),
                        Medication = c.String(maxLength: 1),
                        Feature = c.String(maxLength: 200),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PetInfomationNo)
                .ForeignKey("dbo.Members", t => t.MemberNo, cascadeDelete: true)
                .ForeignKey("dbo.PetKinds", t => new { t.PetCategory, t.PetCode })
                .Index(t => new { t.PetCategory, t.PetCode })
                .Index(t => t.MemberNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetInfomations", new[] { "PetCategory", "PetCode" }, "dbo.PetKinds");
            DropForeignKey("dbo.PetInfomations", "MemberNo", "dbo.Members");
            DropIndex("dbo.PetInfomations", new[] { "MemberNo" });
            DropIndex("dbo.PetInfomations", new[] { "PetCategory", "PetCode" });
            DropTable("dbo.PetInfomations");
        }
    }
}
