﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Linq;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    /// <summary>
    ///     Contains specific syntax data and parsers for the compact profile fragment syntax.
    /// </summary>
    public class CompactProfileFragmentSyntaxDictionary : ProfileFragmentSyntaxDictionary
    {
        /// <summary>
        ///     Create a new <see cref="ProfileFragmentSyntaxDictionary" /> object for the compact syntax.
        /// </summary>
        public CompactProfileFragmentSyntaxDictionary()
        {
            GenCardinalityText = new[] {">", "/", "<", "\\", "'", "+", ".", "-", "@", "^"};
            GenComparisonText = new[] {"", "~", "=", "<>", "<", "<=", ">", ">="};
            Dlm = "=<>~";
            FunctionParameterSeparator = " ";

            Add(new ProfileFragmentSyntax
                    {
                        FragmentType = FragmentType.Block,
                        Format = "`{{{0}`]"
                    });
            Add(new ProfileFragmentSyntax
                    {
                        FragmentType = FragmentType.Condition,
                        Format = "`?{0}{1}{2}:{3}`]"
                    });
            Add(new ProfileFragmentSyntax
                    {
                        FragmentType = FragmentType.Function,
                        Format = "`@{0}:{1}`]"
                    });
            Add(new ProfileFragmentSyntax
                    {
                        FragmentType = FragmentType.Lookup,
                        Variant = 1, // Look up existing data
                        Format = "`%{0}={1}:{2}`]"
                    });
            Add(new ProfileFragmentSyntax
                    {
                        FragmentType = FragmentType.Lookup,
                        Variant = 2, // Look up non-existent data
                        Format = "`%{0}={1}:{2}`;{3}`]"
                    });
            Add(new ProfileFragmentSyntax
                    {
                        FragmentType = FragmentType.Placeholder,
                        Format = "`{0}.{1}`"
                    });
            Add(new ProfileFragmentSyntax
                    {
                        FragmentType = FragmentType.Segment,
                        Variant = 1, // Without delimiter
                        Format = "`[{0}{2}:{1}`]"
                    });
            Add(new ProfileFragmentSyntax
            {
                FragmentType = FragmentType.Segment,
                Variant = 2, // With separator
                Format = "`[{0}{2}:{1}`;{3}`]"
            });
            Add(new ProfileFragmentSyntax
                    {
                        FragmentType = FragmentType.TextBlock,
                        Format = "{0}"
                    });
            Add(new ProfileFragmentSyntax
            {
                FragmentType = FragmentType.Annotation,
                Format = "`-{0}`]"
            });
        }

        private void Add(ProfileFragmentSyntax item)
        {
            Add(item.Key, item);
        }

        /// <summary>
        ///     Parses the generator condition for this syntax.
        /// </summary>
        /// <param name="genDataDef">The generator data definition.</param>
        /// <param name="condition">The condition being parsed.</param>
        public override ConditionParameters ParseCondition(GenDataDef genDataDef, string condition)
        {
            var genCondition = new ConditionParameters();
            var s = condition.Trim();
            var i = s.ToLowerInvariant().IndexOf(" exists", StringComparison.Ordinal);
            if (i > 0)
            {
                var v1 = s.Substring(0, i);
                genCondition.Var1 = genDataDef.GetId(v1);
                genCondition.GenComparison = GenComparison.Exists;
            }
            else
            {
                var v1 = GetOperand(ref s);
                if (v1 != "" && s != "")
                {
                    genCondition.Var1 = genDataDef.GetId(v1, true);
                    genCondition.GenComparison = GetOperator(ref s);

                    if (genCondition.GenComparison != GenComparison.NotExists)
                    {
                        var v2 = s;
                        genCondition.UseLit = v2[0] == '\'' || (v2[0] >= '0' && v2[0] <= '9') || !v2.ToCharArray().Contains('.');

                        if (!genCondition.UseLit)
                            try
                            {
                                genCondition.Var2 = genDataDef.GetId(v2);
                            }
                            catch (Exception)
                            {
                                genCondition.Var2 = new GenDataId {ClassId = -1, PropertyId = -1};
                            }
                        if (genCondition.UseLit || genCondition.Var2.ClassId == -1 || genCondition.Var2.PropertyId == -1)
                        {
                            if (v2[0] == '\'')
                                if (v2[v2.Length - 1] == '\'')
                                    genCondition.Lit = v2.Substring(1, v2.Length - 2);
                                else
                                    throw new Exception("<<<<Missing closing quote in condition value>>>>");
                            else
                            {
                                genCondition.Lit = v2;
                                genCondition.UseLit = true;
                            }
                        }
                    }
                }
                else
                {
                    genCondition.Var1 = genDataDef.GetId(v1, true);
                    genCondition.GenComparison = GenComparison.Exists;
                }
            }
            return genCondition;
        }

        public override GenSegment ParseSegmentHeading(GenDataDef genDataDef, string segmentClass, GenContainerFragmentBase parentContainer, bool isPrimary)
        {
            var s = segmentClass.Substring(segmentClass.Length - 1);
            var c = segmentClass;
            var cardinality = GenCardinality.All;
            for (var i = 0; i < ActiveProfileFragmentSyntaxDictionary.GenCardinalityText.Length; i++)
            {
                var x = ActiveProfileFragmentSyntaxDictionary.GenCardinalityText[i];
                if (s == x)
                {
                    cardinality = (GenCardinality) i;
                    c = segmentClass.Substring(0, segmentClass.Length - 1);
                    break;
                }
            }
            return new GenSegment(new GenSegmentParams(genDataDef, parentContainer, c, cardinality, isPrimary: isPrimary));
        }
    }
}