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
    }
}