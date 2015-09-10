namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanionAnimals", "PetCategory", c => c.String(maxLength: 3, fixedLength: true, unicode: false));
            AddColumn("dbo.CompanionAnimals", "PetCode", c => c.String(maxLength: 4, fixedLength: true, unicode: false));
            CreateIndex("dbo.CompanionAnimals", new[] { "PetCategory", "PetCode" });
            AddForeignKey("dbo.CompanionAnimals", new[] { "PetCategory", "PetCode" }, "dbo.PetKinds", new[] { "PetCategory", "PetCode" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanionAnimals", new[] { "PetCategory", "PetCode" }, "dbo.PetKinds");
            DropIndex("dbo.CompanionAnimals", new[] { "PetCategory", "PetCode" });
            DropColumn("dbo.CompanionAnimals", "PetCode");
            DropColumn("dbo.CompanionAnimals", "PetCategory");
        }
    }
}
