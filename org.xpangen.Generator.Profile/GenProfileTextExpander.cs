// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.FunctionLibrary;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenProfileTextExpander
    {
        private ProfileFragmentSyntaxDictionary Dictionary { get; set; }
        public ProfileTextPostionList ProfileTextPostionList { get; private set; }

        public GenProfileTextExpander(ProfileFragmentSyntaxDictionary dictionary)
        {
            Dictionary = dictionary;
            ProfileTextPostionList = new ProfileTextPostionList();
        }

        private static FragmentType GetFragmentType(Fragment fragment)
        {
            //var fragmentTypeName = fragment.GetType().Name;
            //FragmentType fragmentType;
            //if (!Enum.TryParse(fragmentTypeName, out fragmentType))
            //    throw new GeneratorException("Unknown fragment type: " + fragmentTypeName, GenErrorType.Assertion);
            //return fragmentType;
            return fragment.FragmentType;
        }

        private GenWriter Writer { get; set; }

        public string GetText(Fragment fragment, TextPosition bodyPosition = null)
        {
            var saveWriter = Writer;
            using (var stream = new MemoryStream(100000))
            {
                using (var writer = new GenWriter(stream))
                {
                    Writer = writer;
                    OutputFragment(fragment, bodyPosition ?? new TextPosition());
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    Writer = saveWriter;
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
        }

        private void OutputText(string text, TextPosition position)
        {
            Writer.Write(text, position);
        }

        private string GetBodyText(GenProfileTextExpander pt, FragmentBody body, TextPosition position)
        {
            position.Offset = Writer.Position;
            var saveWriter = Writer;
            using (var stream = new MemoryStream(100000))
            {
                using (var writer = new GenWriter(stream))
                {
                    pt.Writer = writer;
                    pt.OutputBody(body, position);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    pt.Writer = saveWriter;
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
        }

        private void OutputBody(FragmentBody body, TextPosition bodyPosition)
        {
            foreach (var f in body.FragmentList)
                OutputFragment(f, bodyPosition);
        }

        private void OutputFragment(Fragment fragment, TextPosition bodyPosition)
        {
            var fragmentType = GetFragmentType(fragment);
            string format;
            var fragmentPosition = new ProfileTextPosition(new TextPosition(), new TextPosition(), new TextPosition(),
                fragment);
            switch (fragmentType)
            {
                case FragmentType.Null:
                    break;
                case FragmentType.Text:
                    OutputText(((Text) fragment).TextValue, fragmentPosition.Position);
                    break;
                case FragmentType.Placeholder:
                    format = "`{0}.{1}`";
                    var placeholderFragment = (Placeholder) fragment;
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         placeholderFragment.Class,
                                                         placeholderFragment.Property
                                                     }
                        ), fragmentPosition.Position);
                    break;
                case FragmentType.Segment:
                    var segmentFragment = (Segment) fragment;
                    var cardinality = segmentFragment.GenCardinality;
                    var separated = cardinality == GenCardinality.AllDlm || cardinality == GenCardinality.BackDlm;
                    format = (separated
                        ? "`[{0}{2}:{1}`;{3}`]"
                        : "`[{0}{2}:{1}`]");
                    if (!separated)
                        OutputText(string.Format(format, new object[]
                                                         {
                                                             segmentFragment.Class,
                                                             GetBodyText(segmentFragment.Body(),
                                                                 fragmentPosition.BodyPosition),
                                                             Dictionary.GenCardinalityText[(int) cardinality]
                                                         }
                            ), fragmentPosition.Position);
                    else
                        OutputText(string.Format(format, new object[]
                                                         {
                                                             segmentFragment.Class,
                                                             GetBodyText(segmentFragment.Body(),
                                                                 fragmentPosition.BodyPosition),
                                                             Dictionary.GenCardinalityText[(int) cardinality],
                                                             GetBodyText(segmentFragment.SecondaryBody(),
                                                                 fragmentPosition.SecondaryBodyPosition)
                                                         }
                            ), fragmentPosition.Position);
                    break;
                case FragmentType.Profile:
                    OutputBody(((Profile.Profile) fragment).Body(), bodyPosition);
                    break;
                case FragmentType.Block:
                    format = "`{{{0}`]";
                    var blockFragment = (Block) fragment;
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         GetBodyText(blockFragment.Body(), fragmentPosition.BodyPosition)
                                                     }
                        ), fragmentPosition.Position);
                    break;
                case FragmentType.Lookup:
                    var lookupFragment = (Lookup) fragment;
                    var noMatch = lookupFragment.SecondaryBody().FragmentList.Count > 0;
                    format = noMatch ? "`%{0}={1}:{2}`;{3}`]" : "`%{0}={1}:{2}`]";
                    if (!noMatch)
                        OutputText(string.Format(format, new object[]
                                                         {
                                                             Identifier(lookupFragment.Class1, lookupFragment.Property1),
                                                             Identifier(lookupFragment.Class2, lookupFragment.Property2),
                                                             GetBodyText(lookupFragment.Body(),
                                                                 fragmentPosition.BodyPosition)
                                                         }
                            ), fragmentPosition.Position);
                    else
                        OutputText(string.Format(format, new object[]
                                                         {
                                                             Identifier(lookupFragment.Class1, lookupFragment.Property1),
                                                             Identifier(lookupFragment.Class2, lookupFragment.Property2),
                                                             GetBodyText(lookupFragment.Body(),
                                                                 fragmentPosition.BodyPosition),
                                                             GetBodyText(lookupFragment.SecondaryBody(),
                                                                 fragmentPosition.SecondaryBodyPosition)
                                                         }
                            ), fragmentPosition.Position);
                    break;
                case FragmentType.Condition:
                    var conditionFragment = (Condition) fragment;
                    format = "`?{0}{1}{2}:{3}`]";
                    GenComparison comparison;
                    if (!Enum.TryParse(conditionFragment.Comparison, out comparison))
                        throw new GeneratorException("Invalid condition comparison: " + conditionFragment.Comparison,
                            GenErrorType.Assertion);
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         Identifier(conditionFragment.Class1,
                                                             conditionFragment.Property1),
                                                         Dictionary.GenComparisonText[(int) comparison],
                                                         (comparison == GenComparison.Exists ||
                                                          comparison == GenComparison.NotExists)
                                                             ? ""
                                                             : (conditionFragment.UseLit != ""
                                                                 ? GenUtilities.StringOrName(conditionFragment.Lit)
                                                                 : conditionFragment.Class2 + "." +
                                                                   conditionFragment.Property2),
                                                         GetBodyText(conditionFragment.Body(),
                                                             fragmentPosition.BodyPosition)
                                                     }
                        ), fragmentPosition.Position);
                    break;
                case FragmentType.Function:
                    var functionFragment = (Function) fragment;
                    format = "`@{0}:{1}`]";

                    var body = functionFragment.Body().FragmentList;
                    var param = new string[body.Count];
                    for (var i = 0; i < body.Count; i++)
                        param[i] = GetText(body[i], bodyPosition);
                    var separator = Dictionary.FunctionParameterSeparator;
                    var p = string.Join(separator, param);

                    OutputText(string.Format(format, new object[]
                                                     {
                                                         functionFragment.FunctionName,
                                                         p
                                                     }
                        ), fragmentPosition.Position);
                    break;
                case FragmentType.TextBlock:
                    var textBlockFragment = (TextBlock) fragment;
                    OutputText(GetBodyText(textBlockFragment.Body(), fragmentPosition.BodyPosition),
                        fragmentPosition.Position);
                    break;
                case FragmentType.Annotation:
                    format = "`-{0}`]";
                    var annotationFragment = (Annotation) fragment;
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         GetBodyText(annotationFragment.Body(),
                                                             fragmentPosition.BodyPosition)
                                                     }
                        ), fragmentPosition.Position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            bodyPosition.Length += fragmentPosition.Position.Length;
        }

        private static string Identifier(string className, string propertyName)
        {
            return className + '.' + propertyName;
        }

        private string GetBodyText(FragmentBody body, TextPosition position)
        {
            var pt = new GenProfileTextExpander(Dictionary);
            return GetBodyText(pt, body, position);
        }
    }
}