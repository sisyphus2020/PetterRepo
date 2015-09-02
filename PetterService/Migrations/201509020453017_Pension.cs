namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pensions", "StartPensionHours", c => c.String());
            AddColumn("dbo.Pensions", "EndPensionHours", c => c.String());
            DropColumn("dbo.Pensions", "StartPension");
            DropColumn("dbo.Pensions", "EndPension");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pensions", "EndPension", c => c.String());
            AddColumn("dbo.Pensions", "StartPension", c => c.String());
            DropColumn("dbo.Pensions", "EndPensionHours");
            DropColumn("dbo.Pensions", "StartPensionHours");
        }
    }
}
