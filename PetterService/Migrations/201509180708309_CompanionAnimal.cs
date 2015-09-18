namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanionAnimals", "Neutralization", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanionAnimals", "Neutralization");
        }
    }
}
