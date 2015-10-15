namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class store : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Stores", "CommonCodeNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stores", "CommonCodeNo", c => c.Int(nullable: false));
        }
    }
}
