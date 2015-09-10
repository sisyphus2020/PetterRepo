namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Classification3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BeautyShops", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AlterColumn("dbo.Companies", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AlterColumn("dbo.Pensions", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AlterColumn("dbo.PetSitters", "StateFlag", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PetSitters", "StateFlag", c => c.String(maxLength: 1));
            AlterColumn("dbo.Pensions", "StateFlag", c => c.String(maxLength: 1));
            AlterColumn("dbo.Companies", "StateFlag", c => c.String(maxLength: 1));
            AlterColumn("dbo.BeautyShops", "StateFlag", c => c.String(maxLength: 1));
        }
    }
}
