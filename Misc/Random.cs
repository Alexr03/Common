using System.Linq;

namespace Alexr03.Common.Misc
{
    public static class Random
    {
        private static readonly System.Random SystemRandom = new System.Random();
        private const string UpperCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowerCharacters = "abcdefghijklmnopqrstuvwxyz";
        private const string Symbols = @"\/-_=+[{]};:'@#~,<.>/?!£$%^&*()";
        private const string Numbers = "0123456789";

        public static string RandomString(int length, bool includeLowerCase = true, bool includeUpperCase = true, bool includeNumbers = false, bool includeSymbols = false)
        {
            var dataSet = GetDataSet(includeLowerCase, includeUpperCase, includeNumbers, includeSymbols);
            return new string(Enumerable.Repeat(dataSet, length)
                .Select(s => s[SystemRandom.Next(s.Length)]).ToArray());
        }

        private static string GetDataSet(bool includeLowerCase = true, bool includeUpperCase = true, bool includeNumbers = false, bool includeSymbols = false)
        {
            var dataSet = "";
            if (includeLowerCase)
            {
                dataSet += LowerCharacters;
            }
            if (includeUpperCase)
            {
                dataSet += UpperCharacters;
            }
            if (includeNumbers)
            {
                dataSet += Numbers;
            }
            if (includeSymbols)
            {
                dataSet += Symbols;
            }

            return dataSet;
        }
    }
}