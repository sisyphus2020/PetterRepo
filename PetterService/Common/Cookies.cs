using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Common
{
    public class Cookies
    {
        public static void SetUserName(string userName)
        {
            HttpContext.Current.Response.Cookies["userName"].Value = userName;
            HttpContext.Current.Response.Cookies["userName"].Expires = DateTime.Now.AddDays(1);
        }

        public static object GetSessionValue(string name)
        {
            return HttpContext.Current.Session[name];
        }

        public static void SetSessionValue(string name, object value)
        {
            HttpContext.Current.Session[name] = value;
        }
    }
}