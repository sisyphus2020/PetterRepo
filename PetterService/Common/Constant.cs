using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Common
{
    public class Constant
    {
    }

    public static class UploadPath
    {
        public static string Root { get { return HttpContext.Current.Server.MapPath("~/") + "\\"; } }
        public static string Temp { get { return Root + "Temp\\"; } }
        public static string PATH { get { return "~/App_Data" + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd"); } }
        public static string BeautyShopPath { get { return "/Files" + "/" + "BeautyShop" + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd"); } }
        public static string PetSitterPath { get { return "/Files" + "/" + "PetSitter" + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd"); } }
        public static string PensionPath { get { return "/Files" + "/" + "Pension" + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd"); } }
    }

    public sealed class Encription
    {
        public const string keyValue = "Teis";
    }

    public sealed class PetKind
    {
        public const int CAT = 1;
        public const int DOG = 2;

    }

    public sealed class FileSize
    {
        public const int BeautyShopWidth = 980;
        public const int BeautyShopHeight = 360;

        public const int PetSitterWidth = 980;
        public const int PetSitterHeight = 360;

        public const int PensionWidth = 980;
        public const int PensionHeight = 360;

    }

    public static class FileExtension
    {
        public static string[] BeautyShopExtensions = { ".jpg", ".jpeg", ".gif", ".bmp", ".png" };
        public static string[] PetSitterExtensions = { ".jpg", ".jpeg", ".gif", ".bmp", ".png" };
        public static string[] PensionExtensions = { ".jpg", ".jpeg", ".gif", ".bmp", ".png" };
    }

    public static class ErrorMessage
    {
        public static readonly string FileTypeError = "첨부파일 형식이 맞지 않습니다.";
    }
}