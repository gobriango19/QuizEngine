using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizEngine.Utilities.Extensions
{
    public static class StringExtension
    {
        public static IEnumerable<string> SplitString(this string input, int maxLength)
        {
            for (var i = 0; i < input.Length; i += maxLength)
            {
                yield return input.Substring(i, Math.Min(maxLength, input.Length - i));
            }
        }

        public static string UrlEncode(this string input)
        {
            const int maxLength = 32766; // the max length System.Uri.EscapeDataString() can handle

            if (input.Length < maxLength)
                return Uri.EscapeDataString(input);

            var stringBuilder = new StringBuilder();
            var splitInput = input.SplitString(maxLength);
            foreach(var split in splitInput)
            {
                stringBuilder.Append(Uri.EscapeDataString(split));
            }

            return stringBuilder.ToString();
        }
    }
}
