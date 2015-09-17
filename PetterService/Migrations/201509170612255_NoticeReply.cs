namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticeReply : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.NoticeReplies", "MemberNo");
            AddForeignKey("dbo.NoticeReplies", "MemberNo", "dbo.Members", "MemberNo", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NoticeReplies", "MemberNo", "dbo.Members");
            DropIndex("dbo.NoticeReplies", new[] { "MemberNo" });
        }
    }
}
