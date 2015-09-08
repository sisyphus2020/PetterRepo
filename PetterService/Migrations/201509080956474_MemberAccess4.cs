namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberAccess4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberAccesses", "AccessResult", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberAccesses", "AccessResult");
        }
    }
}
