using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PetterService.Common
{
    public class UrlEncoder
    {
        public string URLDecode(string decode)
        {
            if (decode == null) return null;
            if (decode.StartsWith("="))
            {
                return FromBase64(decode.TrimStart('='));
            }
            else
            {
                return HttpUtility.UrlDecode(decode);
            }
        }

        public string UrlEncode(string encode)
        {
            if (encode == null) return null;
            string encoded = HttpUtility.UrlPathEncode(encode);
            if (encoded.Replace("%20", "") == encode.Replace(" ", ""))
            {
                return encoded;
            }
            else
            {
                return "=" + ToBase64(encode);
            }
        }

        public string ToBase64(string encode)
        {
            Byte[] btByteArray = null;
            UTF8Encoding encoding = new UTF8Encoding();
            btByteArray = encoding.GetBytes(encode);
            string sResult = System.Convert.ToBase64String(btByteArray, 0, btByteArray.Length);
            sResult = sResult.Replace("+", "-").Replace("/", "_");
            return sResult;
        }

        public string FromBase64(string decode)
        {
            decode = decode.Replace("-", "+").Replace("_", "/");
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(Convert.FromBase64String(decode));
        }
    }
}