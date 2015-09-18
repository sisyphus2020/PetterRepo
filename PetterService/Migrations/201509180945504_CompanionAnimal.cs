namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanionAnimals", "MemberNo", "dbo.Members");
            DropIndex("dbo.CompanionAnimals", new[] { "MemberNo" });
            DropTable("dbo.CompanionAnimals");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.CompanionAnimalNo);
            
            CreateIndex("dbo.CompanionAnimals", "MemberNo");
            AddForeignKey("dbo.CompanionAnimals", "MemberNo", "dbo.Members", "MemberNo");
        }
    }
}
