using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PetterService.Models
{
    public class PetterServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PetterServiceContext() : base("name=PetterServiceContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<EventBoard>()
            //   .HasRequired(f => f.Member)
            //   .WithRequiredDependent()
            //   .WillCascadeOnDelete(false);

            //modelBuilder.Entity<EventBoardReply>()
            //   .HasRequired(f => f.Member)
            //   .WithRequiredDependent()
            //   .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<PetterService.Models.Pension> Pensions { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BeautyShop> BeautyShops { get; set; }

        //public System.Data.Entity.DbSet<PetterService.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PensionHoliday> PensionHolidays { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PensionService> PensionServices { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BeautyShopService> BeautyShopServices { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BeautyShopHoliday> BeautyShopHolidays { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PetSitterService> PetSitterServices { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PetSitterHoliday> PetSitterHolidays { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PetSitter> PetSitters { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.Member> Members { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.MemberAccess> MemberAccesses { get; set; }

        //public System.Data.Entity.DbSet<PetterService.Models.PetInfomation> PetInfomations { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PetKind> PetKinds { get; set; }

        //public System.Data.Entity.DbSet<PetterService.Models.PetInfomation> PetInfomations { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.CommonCode> CommonCodes { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.CompanionAnimal> CompanionAnimals { get; set; }

        //public System.Data.Entity.DbSet<PetterService.Models.Event> Events { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.EventBoard> EventBoards { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.EventBoardFile> EventBoardFiles { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.EventBoardReply> EventBoardReplies { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BeautyShopBookmark> BeautyShopBookmarks { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BeautyShopStats> BeautyShopStats { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.Notice> Notices { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.NoticeFile> NoticeFiles { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.EventBoardStats> EventBoardStats { get; set; }

        //public System.Data.Entity.DbSet<PetterService.Models.BeautyShopStatistics> BeautyShopStatistics { get; set; }

        //public System.Data.Entity.DbSet<PetterService.Models.BeautyShopStatistics> BeautyShopStatistics { get; set; }

        //public System.Data.Entity.DbSet<PetterService.Models.EventBoardReply> EventBoardReplies { get; set; }
    }
}
