using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaoPhatRainCoat.Utils
{
    public class Helpers
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetUserToken()
        {
            return (string)HttpContext.Current.Session[Constants.SESSION_USER_INFO];
        }
        public static void SetUserToken(string token)
        {
            HttpContext.Current.Session[Constants.SESSION_USER_INFO] = token;
        }
    }
}