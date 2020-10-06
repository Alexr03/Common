using System.Text;
using System.Text.RegularExpressions;

namespace Alexr03.Common.Misc.Strings
{
    public static class StringExtensions
    {
        public static string SplitEveryCapital(this string s)
        {
            var regex = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return regex.Replace(s, " ");
        }

        public static string Base64Encode(this string s)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(s);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string s)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(s);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        
        public static string Md5Encode(this string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}