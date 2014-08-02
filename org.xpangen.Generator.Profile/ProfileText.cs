using System;
using System.Text;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class ProfileText
    {
        private ProfileFragmentSyntaxDictionary Dictionary { get; set; }
        public ProfileText(ProfileFragmentSyntaxDictionary dictionary)
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

        private StringBuilder StringBuilder { get; set; }

        public string GetText(Fragment fragment)
        {
            PrepareOutput();
            OutputFragment(fragment);
            return (string)GetOutput();
        }

        public string GetText(Generator.Profile.Profile.Profile profile)
        {
            PrepareOutput();
            OutputBody(profile.Body());
            return (string)GetOutput();
        }

        private void OutputText(string text)
        {
            StringBuilder.Append(text);
        }

        private object GetOutput()
        {
            return StringBuilder.ToString();
        }

        private void PrepareOutput()
        {
            StringBuilder = new StringBuilder();
        }

        private string GetBodyText(ProfileText pt, FragmentBody body)
        {
            pt.PrepareOutput();
            pt.OutputBody(body);
            return (string)pt.GetOutput();
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
                    format = Dictionary[fragmentType.ToString()].Format;
                    var segmentFragment = (Segment) fragment;
                    GenCardinality cardinality;
                    if (!Enum.TryParse(segmentFragment.Cardinality, out cardinality))
                        throw new GeneratorException("Invalid segment cardinality: " + segmentFragment.Cardinality,
                            GenErrorType.Assertion);
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         segmentFragment.Class,
                                                         GetBodyText(segmentFragment.Body()),
                                                         Dictionary.GenCardinalityText[(int) cardinality]
                                                     }
                        ));
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
                    format = Dictionary[fragmentType.ToString() + (lookupFragment.NoMatch == "" ? "1" : "2")].Format;
                    OutputText(string.Format(format, new object[]
                                                     {
                                                         lookupFragment.Class1 + "." + lookupFragment.Property1,
                                                         lookupFragment.Class2 + "." + lookupFragment.Property2,
                                                         GetBodyText(lookupFragment.Body())
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
                                                         conditionFragment.Class1 + "." + conditionFragment.Property1,
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

        private string GetBodyText(FragmentBody body)
        {
            var pt = new ProfileText(Dictionary);
            return GetBodyText(pt, body);
        }
    }
}