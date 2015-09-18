namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanionAnimals",
                c => new
                    {
                        CompanionAnimalNo = c.Int(nullable: false, identity: true),
                        MemberNo = c.Int(nullable: false),
                        PetKindNo = c.Int(nullable: false),
                        Name = c.String(maxLength: 20),
                        Age = c.Byte(nullable: false),
                        Weight = c.Byte(nullable: false),
                        Gender = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Neutralization = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Marking = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Medication = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Feature = c.String(maxLength: 200),
                        FileName = c.String(maxLength: 100),
                        FilePath = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.CompanionAnimalNo)
                .ForeignKey("dbo.Members", t => t.MemberNo)
                .ForeignKey("dbo.PetKinds", t => t.PetKindNo)
                .Index(t => t.MemberNo)
                .Index(t => t.PetKindNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanionAnimals", "PetKindNo", "dbo.PetKinds");
            DropForeignKey("dbo.CompanionAnimals", "MemberNo", "dbo.Members");
            DropIndex("dbo.CompanionAnimals", new[] { "PetKindNo" });
            DropIndex("dbo.CompanionAnimals", new[] { "MemberNo" });
            DropTable("dbo.CompanionAnimals");
        }
    }
}
