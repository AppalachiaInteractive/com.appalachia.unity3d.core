#region

using System;
using System.Globalization;
using System.Text;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comparisonType)
        {
            return source.IndexOf(toCheck, comparisonType) >= 0;
        }

        public static bool Contains(this string str, char ch)
        {
            if (str == null)
            {
                return false;
            }

            return str.IndexOf(ch) != -1;
        }

        public static bool IsNullOrWhitespace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (var index = 0; index < str.Length; ++index)
                {
                    if (!char.IsWhiteSpace(str[index]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static string Cut(this string s, int chars)
        {
            var length = Mathf.Clamp(s.Length - chars, 0, s.Length);

            return s.Substring(0, length);
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

        public static string SplitPascalCase(this string input)
        {
            switch (input)
            {
                case "":
                case null:
                    return input;
                default:
                    var stringBuilder = new StringBuilder(input.Length);
                    if (char.IsLetter(input[0]))
                    {
                        stringBuilder.Append(char.ToUpper(input[0]));
                    }
                    else
                    {
                        stringBuilder.Append(input[0]);
                    }

                    for (var index = 1; index < input.Length; ++index)
                    {
                        var c = input[index];
                        if (char.IsUpper(c) && !char.IsUpper(input[index - 1]))
                        {
                            stringBuilder.Append(' ');
                        }

                        stringBuilder.Append(c);
                    }

                    return stringBuilder.ToString();
            }
        }

        public static string ToTitleCase(this string input)
        {
            var stringBuilder = new StringBuilder();
            for (var index = 0; index < input.Length; ++index)
            {
                var ch = input[index];
                if ((ch == '_') && ((index + 1) < input.Length))
                {
                    var upper = input[index + 1];
                    if (char.IsLower(upper))
                    {
                        upper = char.ToUpper(upper, CultureInfo.InvariantCulture);
                    }

                    stringBuilder.Append(upper);
                    ++index;
                }
                else
                {
                    stringBuilder.Append(ch);
                }
            }

            return stringBuilder.ToString();
        }

        public static StringBuilder Cut(this StringBuilder s, int chars)
        {
            var length = s.Length;
            var targetLength = length - chars;

            targetLength = Mathf.Clamp(targetLength, 0, length);

            return s.Remove(targetLength - 1, chars);
        }

        public static void CopyToClipboard(this string s)
        {
            var te = new TextEditor {text = s};
            te.SelectAll();
            te.Copy();
        }
    }
}
