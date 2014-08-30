// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    /// <summary>
    ///     An abstract definition of the profile fragment syntax.
    /// </summary>
    public abstract class ProfileFragmentSyntaxDictionary : Dictionary<string, ProfileFragmentSyntax>
    {
        /// <summary>
        ///     Parses the generator condition for this syntax.
        /// </summary>
        /// <param name="genDataDef">The generator data definition.</param>
        /// <param name="condition">The condition being parsed.</param>
        public abstract ConditionParameters ParseCondition(GenDataDef genDataDef, string condition);

        /// <summary>
        ///     Text for each item in the <see cref="GenCardinality" /> enumeration.
        /// </summary>
        public string[] GenCardinalityText;

        /// <summary>
        ///     Text for each item in the <see cref="GenComparison" /> enumeration.
        /// </summary>
        public string[] GenComparisonText;

        /// <summary>
        ///     The currently active <see cref="ProfileFragmentSyntaxDictionary" />. By default this is
        ///     <see
        ///         cref="CompactProfileFragmentSyntaxDictionary" />
        ///     .
        /// </summary>
        public static ProfileFragmentSyntaxDictionary ActiveProfileFragmentSyntaxDictionary
        {
            get
            {
                return _activeProfileFragmentSyntaxDictionary ??
                       (_activeProfileFragmentSyntaxDictionary = new CompactProfileFragmentSyntaxDictionary());
            }
        }

        public string FunctionParameterSeparator { get; protected set; }

        protected string Dlm;
        private static ProfileFragmentSyntaxDictionary _activeProfileFragmentSyntaxDictionary;

        /// <summary>
        ///     Get the operand from the beginning of the string being parsed.
        /// </summary>
        /// <param name="s">The string being parsed. The operand is removed from the string.</param>
        /// <returns>The parsed operand.</returns>
        protected string GetOperand(ref string s)
        {
            var i = 0;
            while (i < s.Length && !IsOperator(s[i])) i++;
            if (i >= s.Length)
                return s;
            var t = s.Substring(0, i);
            s = s.Substring(i);
            return t;
        }

        private bool IsOperator(char x)
        {
            foreach (var c in Dlm)
                if (Equals(c, x))
                    return true;
            return false;
        }

        /// <summary>
        ///     Get the comparison operator from the beginning of the string being parsed.
        /// </summary>
        /// <param name="s">The string being parsed. The operator is removed from the string.</param>
        /// <returns>The parsed operator.</returns>
        protected GenComparison GetOperator(ref string s)
        {
            if (0 < s.Length)
            {
                var n = 0;
                for (var j = 0; j <= GenComparisonText.GetUpperBound(0); j++)
                {
                    var length = GenComparisonText[j].Length;
                    if (n < length) n = length;
                }

                for (var i = n; i > 0; i--)
                {
                    for (var j = 0; j <= GenComparisonText.GetUpperBound(0); j++)
                    {
                        var length = GenComparisonText[j].Length;
                        if (length == i && s.Length >= length)
                        {
                            var t = s.Substring(0, length);
                            if (GenComparisonText[j] == t)
                            {
                                s = s.Substring(length);
                                return (GenComparison) j;
                            }
                        }
                    }
                }
            }
            return GenComparison.Exists;
        }

        /// <summary>
        ///     Parses the segment class text and returns an empty segment profile fragment
        /// </summary>
        /// <param name="genDataDef">The generator data definition.</param>
        /// <param name="segmentClass">The segment class text to be parsed.</param>
        /// <param name="parentSegment"></param>
        /// <param name="parentContainer"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        public abstract GenSegment ParseSegmentHeading(GenDataDef genDataDef, string segmentClass, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, bool isPrimary);
    }
}