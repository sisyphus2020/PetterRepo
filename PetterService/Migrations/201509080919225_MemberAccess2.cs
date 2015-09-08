namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberAccess2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberAccesses", "AccessResult", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberAccesses", "AccessResult");
        }
    }
}
