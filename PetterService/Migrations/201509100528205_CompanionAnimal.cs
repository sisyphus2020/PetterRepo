namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanionAnimals", new[] { "PetCategory", "PetCode" }, "dbo.PetKinds");
            DropIndex("dbo.CompanionAnimals", new[] { "PetCategory", "PetCode" });
            DropPrimaryKey("dbo.PetKinds");
            AlterColumn("dbo.PetKinds", "PetCategory", c => c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false));
            AlterColumn("dbo.PetKinds", "PetCode", c => c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false));
            AddPrimaryKey("dbo.PetKinds", new[] { "PetCategory", "PetCode" });
            DropColumn("dbo.CompanionAnimals", "PetCategory");
            DropColumn("dbo.CompanionAnimals", "PetCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanionAnimals", "PetCode", c => c.String(maxLength: 4));
            AddColumn("dbo.CompanionAnimals", "PetCategory", c => c.String(maxLength: 3));
            DropPrimaryKey("dbo.PetKinds");
            AlterColumn("dbo.PetKinds", "PetCode", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.PetKinds", "PetCategory", c => c.String(nullable: false, maxLength: 3));
            AddPrimaryKey("dbo.PetKinds", new[] { "PetCategory", "PetCode" });
            CreateIndex("dbo.CompanionAnimals", new[] { "PetCategory", "PetCode" });
            AddForeignKey("dbo.CompanionAnimals", new[] { "PetCategory", "PetCode" }, "dbo.PetKinds", new[] { "PetCategory", "PetCode" });
        }
    }
}
