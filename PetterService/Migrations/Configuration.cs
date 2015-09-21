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
            //    new Models.Member() { MemberID = "sisyphus2020@naver.com", Password = "jasper", NickName = "시지프스", FileName = "abc.gif", FilePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now },
            //    new Models.Member() { MemberID = "jasper.teis@gamil.com", Password = "jasper", NickName = "제스퍼", FileName = "abc.gif", FilePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now },
            //    new Models.Member() { MemberID = "sisyphus2020@daum.net", Password = "jasper", NickName = "Google", FileName = "abc.gif", FilePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now }

            //);
            //#endregion Member Seed
            //byte[] bytes = Encoding.Default.GetBytes(myString);
            //System.Text.UTF8Encoding encodingUTF8 = new System.Text.UTF8Encoding();
            //System.Text.UnicodeEncoding encodingUNICODE = new System.Text.UnicodeEncoding();

            //#region CommonCode Seed
            //context.CommonCodes.AddOrUpdate(
            //    new Models.CommonCode() { Category = "Dog", Code = "D001", CodeName = encodingUTF8.GetString(encodingUTF8.GetBytes(@"그레이트데인")) },

            //    new Models.CommonCode() { Category = "Dog", Code = "D002", CodeName = encodingUNICODE.GetString(encodingUNICODE.GetBytes("그레이하운드")) },

            //    new Models.CommonCode() { Category = "Dog", Code = "D003", CodeName = Encoding.UTF8.GetString(Encoding.Default.GetBytes("달마티안")) },
            //    new Models.CommonCode() { Category = "Dog", Code = "D004", CodeName = "마르티즈" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D005", CodeName = "불도그" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D006", CodeName = "세인트버나드" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D007", CodeName = "세퍼드" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D008", CodeName = "아메리칸 코커" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D009", CodeName = "차우차우" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D010", CodeName = "콜리" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D011", CodeName = "포예리아안" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D012", CodeName = "똥개" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D013", CodeName = "푸들" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D014", CodeName = "AAA" }
            //);
            //#endregion CommonCode Seed

            //#region PetKind Seed
            //context.PetKinds.AddOrUpdate(
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D001", PetName = "그레이트데인" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D002", PetName = "그레이하운드" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D003", PetName = "달마티안" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D004", PetName = "마르티즈" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D005", PetName = "불도그" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D006", PetName = "세인트버나드" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D007", PetName = "세퍼드" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D008", PetName = "아메리칸 코커" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D009", PetName = "차우차우" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D010", PetName = "콜리" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D011", PetName = "포예리아안" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D012", PetName = "똥개" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D013", PetName = "똥개" }
            //);
            //#endregion PetKind Seed

            //#region Company Seed
            //context.Companies.AddOrUpdate(x => x.CompanyNo,
            //    new Company() { CompanyNo = 17, CompanyName = "강아지 대통령", CompanyAddr = "서울 송파구 장지동", StartShopHours = "0930", EndShopHours = "1830", Holiday = "토요일", Introduction = "개사료 판매합니다.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") },
            //    new Company() { CompanyNo = 18, CompanyName = "고양이 대통령", CompanyAddr = "서울 구로구 구로동", StartShopHours = "0630", EndShopHours = "1800", Holiday = "토요일", Introduction = "고양이 사료 판매합니다.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") },
            //    new Company() { CompanyNo = 19, CompanyName = "돼지 대통령", CompanyAddr = "서울 노원구 사계동", StartShopHours = "0730", EndShopHours = "1930", Holiday = "토요일", Introduction = "돼지료 판매합니다.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") }

            //);
            //#endregion Company Seed

            //#region CommonCode Seed
            //context.CommonCodes.AddOrUpdate(
            //    new Models.CommonCode() { Category = "Store", Code = "S001", CodeName = "펫미용" },
            //    new Models.CommonCode() { Category = "Store", Code = "S002", CodeName = "펫시터" },
            //    new Models.CommonCode() { Category = "Store", Code = "S003", CodeName = "숙소" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H001", CodeName = "월요일" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H002", CodeName = "화요일" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H003", CodeName = "수요일" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H004", CodeName = "목요일" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H005", CodeName = "금요일" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H006", CodeName = "토요일" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H007", CodeName = "일요일" },
            //    new Models.CommonCode() { Category = "Service", Code = "S001", CodeName = "스파" },
            //    new Models.CommonCode() { Category = "Service", Code = "S002", CodeName = "머드팩" },
            //    new Models.CommonCode() { Category = "Service", Code = "S003", CodeName = "방문미용" },
            //    new Models.CommonCode() { Category = "Service", Code = "S004", CodeName = "대형견" },
            //    new Models.CommonCode() { Category = "Service", Code = "S005", CodeName = "셀프목욕" }
            //);
            //#endregion CommonCode Seed

        }
    }
}
