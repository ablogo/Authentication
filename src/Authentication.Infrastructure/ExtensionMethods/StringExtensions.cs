using System;
using System.Text;

namespace Authentication.Infrastructure.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string StringToBase64(this string stringToBase64)
        {
            try
            {
                if (!string.IsNullOrEmpty(stringToBase64))
                {
                    var textBytes = Encoding.UTF8.GetBytes(stringToBase64);
                    return Convert.ToBase64String(textBytes);
                }
            }
            catch (Exception) { throw; }
            return "";
        }

        public static string Base64ToString(this string base64String)
        {
            try
            {
                if (!string.IsNullOrEmpty(base64String))
                {
                    var textBytes = Convert.FromBase64String(base64String);
                    return Encoding.UTF8.GetString(textBytes);
                }
            }
            catch (Exception) { throw; }
            return "";
        }

    }
}
