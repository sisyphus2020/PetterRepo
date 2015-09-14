namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoard : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "MemberNo", "dbo.Members");
            DropIndex("dbo.Events", new[] { "MemberNo" });
            CreateTable(
                "dbo.EventBoards",
                c => new
                    {
                        EventBoardNo = c.Int(nullable: false, identity: true),
                        MemberNo = c.Int(nullable: false),
                        Title = c.String(maxLength: 200),
                        Content = c.String(maxLength: 4000),
                        StateFlag = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.EventBoardNo)
                .ForeignKey("dbo.Members", t => t.MemberNo, cascadeDelete: true)
                .Index(t => t.MemberNo);
            
            DropTable("dbo.Events");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.EventNo);
            
            DropForeignKey("dbo.EventBoards", "MemberNo", "dbo.Members");
            DropIndex("dbo.EventBoards", new[] { "MemberNo" });
            DropTable("dbo.EventBoards");
            CreateIndex("dbo.Events", "MemberNo");
            AddForeignKey("dbo.Events", "MemberNo", "dbo.Members", "MemberNo", cascadeDelete: true);
        }
    }
}
