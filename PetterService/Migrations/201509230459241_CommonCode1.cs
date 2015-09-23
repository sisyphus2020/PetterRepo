namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommonCode1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommonCodes",
                c => new
                    {
                        CodeNo = c.Int(nullable: false, identity: true),
                        CodeID = c.String(maxLength: 6, fixedLength: true, unicode: false),
                        ParentCodeID = c.String(maxLength: 6, fixedLength: true, unicode: false),
                        CodeName = c.String(maxLength: 50),
                        CodeDescription = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.CodeNo)
                .Index(t => t.CodeID, name: "IX_COMMONCODE_CODEID")
                .Index(t => t.ParentCodeID, name: "IX_COMMONCODE_PARENTCODEID");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CommonCodes", "IX_COMMONCODE_PARENTCODEID");
            DropIndex("dbo.CommonCodes", "IX_COMMONCODE_CODEID");
            DropTable("dbo.CommonCodes");
        }
    }
}
