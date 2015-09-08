namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Classification : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BeautyShops", "IDX_CRADE");
            DropIndex("dbo.Companies", "IDX_CRADE");
            DropIndex("dbo.Pensions", "IDX_CRADE");
            DropIndex("dbo.PetSitters", "IDX_CRADE");
            AlterColumn("dbo.BeautyShops", "Grade", c => c.Single(nullable: false));
            AlterColumn("dbo.Companies", "Grade", c => c.Single(nullable: false));
            AlterColumn("dbo.Pensions", "Grade", c => c.Single(nullable: false));
            AlterColumn("dbo.PetSitters", "Grade", c => c.Single(nullable: false));
            CreateIndex("dbo.BeautyShops", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.Companies", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.Pensions", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.PetSitters", "Grade", name: "IDX_CRADE");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PetSitters", "IDX_CRADE");
            DropIndex("dbo.Pensions", "IDX_CRADE");
            DropIndex("dbo.Companies", "IDX_CRADE");
            DropIndex("dbo.BeautyShops", "IDX_CRADE");
            AlterColumn("dbo.PetSitters", "Grade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Pensions", "Grade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Companies", "Grade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BeautyShops", "Grade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.PetSitters", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.Pensions", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.Companies", "Grade", name: "IDX_CRADE");
            CreateIndex("dbo.BeautyShops", "Grade", name: "IDX_CRADE");
        }
    }
}
