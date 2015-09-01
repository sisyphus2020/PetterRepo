using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PetterService.Common
{
    public class EnumHelper
    {
        public enum UploadType
        {
            [DescriptionAttribute("")]
            All,

            [DescriptionAttribute(@"([^\s]+(\.(?i)(mp2|mp3|m3u))$)")]
            Audio,

            [DescriptionAttribute(@"([^\s]+(\.(?i)(avi|mpg|mpeg))$)")]
            Video,

            [DescriptionAttribute(@"([^\s]+(\.(?i)(doc|docx|txt))$)")]
            Document,

            [DescriptionAttribute(@"([^\s]+(\.(?i)(jpg|png|gif|bmp))$)")]
            Image
        }

        public static string toDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}