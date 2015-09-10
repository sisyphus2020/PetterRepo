namespace PetterService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Text;
    using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<PetterService.Models.PetterServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PetterService.Models.PetterServiceContext context)
        {
            //#region Admin Seed
            //context.Admins.AddOrUpdate(x => x.AdminNo,
            //    new Admin() { AdminNo = 17, WriteId = "Jasper.Teis@gamil.com", Password = "124", WriteName = @"홍길동", DateCreated = DateTime.Now },
            //    new Admin() { AdminNo = 18, WriteId = "ABC@gamil.com", Password = "123", WriteName = @"이순신", DateCreated = DateTime.Now },
            //    new Admin() { AdminNo = 19, WriteId = "BBB@gamil.com", Password = "456", WriteName = @"유성룡", DateCreated = DateTime.Now }
            //);
            //#endregion Admin Seed

            //#region Member Seed
            //context.Members.AddOrUpdate(x => x.MemberNo,
            //    new Models.Member() { MemberID = "sisyphus2020@naver.com", Password = "jasper", NickName = "시지프스", PictureName = "abc.gif", PicturePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now },
            //    new Models.Member() { MemberID = "jasper.teis@gamil.com", Password = "jasper", NickName = "제스퍼", PictureName = "abc.gif", PicturePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now },
            //    new Models.Member() { MemberID = "sisyphus2020@daum.net", Password = "jasper", NickName = "Google", PictureName = "abc.gif", PicturePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now }

            //);
            //#endregion Member Seed
            //byte[] bytes = Encoding.Default.GetBytes(myString);
            System.Text.UTF8Encoding encodingUTF8 = new System.Text.UTF8Encoding();
            System.Text.UnicodeEncoding encodingUNICODE = new System.Text.UnicodeEncoding();

            #region CommonCode Seed
            context.CommonCodes.AddOrUpdate(
                new Models.CommonCode() { Category = "Dog", Code = "D001", CodeName = encodingUTF8.GetString(encodingUTF8.GetBytes(@"그레이트데인")) },
                
                new Models.CommonCode() { Category = "Dog", Code = "D002", CodeName = encodingUNICODE.GetString(encodingUNICODE.GetBytes("그레이하운드")) },

                new Models.CommonCode() { Category = "Dog", Code = "D003", CodeName = Encoding.UTF8.GetString(Encoding.Default.GetBytes("달마티안")) },
                new Models.CommonCode() { Category = "Dog", Code = "D004", CodeName = "마르티즈" },
                new Models.CommonCode() { Category = "Dog", Code = "D005", CodeName = "불도그" },
                new Models.CommonCode() { Category = "Dog", Code = "D006", CodeName = "세인트버나드" },
                new Models.CommonCode() { Category = "Dog", Code = "D007", CodeName = "세퍼드" },
                new Models.CommonCode() { Category = "Dog", Code = "D008", CodeName = "아메리칸 코커" },
                new Models.CommonCode() { Category = "Dog", Code = "D009", CodeName = "차우차우" },
                new Models.CommonCode() { Category = "Dog", Code = "D010", CodeName = "콜리" },
                new Models.CommonCode() { Category = "Dog", Code = "D011", CodeName = "포예리아안" },
                new Models.CommonCode() { Category = "Dog", Code = "D012", CodeName = "똥개" },
                new Models.CommonCode() { Category = "Dog", Code = "D013", CodeName = "푸들" },
                new Models.CommonCode() { Category = "Dog", Code = "D014", CodeName = "AAA" }
            );
            #endregion CommonCode Seed

                //#region Company Seed
                //context.Companies.AddOrUpdate(x => x.CompanyNo,
                //    new Company() { CompanyNo = 17, CompanyName = "강아지 대통령", CompanyAddr = "서울 송파구 장지동", StartShopHours = "0930", EndShopHours = "1830", Holiday = "토요일", Introduction = "개사료 판매합니다.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") },
                //    new Company() { CompanyNo = 18, CompanyName = "고양이 대통령", CompanyAddr = "서울 구로구 구로동", StartShopHours = "0630", EndShopHours = "1800", Holiday = "토요일", Introduction = "고양이 사료 판매합니다.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") },
                //    new Company() { CompanyNo = 19, CompanyName = "돼지 대통령", CompanyAddr = "서울 노원구 사계동", StartShopHours = "0730", EndShopHours = "1930", Holiday = "토요일", Introduction = "돼지료 판매합니다.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") }

                //);
                //#endregion Company Seed

        }
    }
}
