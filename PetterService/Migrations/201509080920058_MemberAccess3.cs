namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberAccess3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MemberAccesses", "AccessResult");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberAccesses", "AccessResult", c => c.String());
        }
    }
}
