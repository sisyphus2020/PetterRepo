namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanionAnimal : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CommonCodes", "IX_COMMONCODE_PARENTCODEID");
            DropColumn("dbo.CommonCodes", "ParentCodeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommonCodes", "ParentCodeID", c => c.String(maxLength: 6, fixedLength: true, unicode: false));
            CreateIndex("dbo.CommonCodes", "ParentCodeID", name: "IX_COMMONCODE_PARENTCODEID");
        }
    }
}
