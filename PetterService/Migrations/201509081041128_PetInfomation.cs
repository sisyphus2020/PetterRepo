namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetInfomation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PetInfomations",
                c => new
                    {
                        PetInfomationNo = c.Int(nullable: false, identity: true),
                        MemberNo = c.Int(nullable: false),
                        Name = c.String(maxLength: 20),
                        Age = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        Gender = c.String(maxLength: 1),
                        Marking = c.String(maxLength: 1),
                        Medication = c.String(maxLength: 1),
                        Feature = c.String(maxLength: 200),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PetInfomationNo)
                .ForeignKey("dbo.Members", t => t.MemberNo, cascadeDelete: true)
                .Index(t => t.MemberNo);
            
            AlterColumn("dbo.MemberAccesses", "MemberID", c => c.String(maxLength: 500));
            CreateIndex("dbo.MemberAccesses", "MemberID", name: "IDX_MEMBERACCESS_MEMBERID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetInfomations", "MemberNo", "dbo.Members");
            DropIndex("dbo.PetInfomations", new[] { "MemberNo" });
            DropIndex("dbo.MemberAccesses", "IDX_MEMBERACCESS_MEMBERID");
            AlterColumn("dbo.MemberAccesses", "MemberID", c => c.String(maxLength: 50));
            DropTable("dbo.PetInfomations");
        }
    }
}
