namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanionAnimals", "MemberNo", "dbo.Members");
            DropForeignKey("dbo.CompanionAnimals", "PetKindNo", "dbo.PetKinds");
            DropIndex("dbo.CompanionAnimals", new[] { "MemberNo" });
            DropIndex("dbo.CompanionAnimals", new[] { "PetKindNo" });
            AddColumn("dbo.CompanionAnimals", "MemberID", c => c.String(maxLength: 50));
            AddColumn("dbo.CompanionAnimals", "CodeID", c => c.String(maxLength: 6, fixedLength: true, unicode: false));
            CreateIndex("dbo.CompanionAnimals", "MemberID", name: "IX_STORE_MEMBERID");
            CreateIndex("dbo.CompanionAnimals", "CodeID", name: "IX_STORE_CODEID");
            DropColumn("dbo.CompanionAnimals", "MemberNo");
            DropColumn("dbo.CompanionAnimals", "PetKindNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanionAnimals", "PetKindNo", c => c.Int(nullable: false));
            AddColumn("dbo.CompanionAnimals", "MemberNo", c => c.Int(nullable: false));
            DropIndex("dbo.CompanionAnimals", "IX_STORE_CODEID");
            DropIndex("dbo.CompanionAnimals", "IX_STORE_MEMBERID");
            DropColumn("dbo.CompanionAnimals", "CodeID");
            DropColumn("dbo.CompanionAnimals", "MemberID");
            CreateIndex("dbo.CompanionAnimals", "PetKindNo");
            CreateIndex("dbo.CompanionAnimals", "MemberNo");
            AddForeignKey("dbo.CompanionAnimals", "PetKindNo", "dbo.PetKinds", "PetKindNo");
            AddForeignKey("dbo.CompanionAnimals", "MemberNo", "dbo.Members", "MemberNo");
        }
    }
}
