﻿using System;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PetterService.Common
{
    public class Utilities
    {
        public static DateTime DateTimeMinValue
        {
            get
            {
                return new DateTime(1753, 1, 1);
            }
        }

        public static void CreateDirectory(string folder)
        {
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public static byte[] GetMD5Encrypted(string password)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(md5.ComputeHash(encoder.GetBytes(password)));
            byte[] hashedBytes = ms.ToArray();

            return hashedBytes;
        }

        public static string GetSHA1Hash(string password)
        {

            SHA1CryptoServiceProvider oSHA1Hasher =
                       new System.Security.Cryptography.SHA1CryptoServiceProvider();
            Byte[] hashedBytes = null;
            UTF8Encoding encoder = new System.Text.UTF8Encoding();

            hashedBytes = oSHA1Hasher.ComputeHash(encoder.GetBytes(password));

            return Encoding.Default.GetString(hashedBytes);
        }

        public static string Encrypt(string ToEncrypt, bool useHasing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(ToEncrypt);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            string Key = Encription.keyValue;

            if (useHasing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(Key);
            }
            TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tDes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tDes.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

        public static string Decrypt(string cypherString, bool useHasing)
        {
            byte[] keyArray;
            byte[] toDecryptArray = Convert.FromBase64String(cypherString);

            //byte[] toEncryptArray = Convert.FromBase64String(cypherString);
            //System.Configuration.AppSettingsReader settingReader = new AppSettingsReader();

            string key = Encription.keyValue;

            if (useHasing)
            {
                MD5CryptoServiceProvider hashmd = new MD5CryptoServiceProvider();
                keyArray = hashmd.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tDes.CreateDecryptor();

            try
            {
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                tDes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ResizeImage(string fullPath, string thumbnamilName, int width,  int  height , ImageFormat imageFormat)
        {
            //imageFormat.Png
        }

        public static string additionFileName(string fileName)
        {
            //string fullname = headers.ContentDisposition.FileName;
            string fullname = fileName;
            string name = string.Empty;
            string ext = string.Empty;

            fullname = fullname.Replace("\"", string.Empty);
            name = fullname.Substring(0, fullname.LastIndexOf('.'));
            ext = fullname.Substring(fullname.LastIndexOf('.'));

            return name + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
        }
    }
}