﻿using System;
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
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

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

        public System.Data.Entity.DbSet<PetterService.Models.Store> Stores { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PensionHoliday> PensionHolidays { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PensionService> PensionServices { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreService> StoreServices { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreHoliday> StoreHolidays { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PetSitterService> PetSitterServices { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PetSitterHoliday> PetSitterHolidays { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PetSitter> PetSitters { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.Member> Members { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.MemberAccess> MemberAccesses { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.EventBoard> EventBoards { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.EventBoardFile> EventBoardFiles { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.EventBoardReply> EventBoardReplies { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreBookmark> BeautyShopBookmarks { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreStats> StoreStats { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.Notice> Notices { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.NoticeFile> NoticeFiles { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.EventBoardStats> EventBoardStats { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.NoticeStats> NoticeStats { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.NoticeReply> NoticeReplies { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreReview> StoreReviews { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreReviewFile> StoreReviewFiles { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreReviewStats> StoreReviewStats { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreReviewLike> StoreReviewLikes { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.PetKind> PetKinds { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.CompanionAnimal> CompanionAnimals { get; set; }

        //public System.Data.Entity.DbSet<PetterService.Models.CommonCode> CommonCodes { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BoardStats> BoardStats { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BoardLike> BoardLikes { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BoardReply> BoardReplies { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.BoardFile> BoardFiles { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.Board> Boards { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.CommonCode> CommonCodes { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreQuestion> StoreQuestions { get; set; }

        public System.Data.Entity.DbSet<PetterService.Models.StoreQuestionBoard> StoreQuestionBoards { get; set; }
    }
}
