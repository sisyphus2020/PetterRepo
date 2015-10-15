namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommonCodes", "ParentCodeID", c => c.String(maxLength: 3, fixedLength: true, unicode: false));
            CreateIndex("dbo.CommonCodes", "ParentCodeID", name: "IX_COMMONCODE_PARENTCODEID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CommonCodes", "IX_COMMONCODE_PARENTCODEID");
            DropColumn("dbo.CommonCodes", "ParentCodeID");
        }
    }
}
