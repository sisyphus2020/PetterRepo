namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventNo = c.Int(nullable: false, identity: true),
                        MemberNo = c.Int(nullable: false),
                        Title = c.String(maxLength: 200),
                        Content = c.String(maxLength: 4000),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.EventNo)
                .ForeignKey("dbo.Members", t => t.MemberNo, cascadeDelete: true)
                .Index(t => t.MemberNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "MemberNo", "dbo.Members");
            DropIndex("dbo.Events", new[] { "MemberNo" });
            DropTable("dbo.Events");
        }
    }
}
