namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanionAnimals", "DateCreated", c => c.DateTime());
            AddColumn("dbo.CompanionAnimals", "DateModified", c => c.DateTime());
            AddColumn("dbo.CompanionAnimals", "DateDeleted", c => c.DateTime());
            DropColumn("dbo.CompanionAnimals", "DateDetails_DateCreated");
            DropColumn("dbo.CompanionAnimals", "DateDetails_DateModified");
            DropColumn("dbo.CompanionAnimals", "DateDetails_DateDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanionAnimals", "DateDetails_DateDeleted", c => c.DateTime());
            AddColumn("dbo.CompanionAnimals", "DateDetails_DateModified", c => c.DateTime());
            AddColumn("dbo.CompanionAnimals", "DateDetails_DateCreated", c => c.DateTime());
            DropColumn("dbo.CompanionAnimals", "DateDeleted");
            DropColumn("dbo.CompanionAnimals", "DateModified");
            DropColumn("dbo.CompanionAnimals", "DateCreated");
        }
    }
}
