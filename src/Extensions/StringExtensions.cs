#region

using System.Text;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class StringExtensions
    {
        // This needs to be added to a public static class to be used like an extension
        public static void CopyToClipboard(this string s)
        {
            var te = new TextEditor {text = s};
            te.SelectAll();
            te.Copy();
        }

        public static string Cut(this string s, int chars)
        {
            var length = Mathf.Clamp(s.Length - chars, 0, s.Length);

            return s.Substring(0, length);
        }

        public static StringBuilder Cut(this StringBuilder s, int chars)
        {
            var length = s.Length;
            var targetLength = length - chars;

            targetLength = Mathf.Clamp(targetLength, 0, length);

            return s.Remove(targetLength - 1, chars);
        }

        public static string SeperateWords(this string value)
        {
            var caps = 0;
            for (var i = 1; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    caps += 1;
                }
            }

            if (caps == 0)
            {
                return value;
            }

            var chars = new char[value.Length + caps];

            var outIndex = 0;

            for (var i = 0; i < value.Length; i++)
            {
                var character = value[i];

                if ((i > 0) && char.IsUpper(character))
                {
                    chars[outIndex] = ' ';
                    outIndex += 1;
                }

                chars[outIndex] = character;
                outIndex += 1;
            }

            return new string(chars);
        }
    }
}
