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
                        CommonCodeNo = c.Int(nullable: false, identity: true),
                        Category = c.String(maxLength: 20),
                        Code = c.String(maxLength: 4, fixedLength: true, unicode: false),
                        CodeName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CommonCodeNo);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommonCodes");
        }
    }
}
