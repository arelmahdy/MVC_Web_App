using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Web_App.Data
{
    public static class AppHash
    {
        
        public static string HashPassword(string input)
        {
            string ps = string.Empty;
            MD5 hash = MD5.Create();
            byte[] data = hash.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder st = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                st.Append(data[i].ToString("x2"));

            }
            ps = st.ToString();
            return ps;

        }
        public static string StringToBase64(string text)
        {
            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(textBytes);
        }
        public static string Base46ToString(string base64Text)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64Text);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
