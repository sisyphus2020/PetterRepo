namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetInfomation1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.MemberAccesses", "IDX_MEMBERACCESS_MEMBERID");
            AlterColumn("dbo.MemberAccesses", "MemberID", c => c.String(maxLength: 50));
            CreateIndex("dbo.MemberAccesses", "MemberID", name: "IDX_MEMBERACCESS_MEMBERID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MemberAccesses", "IDX_MEMBERACCESS_MEMBERID");
            AlterColumn("dbo.MemberAccesses", "MemberID", c => c.String(maxLength: 500));
            CreateIndex("dbo.MemberAccesses", "MemberID", name: "IDX_MEMBERACCESS_MEMBERID");
        }
    }
}
