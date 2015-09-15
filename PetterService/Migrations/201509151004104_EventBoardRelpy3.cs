namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventBoardRelpy3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Members", "IDX_MEMBER_MEMBERID");
            CreateIndex("dbo.Members", "MemberID", unique: true, name: "IDX_MEMBER_MEMBERID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Members", "IDX_MEMBER_MEMBERID");
            CreateIndex("dbo.Members", "MemberID", unique: true, name: "IDX_MEMBER_MEMBERID");
        }
    }
}
