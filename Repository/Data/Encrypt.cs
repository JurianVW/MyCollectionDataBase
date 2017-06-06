using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Repository.Data
{
    public class Encrypt
    {
        public string EncryptPassword(string password)
        {
            string saltString = "MCDB";
            MD5 algorithm = MD5.Create();
            byte[] plainText = Encoding.ASCII.GetBytes(password);
            byte[] salt = Encoding.ASCII.GetBytes(saltString);
            byte[] plainTextWithSalt = new byte[plainText.Length + salt.Length];
            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSalt[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSalt[plainText.Length + i] = salt[i];
            }
            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSalt));
        }
    }
}