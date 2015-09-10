namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberAccess1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberAccesses", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberAccesses", "DateCreated");
        }
    }
}
