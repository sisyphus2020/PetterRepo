namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommondCode : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.CommonCodes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CommonCodes",
                c => new
                    {
                        CoommonCodeNo = c.Int(nullable: false, identity: true),
                        Category = c.String(maxLength: 20),
                        Code = c.String(maxLength: 4, fixedLength: true, unicode: false),
                        CodeName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CoommonCodeNo);
            
        }
    }
}
