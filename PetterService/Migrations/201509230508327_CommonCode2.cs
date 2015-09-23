namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommonCode2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CommonCodes", "IX_COMMONCODE_CODEID");
            CreateIndex("dbo.CommonCodes", "CodeID", unique: true, name: "IX_COMMONCODE_CODEID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CommonCodes", "IX_COMMONCODE_CODEID");
            CreateIndex("dbo.CommonCodes", "CodeID", name: "IX_COMMONCODE_CODEID");
        }
    }
}
