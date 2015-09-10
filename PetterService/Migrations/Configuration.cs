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
            //    new Admin() { AdminNo = 17, WriteId = "Jasper.Teis@gamil.com", Password = "124", WriteName = @"ȫ�浿", DateCreated = DateTime.Now },
            //    new Admin() { AdminNo = 18, WriteId = "ABC@gamil.com", Password = "123", WriteName = @"�̼���", DateCreated = DateTime.Now },
            //    new Admin() { AdminNo = 19, WriteId = "BBB@gamil.com", Password = "456", WriteName = @"������", DateCreated = DateTime.Now }
            //);
            //#endregion Admin Seed

            //#region Member Seed
            //context.Members.AddOrUpdate(x => x.MemberNo,
            //    new Models.Member() { MemberID = "sisyphus2020@naver.com", Password = "jasper", NickName = "��������", PictureName = "abc.gif", PicturePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now },
            //    new Models.Member() { MemberID = "jasper.teis@gamil.com", Password = "jasper", NickName = "������", PictureName = "abc.gif", PicturePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now },
            //    new Models.Member() { MemberID = "sisyphus2020@daum.net", Password = "jasper", NickName = "Google", PictureName = "abc.gif", PicturePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now }

            //);
            //#endregion Member Seed
            //byte[] bytes = Encoding.Default.GetBytes(myString);
            System.Text.UTF8Encoding encodingUTF8 = new System.Text.UTF8Encoding();
            System.Text.UnicodeEncoding encodingUNICODE = new System.Text.UnicodeEncoding();

            #region CommonCode Seed
            context.CommonCodes.AddOrUpdate(
                new Models.CommonCode() { Category = "Dog", Code = "D001", CodeName = encodingUTF8.GetString(encodingUTF8.GetBytes(@"�׷���Ʈ����")) },
                
                new Models.CommonCode() { Category = "Dog", Code = "D002", CodeName = encodingUNICODE.GetString(encodingUNICODE.GetBytes("�׷����Ͽ��")) },

                new Models.CommonCode() { Category = "Dog", Code = "D003", CodeName = Encoding.UTF8.GetString(Encoding.Default.GetBytes("�޸�Ƽ��")) },
                new Models.CommonCode() { Category = "Dog", Code = "D004", CodeName = "����Ƽ��" },
                new Models.CommonCode() { Category = "Dog", Code = "D005", CodeName = "�ҵ���" },
                new Models.CommonCode() { Category = "Dog", Code = "D006", CodeName = "����Ʈ������" },
                new Models.CommonCode() { Category = "Dog", Code = "D007", CodeName = "���۵�" },
                new Models.CommonCode() { Category = "Dog", Code = "D008", CodeName = "�Ƹ޸�ĭ ��Ŀ" },
                new Models.CommonCode() { Category = "Dog", Code = "D009", CodeName = "��������" },
                new Models.CommonCode() { Category = "Dog", Code = "D010", CodeName = "�ݸ�" },
                new Models.CommonCode() { Category = "Dog", Code = "D011", CodeName = "�������ƾ�" },
                new Models.CommonCode() { Category = "Dog", Code = "D012", CodeName = "�˰�" },
                new Models.CommonCode() { Category = "Dog", Code = "D013", CodeName = "Ǫ��" },
                new Models.CommonCode() { Category = "Dog", Code = "D014", CodeName = "AAA" }
            );
            #endregion CommonCode Seed

                //#region Company Seed
                //context.Companies.AddOrUpdate(x => x.CompanyNo,
                //    new Company() { CompanyNo = 17, CompanyName = "������ �����", CompanyAddr = "���� ���ı� ������", StartShopHours = "0930", EndShopHours = "1830", Holiday = "�����", Introduction = "����� �Ǹ��մϴ�.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") },
                //    new Company() { CompanyNo = 18, CompanyName = "����� �����", CompanyAddr = "���� ���α� ���ε�", StartShopHours = "0630", EndShopHours = "1800", Holiday = "�����", Introduction = "����� ��� �Ǹ��մϴ�.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") },
                //    new Company() { CompanyNo = 19, CompanyName = "���� �����", CompanyAddr = "���� ����� ��赿", StartShopHours = "0730", EndShopHours = "1930", Holiday = "�����", Introduction = "������ �Ǹ��մϴ�.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") }

                //);
                //#endregion Company Seed

        }
    }
}
