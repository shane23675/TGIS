using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace TGIS.Models
{
    public static class Hash
    {
        public static string PwdHash(string pwd)
        {
            SHA256 sHA256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(pwd);
            byte[] crypto = sHA256.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);

            return result;
        }
    }
}