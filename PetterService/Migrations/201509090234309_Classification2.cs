namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Classification2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeautyShops", "Phone", c => c.String(maxLength: 20));
            AddColumn("dbo.BeautyShops", "StartTime", c => c.String(maxLength: 4));
            AddColumn("dbo.BeautyShops", "EndTime", c => c.String(maxLength: 4));
            AddColumn("dbo.BeautyShops", "StateFlag", c => c.String(maxLength: 1));
            AddColumn("dbo.BeautyShops", "WriteIP", c => c.String(maxLength: 20));
            AddColumn("dbo.BeautyShops", "ModifyIP", c => c.String(maxLength: 20));
            AddColumn("dbo.BeautyShops", "DateDeleted", c => c.DateTime(nullable: false));
            AddColumn("dbo.Companies", "Phone", c => c.String(maxLength: 20));
            AddColumn("dbo.Companies", "StartTime", c => c.String(maxLength: 4));
            AddColumn("dbo.Companies", "EndTime", c => c.String(maxLength: 4));
            AddColumn("dbo.Companies", "StateFlag", c => c.String(maxLength: 1));
            AddColumn("dbo.Companies", "WriteIP", c => c.String(maxLength: 20));
            AddColumn("dbo.Companies", "ModifyIP", c => c.String(maxLength: 20));
            AddColumn("dbo.Companies", "DateDeleted", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pensions", "Phone", c => c.String(maxLength: 20));
            AddColumn("dbo.Pensions", "StartTime", c => c.String(maxLength: 4));
            AddColumn("dbo.Pensions", "EndTime", c => c.String(maxLength: 4));
            AddColumn("dbo.Pensions", "StateFlag", c => c.String(maxLength: 1));
            AddColumn("dbo.Pensions", "WriteIP", c => c.String(maxLength: 20));
            AddColumn("dbo.Pensions", "ModifyIP", c => c.String(maxLength: 20));
            AddColumn("dbo.Pensions", "DateDeleted", c => c.DateTime(nullable: false));
            AddColumn("dbo.PetSitters", "Phone", c => c.String(maxLength: 20));
            AddColumn("dbo.PetSitters", "StartTime", c => c.String(maxLength: 4));
            AddColumn("dbo.PetSitters", "EndTime", c => c.String(maxLength: 4));
            AddColumn("dbo.PetSitters", "StateFlag", c => c.String(maxLength: 1));
            AddColumn("dbo.PetSitters", "WriteIP", c => c.String(maxLength: 20));
            AddColumn("dbo.PetSitters", "ModifyIP", c => c.String(maxLength: 20));
            AddColumn("dbo.PetSitters", "DateDeleted", c => c.DateTime(nullable: false));
            DropColumn("dbo.BeautyShops", "StartHours");
            DropColumn("dbo.BeautyShops", "EndHours");
            DropColumn("dbo.Companies", "StartHours");
            DropColumn("dbo.Companies", "EndHours");
            DropColumn("dbo.Pensions", "StartHours");
            DropColumn("dbo.Pensions", "EndHours");
            DropColumn("dbo.PetSitters", "StartHours");
            DropColumn("dbo.PetSitters", "EndHours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PetSitters", "EndHours", c => c.String(maxLength: 4));
            AddColumn("dbo.PetSitters", "StartHours", c => c.String(maxLength: 4));
            AddColumn("dbo.Pensions", "EndHours", c => c.String(maxLength: 4));
            AddColumn("dbo.Pensions", "StartHours", c => c.String(maxLength: 4));
            AddColumn("dbo.Companies", "EndHours", c => c.String(maxLength: 4));
            AddColumn("dbo.Companies", "StartHours", c => c.String(maxLength: 4));
            AddColumn("dbo.BeautyShops", "EndHours", c => c.String(maxLength: 4));
            AddColumn("dbo.BeautyShops", "StartHours", c => c.String(maxLength: 4));
            DropColumn("dbo.PetSitters", "DateDeleted");
            DropColumn("dbo.PetSitters", "ModifyIP");
            DropColumn("dbo.PetSitters", "WriteIP");
            DropColumn("dbo.PetSitters", "StateFlag");
            DropColumn("dbo.PetSitters", "EndTime");
            DropColumn("dbo.PetSitters", "StartTime");
            DropColumn("dbo.PetSitters", "Phone");
            DropColumn("dbo.Pensions", "DateDeleted");
            DropColumn("dbo.Pensions", "ModifyIP");
            DropColumn("dbo.Pensions", "WriteIP");
            DropColumn("dbo.Pensions", "StateFlag");
            DropColumn("dbo.Pensions", "EndTime");
            DropColumn("dbo.Pensions", "StartTime");
            DropColumn("dbo.Pensions", "Phone");
            DropColumn("dbo.Companies", "DateDeleted");
            DropColumn("dbo.Companies", "ModifyIP");
            DropColumn("dbo.Companies", "WriteIP");
            DropColumn("dbo.Companies", "StateFlag");
            DropColumn("dbo.Companies", "EndTime");
            DropColumn("dbo.Companies", "StartTime");
            DropColumn("dbo.Companies", "Phone");
            DropColumn("dbo.BeautyShops", "DateDeleted");
            DropColumn("dbo.BeautyShops", "ModifyIP");
            DropColumn("dbo.BeautyShops", "WriteIP");
            DropColumn("dbo.BeautyShops", "StateFlag");
            DropColumn("dbo.BeautyShops", "EndTime");
            DropColumn("dbo.BeautyShops", "StartTime");
            DropColumn("dbo.BeautyShops", "Phone");
        }
    }
}
