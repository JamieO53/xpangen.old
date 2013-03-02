// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Scanner
{
    /// <summary>
    /// Defines a character set.
    /// </summary>
    public class CharSet
    {
        private readonly char[] _ca;
        private readonly char[,] _cra;

        /// <summary>
        /// The characters being recognized.
        /// 
        /// Only the whitespace escape characters (/t, /r, /n) and // for / and /- for - are accepted.
        /// </summary>
        /// <param name="expression">Corresponds the the contents of a RegEx bracket expression.</param>
        public CharSet(string expression)
        {
            var x0 = "";
            var x1 = "";
            var x2 = "";

            var i = 0;
            var n = expression.Length;
            while (i < n)
            {
                var c0 = expression[i];
                if (c0 == '\\' && i + 1 < n)
                {
                    i++;
                    c0 = EscapeCharacter(expression[i]);
                }
                i++;
                if (i+1 < n && expression[i] == '-')
                {
                    i++;
                    var c1 = expression[i];
                    if (c1 == '\\' && i + 1 < n)
                    {
                        i++;
                        c1 = EscapeCharacter(c1);
                    }
                    x1 += c0;
                    x2 += c1;
                }
                else
                    x0 += c0;
            }

            _ca = x0.ToCharArray();
            _cra = new char[x1.Length, 2];
            for (var j = 0; j < x1.Length; j++)
            {
                _cra[j, 0] = x1[j];
                _cra[j, 1] = x2[j];
            }
        }

        /// <summary>
        /// Checks if the character is in the CharSet
        /// </summary>
        /// <param name="c">The character being matched</param>
        /// <returns>Is the character in the CharSet?</returns>
        public bool Match(char c)
        {
            foreach (char x in _ca)
                if (x == c) return true;
            for (var i = 0; i <= _cra.GetUpperBound(0); i++)
                if (c >= _cra[i, 0] && c <= _cra[i, 1])
                    return true;
            return false;
        }

        /// <summary>
        /// Replace the character with its escaped value.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static char EscapeCharacter(char c)
        {
            switch (c)
            {
                case 'r':
                    c = '\r';
                    break;
                case 'n':
                    c = '\n';
                    break;
                case 't':
                    c = '\t';
                    break;
            }
            return c;
        }
    }
}
