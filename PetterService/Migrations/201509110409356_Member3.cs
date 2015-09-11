namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Member3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BeautyShops", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.BeautyShops", "DateModified", c => c.DateTime());
            AlterColumn("dbo.BeautyShops", "DateDeleted", c => c.DateTime());
            AlterColumn("dbo.Companies", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Companies", "DateModified", c => c.DateTime());
            AlterColumn("dbo.Companies", "DateDeleted", c => c.DateTime());
            AlterColumn("dbo.Members", "PictureName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Members", "PicturePath", c => c.String(maxLength: 100));
            AlterColumn("dbo.Members", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Members", "DateModified", c => c.DateTime());
            AlterColumn("dbo.Members", "DateDeleted", c => c.DateTime());
            AlterColumn("dbo.Pensions", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Pensions", "DateModified", c => c.DateTime());
            AlterColumn("dbo.Pensions", "DateDeleted", c => c.DateTime());
            AlterColumn("dbo.PetSitters", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.PetSitters", "DateModified", c => c.DateTime());
            AlterColumn("dbo.PetSitters", "DateDeleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PetSitters", "DateDeleted", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PetSitters", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PetSitters", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Pensions", "DateDeleted", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Pensions", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Pensions", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Members", "DateDeleted", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Members", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Members", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Members", "PicturePath", c => c.String(maxLength: 200));
            AlterColumn("dbo.Members", "PictureName", c => c.String(maxLength: 200));
            AlterColumn("dbo.Companies", "DateDeleted", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Companies", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Companies", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BeautyShops", "DateDeleted", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BeautyShops", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BeautyShops", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
