// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenProfileTextExpander
    {
        private ProfileFragmentSyntaxDictionary Dictionary { get; set; }

        public GenProfileTextExpander(ProfileFragmentSyntaxDictionary dictionary)
        {
            Dictionary = dictionary;
        }

        private static FragmentType GetFragmentType(Fragment fragment)
        {
            var fragmentTypeName = fragment.GetType().Name;
            FragmentType fragmentType;
            if (!Enum.TryParse(fragmentTypeName, out fragmentType))
                throw new GeneratorException("Unknown fragment type: " + fragmentTypeName, GenErrorType.Assertion);
            return fragmentType;
        }

        private GenWriter Writer { get; set; }

        public string GetText(Fragment fragment)
        {
            var saveWriter = Writer;
            using (var stream = new MemoryStream(100000))
            {
                using (var writer = new GenWriter(stream))
                {
                    Writer = writer;
                    OutputFragment(fragment);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    Writer = saveWriter;
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
        }

        private void OutputText(string text)
        {
            Writer.Write(text);
        }

        private string GetBodyText(GenProfileTextExpander pt, FragmentBody body)
        {
            var saveWriter = Writer;
            using (var stream = new MemoryStream(100000))
            {
                using (var writer = new GenWriter(stream))
                {
                    pt.Writer = writer;
                    pt.OutputBody(body);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    pt.Writer = saveWriter;
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
        }

        private void OutputBody(FragmentBody body)
        {
            foreach (var f in body.FragmentList)
                OutputFragment(f);
        }

        private void OutputFragment(Fragment fragment)
        {
            var fragmentType = GetFragmentType(fragment);
            string format;
            switch (fragmentType)
            {
                case FragmentType.Null:
                    break;
                case FragmentType.Text:
                    OutputText(((Text) fragment).TextValue);
                    break;
                case FragmentType.Placeholder:
                    format = Dictionary[fragmentType.ToString()].Format;
                    var placeholderFragment = (Placeholder) fragment;
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         placeholderFragment.Class,
                                                         placeholderFragment.Property
                                                     }
                        ));
                    break;
                case FragmentType.Segment:
                    var segmentFragment = (Segment) fragment;
                    GenCardinality cardinality;
                    if (!Enum.TryParse(segmentFragment.Cardinality, out cardinality))
                        throw new GeneratorException("Invalid segment cardinality: " + segmentFragment.Cardinality,
                            GenErrorType.Assertion);
                    var variant = (cardinality == GenCardinality.AllDlm || cardinality == GenCardinality.BackDlm
                        ? "2"
                        : "1");
                    format = Dictionary[fragmentType + variant].Format;
                    if (variant == "1")
                        OutputText(string.Format(format, new object[]
                                                         {
                                                             segmentFragment.Class,
                                                             GetBodyText(segmentFragment.Body()),
                                                             Dictionary.GenCardinalityText[(int) cardinality]
                                                         }
                            ));
                    else
                        OutputText(string.Format(format, new object[]
                                                         {
                                                             segmentFragment.Class,
                                                             GetBodyText(segmentFragment.Body()),
                                                             Dictionary.GenCardinalityText[(int) cardinality],
                                                             GetBodyText(segmentFragment.SecondaryBody())
                                                         }
                            ));
                    break;
                case FragmentType.Profile:
                    OutputBody(((Profile.Profile) fragment).Body());
                    break;
                case FragmentType.Block:
                    format = Dictionary[fragmentType.ToString()].Format;
                    var blockFragment = (Block) fragment;
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         GetBodyText(blockFragment.Body())
                                                     }
                        ));
                    break;
                case FragmentType.Lookup:
                    var lookupFragment = (Lookup) fragment;
                    var noMatch = lookupFragment.SecondaryBody().FragmentList.Count > 0;
                    format = Dictionary[fragmentType + (noMatch ? "2" : "1")].Format;
                    if (!noMatch)
                        OutputText(string.Format(format, new object[]
                                                         {
                                                             Identifier(lookupFragment.Class1, lookupFragment.Property1),
                                                             Identifier(lookupFragment.Class2, lookupFragment.Property2),
                                                             GetBodyText(lookupFragment.Body())
                                                         }
                            ));
                    else
                        OutputText(string.Format(format, new object[]
                                                         {
                                                             Identifier(lookupFragment.Class1, lookupFragment.Property1),
                                                             Identifier(lookupFragment.Class2, lookupFragment.Property2),
                                                             GetBodyText(lookupFragment.Body()),
                                                             GetBodyText(lookupFragment.SecondaryBody())
                                                         }
                            ));
                    break;
                case FragmentType.Condition:
                    var conditionFragment = (Condition) fragment;
                    format = Dictionary[fragmentType.ToString()].Format;
                    GenComparison comparison;
                    if (!Enum.TryParse(conditionFragment.Comparison, out comparison))
                        throw new GeneratorException("Invalid condition comparison: " + conditionFragment.Comparison,
                            GenErrorType.Assertion);
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         Identifier(conditionFragment.Class1, conditionFragment.Property1),
                                                         Dictionary.GenComparisonText[(int) comparison],
                                                         (comparison == GenComparison.Exists ||
                                                          comparison == GenComparison.NotExists)
                                                             ? ""
                                                             : (conditionFragment.UseLit != ""
                                                                 ? GenUtilities.StringOrName(conditionFragment.Lit)
                                                                 : conditionFragment.Class2 + "." +
                                                                   conditionFragment.Property2),
                                                         GetBodyText(conditionFragment.Body())
                                                     }
                        ));
                    break;
                case FragmentType.Function:
                    var functionFragment = (Function) fragment;
                    format = Dictionary[fragmentType.ToString()].Format;

                    var body = functionFragment.Body().FragmentList;
                    var param = new string[body.Count];
                    for (var i = 0; i < body.Count; i++)
                        param[i] = GetText(body[i]);
                    var separator = Dictionary.FunctionParameterSeparator;
                    var p = string.Join(separator, param);

                    OutputText(string.Format(format, new object[]
                                                     {
                                                         functionFragment.FunctionName,
                                                         p
                                                     }
                        ));
                    break;
                case FragmentType.TextBlock:
                    var textBlockFragment = (TextBlock) fragment;
                    OutputText(GetBodyText(textBlockFragment.Body()));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string Identifier(string className, string propertyName)
        {
            return className + '.' + propertyName;
        }

        private string GetBodyText(FragmentBody body)
        {
            var pt = new GenProfileTextExpander(Dictionary);
            return GetBodyText(pt, body);
        }
    }
}