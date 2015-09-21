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
            //    new Models.Member() { MemberID = "sisyphus2020@naver.com", Password = "jasper", NickName = "��������", FileName = "abc.gif", FilePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now },
            //    new Models.Member() { MemberID = "jasper.teis@gamil.com", Password = "jasper", NickName = "������", FileName = "abc.gif", FilePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now },
            //    new Models.Member() { MemberID = "sisyphus2020@daum.net", Password = "jasper", NickName = "Google", FileName = "abc.gif", FilePath = "/Member/2015/09/08", Coordinate = DbGeography.FromText("POINT(126.977832 37.5713)"), Latitude = 126.977832, Longitude = 37.5713, DateCreated = DateTime.Now, DateModified = DateTime.Now }

            //);
            //#endregion Member Seed
            //byte[] bytes = Encoding.Default.GetBytes(myString);
            //System.Text.UTF8Encoding encodingUTF8 = new System.Text.UTF8Encoding();
            //System.Text.UnicodeEncoding encodingUNICODE = new System.Text.UnicodeEncoding();

            //#region CommonCode Seed
            //context.CommonCodes.AddOrUpdate(
            //    new Models.CommonCode() { Category = "Dog", Code = "D001", CodeName = encodingUTF8.GetString(encodingUTF8.GetBytes(@"�׷���Ʈ����")) },

            //    new Models.CommonCode() { Category = "Dog", Code = "D002", CodeName = encodingUNICODE.GetString(encodingUNICODE.GetBytes("�׷����Ͽ��")) },

            //    new Models.CommonCode() { Category = "Dog", Code = "D003", CodeName = Encoding.UTF8.GetString(Encoding.Default.GetBytes("�޸�Ƽ��")) },
            //    new Models.CommonCode() { Category = "Dog", Code = "D004", CodeName = "����Ƽ��" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D005", CodeName = "�ҵ���" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D006", CodeName = "����Ʈ������" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D007", CodeName = "���۵�" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D008", CodeName = "�Ƹ޸�ĭ ��Ŀ" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D009", CodeName = "��������" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D010", CodeName = "�ݸ�" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D011", CodeName = "�������ƾ�" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D012", CodeName = "�˰�" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D013", CodeName = "Ǫ��" },
            //    new Models.CommonCode() { Category = "Dog", Code = "D014", CodeName = "AAA" }
            //);
            //#endregion CommonCode Seed

            //#region PetKind Seed
            //context.PetKinds.AddOrUpdate(
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D001", PetName = "�׷���Ʈ����" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D002", PetName = "�׷����Ͽ��" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D003", PetName = "�޸�Ƽ��" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D004", PetName = "����Ƽ��" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D005", PetName = "�ҵ���" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D006", PetName = "����Ʈ������" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D007", PetName = "���۵�" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D008", PetName = "�Ƹ޸�ĭ ��Ŀ" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D009", PetName = "��������" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D010", PetName = "�ݸ�" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D011", PetName = "�������ƾ�" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D012", PetName = "�˰�" },
            //    new Models.PetKind() { PetCategory = "Dog", PetCode = "D013", PetName = "�˰�" }
            //);
            //#endregion PetKind Seed

            //#region Company Seed
            //context.Companies.AddOrUpdate(x => x.CompanyNo,
            //    new Company() { CompanyNo = 17, CompanyName = "������ �����", CompanyAddr = "���� ���ı� ������", StartShopHours = "0930", EndShopHours = "1830", Holiday = "�����", Introduction = "����� �Ǹ��մϴ�.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") },
            //    new Company() { CompanyNo = 18, CompanyName = "����� �����", CompanyAddr = "���� ���α� ���ε�", StartShopHours = "0630", EndShopHours = "1800", Holiday = "�����", Introduction = "����� ��� �Ǹ��մϴ�.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") },
            //    new Company() { CompanyNo = 19, CompanyName = "���� �����", CompanyAddr = "���� ����� ��赿", StartShopHours = "0730", EndShopHours = "1930", Holiday = "�����", Introduction = "������ �Ǹ��մϴ�.", Geo = DbGeography.FromText("POINT(126.9784 37.5667)") }

            //);
            //#endregion Company Seed

            //#region CommonCode Seed
            //context.CommonCodes.AddOrUpdate(
            //    new Models.CommonCode() { Category = "Store", Code = "S001", CodeName = "��̿�" },
            //    new Models.CommonCode() { Category = "Store", Code = "S002", CodeName = "�����" },
            //    new Models.CommonCode() { Category = "Store", Code = "S003", CodeName = "����" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H001", CodeName = "������" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H002", CodeName = "ȭ����" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H003", CodeName = "������" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H004", CodeName = "�����" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H005", CodeName = "�ݿ���" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H006", CodeName = "�����" },
            //    new Models.CommonCode() { Category = "Holiday", Code = "H007", CodeName = "�Ͽ���" },
            //    new Models.CommonCode() { Category = "Service", Code = "S001", CodeName = "����" },
            //    new Models.CommonCode() { Category = "Service", Code = "S002", CodeName = "�ӵ���" },
            //    new Models.CommonCode() { Category = "Service", Code = "S003", CodeName = "�湮�̿�" },
            //    new Models.CommonCode() { Category = "Service", Code = "S004", CodeName = "������" },
            //    new Models.CommonCode() { Category = "Service", Code = "S005", CodeName = "�������" }
            //);
            //#endregion CommonCode Seed

        }
    }
}
