// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
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

        private void OutputBody(FragmentBody body, TextPosition bodyPosition)
        {
            bodyPosition.Offset = Writer.Position;
            foreach (var f in body.FragmentList)
                OutputFragment(f, bodyPosition);
            bodyPosition.Length = Writer.Position - bodyPosition.Offset; //bodyPosition.Length;
        }

        private void OutputFragment(Fragment fragment, TextPosition bodyPosition)
        {
            var fragmentType = fragment.FragmentType;
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
                    var placeholderFragment = (Placeholder) fragment;
                    OutputText("`" + Identifier(placeholderFragment.Class, placeholderFragment.Property) + "`",
                        fragmentPosition.Position);
                    break;
                case FragmentType.Segment:
                    var segmentFragment = (Segment) fragment;
                    var cardinality = segmentFragment.GenCardinality;
                    var separated = cardinality == GenCardinality.AllDlm || cardinality == GenCardinality.BackDlm;
                    
                    OutputText("`[" + segmentFragment.Class + Dictionary.GenCardinalityText[(int) cardinality] + ":",
                        fragmentPosition.Position);
                    OutputBody(segmentFragment.Body(), fragmentPosition.BodyPosition);
                    if (separated)
                    {
                        OutputText("`;", fragmentPosition.Position);
                        OutputBody(segmentFragment.SecondaryBody(), fragmentPosition.SecondaryBodyPosition);
                    }
                    OutputText("`]", fragmentPosition.Position);
                    break;
                case FragmentType.Profile:
                    OutputBody(((Profile.Profile) fragment).Body(), bodyPosition);
                    break;
                case FragmentType.Block:
                    var blockFragment = (Block) fragment;
                    OutputText("`{", fragmentPosition.Position);
                    OutputBody(blockFragment.Body(), fragmentPosition.BodyPosition);
                    OutputText("`]", fragmentPosition.Position);
                    break;
                case FragmentType.Lookup:
                    var lookupFragment = (Lookup) fragment;
                    var noMatch = lookupFragment.SecondaryBody().FragmentList.Count > 0;

                    OutputText(
                        "`%" + Identifier(lookupFragment.Class1, lookupFragment.Property1) + "=" +
                        Identifier(lookupFragment.Class2, lookupFragment.Property2) + ":",
                        fragmentPosition.Position);
                    OutputBody(lookupFragment.Body(), fragmentPosition.BodyPosition);
                    if (noMatch)
                    {
                        OutputText("`;", fragmentPosition.Position);
                        OutputBody(lookupFragment.SecondaryBody(), fragmentPosition.SecondaryBodyPosition);
                    }
                    OutputText("`]", fragmentPosition.Position);
                    break;
                case FragmentType.Condition:
                    var conditionFragment = (Condition) fragment;
                    var comparison = conditionFragment.GenComparison;
                    var elsePart = conditionFragment.Secondary != "Empty1";

                    OutputText(
                        "`?" + Identifier(conditionFragment.Class1, conditionFragment.Property1) +
                        Dictionary.GenComparisonText[(int) comparison] +
                        ((comparison == GenComparison.Exists || comparison == GenComparison.NotExists)
                            ? ""
                            : (conditionFragment.UseLit != ""
                                ? GenUtilities.StringOrName(conditionFragment.Lit)
                                : conditionFragment.Class2 + "." + conditionFragment.Property2)) + ":",
                        fragmentPosition.Position);
                    OutputBody(conditionFragment.Body(), fragmentPosition.BodyPosition);
                    if (elsePart)
                    {
                        OutputText("`;", fragmentPosition.Position);
                        OutputBody(conditionFragment.SecondaryBody(), fragmentPosition.SecondaryBodyPosition);
                    }
                    OutputText("`]", fragmentPosition.Position);
                    break;
                case FragmentType.Function:
                    var functionFragment = (Function) fragment;
                    var body = functionFragment.Body().FragmentList;
                    var param = new string[body.Count];
                    for (var i = 0; i < body.Count; i++)
                        param[i] = GetText(body[i], bodyPosition);
                    var separator = Dictionary.FunctionParameterSeparator;
                    var p = string.Join(separator, param);

                    OutputText("`@" + functionFragment.FunctionName + ":", fragmentPosition.Position);
                    OutputText(p, fragmentPosition.Position);
                    OutputText("`]", fragmentPosition.Position);
                    break;
                case FragmentType.TextBlock:
                    var textBlockFragment = (TextBlock) fragment;
                    OutputBody(textBlockFragment.Body(), fragmentPosition.BodyPosition);
                    OutputText("", fragmentPosition.Position); // Necessary to set fragment length
                    break;
                case FragmentType.Annotation:
                    var annotationFragment = (Annotation) fragment;
                    OutputText("`-", fragmentPosition.Position);
                    OutputBody(annotationFragment.Body(), fragmentPosition.BodyPosition);
                    OutputText("`]", fragmentPosition.Position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            bodyPosition.Length += fragmentPosition.Position.Length;
            ProfileTextPostionList.Add(fragmentPosition.Position.Key, fragmentPosition);
        }

        private static string Identifier(string className, string propertyName)
        {
            return className + '.' + propertyName;
        }
    }
}