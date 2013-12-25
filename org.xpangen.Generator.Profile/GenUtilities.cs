// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;

namespace org.xpangen.Generator.Profile
{
    public static class GenUtilities
    {
        /// <summary>
        /// Cust the specified substring out of the given string.
        /// </summary>
        /// <param name="value1">The original string.</param>
        /// <param name="value2">The string being cut out.</param>
        /// <returns>The edited string.</returns>
        public static string CutString(string value1, string value2)
        {
            var i = value1.IndexOf(value2, StringComparison.Ordinal);
            if (i < 0)
                return value1;

            return value1.Substring(0, i) + value1.Substring(i + value2.Length);
        }

        /// <summary>
        /// Tests if the given string is numeric.
        /// </summary>
        /// <param name="value">The string being tested.</param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            if (value == "")
                return false;
            foreach (var c in value)
                if ('0' > c || c > '9')
                    return false;
            return true;
        }

        /// <summary>
        /// Lengthens the shorter string to the same length as the longer one by padding it with leading zeros.
        /// </summary>
        /// <param name="value1">The first number.</param>
        /// <param name="value2">The second number.</param>
        public static void PadShortNumericOperand(ref string value1, ref string value2)
        {
            if (value1.Length < value2.Length)
            {
                var pad = new StringBuilder(value2.Length - value1.Length);
                for (var i = 0; i < value2.Length - value1.Length; i++)
                    pad.Append('0');
                value1 = pad + value1;
            }
            else
            {
                var pad = new StringBuilder(value1.Length - value2.Length);
                for (var i = 0; i < value1.Length - value2.Length; i++)
                    pad.Append('0');
                value2 = pad + value2;
            }
        }

        /// <summary>
        /// Surrounds the given string with single quotes. Embedded quotes are doubled.
        /// </summary>
        /// <param name="value">The string being quoted.</param>
        /// <returns>The quoted string.</returns>
        public static string QuoteString(string value)
        {
            var s = new StringBuilder("'");
            foreach (var c in value)
            {
                switch (c)
                {
                    case '\'':
                        s.Append("''");
                        break;
                    case '\t':
                        s.Append(@"\t");
                        break;
                    case '\r':
                        s.Append(@"\r");
                        break;
                    case '\n':
                        s.Append(@"\n");
                        break;
                    default:
                        s.Append(c);
                        break;
                }
            }
            s.Append("'");
            return s.ToString();
        }

        /// <summary>
        /// Determines if the given string is a valid identifier, and if not to surround it with quotes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringOrName(string value)
        {
            var isName = true;
            foreach (var c in value)
            {
                if ('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z' || '0' <= c && c <= '9' || c == '_') continue;
                isName = false;
                break;
            }
            return isName ? value : QuoteString(value);
        }

        /// <summary>
        /// Turns an identifier into a list of words. Words are identified by capitals (Camel Case), numerics and underscores or hyphens.
        /// </summary>
        /// <param name="value">The identifier being transformed.</param>
        /// <returns>The transformed identfier text.</returns>
        public static string UnIdentifier(string value)
        {
            var s = new StringBuilder();
            foreach (var c in value)
            {
                if (c >= 'a' && c <= 'z')
                    s.Append(c);
                else if (c >= 'A' && c <= 'Z')
                {
                    if (s.Length > 0 && s[s.Length - 1] != ' ')
                        s.Append(' ');
                    s.Append(c);
                }
                else if (c >= '0' && c <= '9')
                {
                    if (s.Length > 0 && !(s[s.Length - 1] >= '0' && s[s.Length - 1] <= '9'))
                        s.Append(' ');
                    s.Append(c);
                }
                else if (c == '_' || c == '-')
                    if (s.Length > 0 && s[s.Length - 1] != ' ')
                        s.Append(' ');
            }
            return s.ToString();
        }
    }
}
