namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Member1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "MemberID", c => c.String(maxLength: 50));
            AlterColumn("dbo.Members", "NickName", c => c.String(maxLength: 50));
            CreateIndex("dbo.Members", "MemberID", unique: true, name: "IDX_MEMBER_MEMBERID");
            CreateIndex("dbo.Members", "NickName", unique: true, name: "IDX_MEMBER_NICKNAME");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Members", "IDX_MEMBER_NICKNAME");
            DropIndex("dbo.Members", "IDX_MEMBER_MEMBERID");
            AlterColumn("dbo.Members", "NickName", c => c.String());
            AlterColumn("dbo.Members", "MemberID", c => c.String());
        }
    }
}
