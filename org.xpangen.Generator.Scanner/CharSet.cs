// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace org.xpangen.Generator.Scanner
{
    /// <summary>
    /// Defines a character set.
    /// </summary>
    public class CharSet
    {
        private readonly int _charCount;
        private char _c0, _c1, _c2, _c3;
        private readonly char[] _ca;
        private readonly char[][] _cra;

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
                        c1 = EscapeCharacter(expression[i]);
                    }
                    x1 += c0;
                    x2 += c1;
                    i++;
                }
                else
                    x0 += c0;
            }

            _ca = x0.ToCharArray();
            _charCount = x0.Length;
            if (_charCount > 0)
                if (_charCount <= 4)
                {
                    _c0 = _ca[0];
                    if (_charCount > 1) _c1 = _ca[1];
                    if (_charCount > 2) _c2 = _ca[2];
                    if (_charCount > 3) _c3 = _ca[3];
                }
            
            _cra = new char[x1.Length][];
            if (x1.Length == 0) return;
            
            var sl = new SortedList<char, char[]>(x1.Length);
            for (var j = 0; j < x1.Length; j++)
                sl.Add(x1[j], new[] {x1[j], x2[j]});
            _cra = sl.Values.ToArray();
        }

        /// <summary>
        /// Checks if the character is in the CharSet
        /// </summary>
        /// <param name="c">The character being matched</param>
        /// <returns>Is the character in the CharSet?</returns>
        public bool Match(char c)
        {
            // The follwing is an optimization of this code
            // i.e. the loop is unrolled, and the contents of the loop moved to scalars.
            //foreach (var x in _ca)
            //    if (x == c) return true;
            switch (_charCount)
            {
                case 0:
                    break;
                case 1:
                    if (c == _c0) return true;
                    break;
                case 2:
                    if (c == _c0 || c == _c1) return true;
                    break;
                case 3:
                    if (c == _c0 || c == _c1 || c == _c2) return true;
                    break;
                case 4:
                    if (c == _c0 || c == _c1 || c == _c2 || c == _c3) return true;
                    break;
                default:
                    foreach (var x in _ca)
                        if (x == c) return true;
                    break;
            }
            
            // The following is an optimization of this code
            //for (var i = 0; i < _cra.Length; i++)
            //{
            //    var range = _cra[i];
            //    if (range[0] > c) continue;
            //    return c <= range[1];
            //}
            //return false;

            var n = _cra.Length;
            if (n == 0) return false;
            
            var i = 0;
            char[] cra;
            do
            {
                cra = _cra[i];
                i++;
            } while (i < n && cra[1] < c);
            return cra[0] <= c && c <= cra[1];
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
