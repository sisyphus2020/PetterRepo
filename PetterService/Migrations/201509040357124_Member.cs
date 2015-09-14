namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Member : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberNo = c.Int(nullable: false, identity: true),
                        MemberID = c.String(),
                        Password = c.String(),
                        NickName = c.String(),
                        FileName = c.String(),
                        FilePath = c.String(),
                        Coordinate = c.Geography(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MemberNo);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Members");
        }
    }
}
