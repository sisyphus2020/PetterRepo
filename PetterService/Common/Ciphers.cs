using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Common
{
    public class Ciphers
    {
        public string getCryptPassword(string password)
        {
            string hashPasword = password + CryptKey.PetterSalt;
            string petterSalt = BCrypt.GenerateSalt();
            string hashCode = BCrypt.HashPassword(hashPasword, petterSalt);

            return hashCode;
        }

        public bool getPasswordMatch(string hashPassword, string password)
        {
            return BCrypt.CheckPassword(password + CryptKey.PetterSalt, hashPassword);
        }

    }
}