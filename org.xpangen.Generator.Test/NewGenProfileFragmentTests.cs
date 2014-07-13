using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.NewProfile;
using org.xpangen.Generator.Profile;
using Text = org.xpangen.Generator.Data.Model.NewProfile.Text;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests profile fragments using the Profile Definition data and visitor pattern
    /// </summary>
    [TestFixture]
    public class NewGenProfileFragmentTests: GenProfileFragmentsTestBase
    {
        public class FragmentData
        {
            public FragmentData(FragmentType fragmentType)
            {
                FragmentType = fragmentType;
            }

            FragmentType FragmentType { get; set; }
            Type FragmentProfileType
            {
                get
                {
                    switch (FragmentType)
                    {
                        case FragmentType.Profile:
                            return typeof (GenProfileFragment);
                        case FragmentType.Null:
                            return typeof (GenNullFragment);
                        case FragmentType.Text:
                            return typeof(GenTextFragment);
                        case FragmentType.Placeholder:
                            return typeof(GenPlaceholderFragment);
                        case FragmentType.Body:
                            return typeof(GenSegBody);
                        case FragmentType.Segment:
                            return typeof (GenSegment);
                        case FragmentType.Block:
                            return typeof(GenBlock);
                        case FragmentType.Lookup:
                            return typeof(GenLookup);
                        case FragmentType.Condition:
                            return typeof(GenCondition);
                        case FragmentType.Function:
                            return typeof(GenFunction);
                        case FragmentType.TextBlock:
                            return typeof(GenTextBlock);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            Type FragmentDataType{
                get
                {
                    switch (FragmentType)
                    {
                        case FragmentType.Profile:
                            return typeof(Data.Model.NewProfile.Profile);
                        case FragmentType.Null:
                            return typeof(Null);
                        case FragmentType.Text:
                            return typeof(Text);
                        case FragmentType.Placeholder:
                            return typeof(Placeholder);
                        case FragmentType.Body:
                            return typeof(Fragment);
                        case FragmentType.Segment:
                            return typeof(Segment);
                        case FragmentType.Block:
                            return typeof(Block);
                        case FragmentType.Lookup:
                            return typeof(Lookup);
                        case FragmentType.Condition:
                            return typeof(Condition);
                        case FragmentType.Function:
                            return typeof(Function);
                        case FragmentType.TextBlock:
                            return typeof(TextBlock);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private class FragmentDataList: List<FragmentData>
        {
            public FragmentDataList()
            {
                foreach (FragmentType fragmentType in Enum.GetValues(typeof(FragmentType)))
                    Add(new FragmentData(fragmentType));
            }
        }

        private static FragmentDataList Fragments { get { return new FragmentDataList(); }}

        [TestCase(Description = "Verifies that the fragment factory creates the correct class fragments.")]
        //public void FactoryTest([ValueSource("Fragments")] FragmentData fragmentData)
        public void FactoryTest()
        {
            var p = CreateAllFragmentProfileDefinition();
            var fragments = p.ProfileRootList[0].ProfileList[0].FragmentBodyList[0].FragmentList;
            var t = new ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary);
            Assert.AreEqual("", t.GetText(fragments[0]));
            Assert.AreEqual("`Class.Name`", t.GetText(fragments[1]));
            Assert.AreEqual("Some text", t.GetText(fragments[2]));
            Assert.AreEqual("`[Class>:`]", t.GetText(fragments[3]));
            Assert.AreEqual("`{`]", t.GetText(fragments[4]));
            Assert.AreEqual("`?Class.Name=Class:`]", t.GetText(fragments[5]));
            Assert.AreEqual("`%Class.Name=Class.Name:`]", t.GetText(fragments[6]));
            Assert.AreEqual("`@Map:`]", t.GetText(fragments[7]));
            Assert.AreEqual("", t.GetText(fragments[8]));
        }

        private static ProfileDefinition CreateAllFragmentProfileDefinition()
        {
            var p = new ProfileDefinition();
            var r = p.AddProfileRoot("ProfileRoot");
            var pr = r.AddProfile("Profile");
            var f = r.AddDefinition("Definition");
            var pb = pr.AddFragmentBody("Profile");
            pb.AddNull("Null");
            pb.AddPlaceholder("PlaceHolder", "Class", "Name");
            pb.AddText("Text", "Some text");
            pb.AddSegment("Class", "All");
            pb.AddBlock();
            pb.AddCondition("Class", "Name", "Eq", "", "", "Class", "True");
            pb.AddLookup("", "Class", "Name", "Class", "Name");
            pb.AddFunction("Map");
            pb.AddTextBlock();
            return p;
        }
    }

    public class ProfileText
    {
        private ProfileFragmentSyntaxDictionary Dictionary { get; set; }
        public ProfileText(ProfileFragmentSyntaxDictionary dictionary)
        {
            Dictionary = dictionary;
        }

        public string GetText(Fragment fragment)
        {
            var fragmentType = GetFragmentType(fragment);
            string format;
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    throw new NotImplementedException();
                case FragmentType.Null:
                    return "";
                case FragmentType.Text:
                    return ((Text)fragment).TextValue;
                case FragmentType.Placeholder:
                    format = Dictionary[fragmentType.ToString()].Format;
                    var placeholderFragment = (Placeholder) fragment;
                    return string.Format(format, new object[]
                                                     {
                                                         placeholderFragment.Class,
                                                         placeholderFragment.Property
                                                     }
                        );
                case FragmentType.Body:
                    throw new NotImplementedException();
                case FragmentType.Segment:
                    format = Dictionary[fragmentType.ToString()].Format;
                    var segmentFragment = (Segment) fragment;
                    GenCardinality cardinality;
                    if (!Enum.TryParse(segmentFragment.Cardinality, out cardinality))
                        throw new GeneratorException("Invalid segment cardinality: " + segmentFragment.Cardinality, GenErrorType.Assertion);
                    return string.Format(format, new object[]
                                             {
                                                 segmentFragment.Class,
                                                 GetBodyText(segmentFragment),
                                                 Dictionary.GenCardinalityText[(int) cardinality]
                                             }
                        );
                case FragmentType.Block:
                    format = Dictionary[fragmentType.ToString()].Format;
                    var blockFragment = (Block)fragment;
                    return string.Format(format, new object[]
                                             {
                                                 GetBodyText(blockFragment)
                                             }
                        );
                case FragmentType.Lookup:
                    var lookupFragment = (Lookup)fragment;
                    format = Dictionary[fragmentType.ToString() + (lookupFragment.NoMatch == "" ? "1" : "2")].Format;
                    return string.Format(format, new object[]
                                             {
                                                 lookupFragment.Class1 + "." + lookupFragment.Property1,
                                                 lookupFragment.Class2 + "." + lookupFragment.Property2,
                                                 GetBodyText(lookupFragment)
                                             }
                        );
                case FragmentType.Condition:
                    var conditionFragment = (Condition)fragment;
                    format = Dictionary[fragmentType.ToString()].Format;
                    GenComparison comparison;
                    if (!Enum.TryParse(conditionFragment.Comparison, out comparison))
                        throw new GeneratorException("Invalid condition comparison: " + conditionFragment.Comparison, GenErrorType.Assertion);
                    return string.Format(format, new object[]
                                             {
                                                 conditionFragment.Class1 + "." + conditionFragment.Property1,
                                                 Dictionary.GenComparisonText[(int) comparison],
                                                 (comparison == GenComparison.Exists ||
                                                  comparison == GenComparison.NotExists)
                                                     ? ""
                                                     : (conditionFragment.UseLit != ""
                                                            ? GenUtilities.StringOrName(conditionFragment.Lit)
                                                            : conditionFragment.Class2 + "." + conditionFragment.Property2),
                                                 GetBodyText(conditionFragment)
                                             }
                        );
                case FragmentType.Function:
                    var functionFragment = (Function)fragment;
                    format = Dictionary[fragmentType.ToString()].Format;

                    var body = functionFragment.Body().FragmentList;
                    var param = new string[body.Count];
                    for (var i = 0; i < body.Count; i++)
                        param[i] = GetText(body[i]);
                    var p = string.Join(" ", param);

                    return string.Format(format, new object[]
                                             {
                                                 functionFragment.FunctionName,
                                                 p
                                             }
                        );
                case FragmentType.TextBlock:
                    var textBlockFragment = (TextBlock)fragment;
                    return GetBodyText(textBlockFragment);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetBodyText(ContainerFragment fragment)
        {
            PrepareBodyOutput();
            foreach (var f in fragment.Body().FragmentList)
                OutputBodyFragment(f);
            return GetBodyOutput();
        }

        private string GetBodyOutput()
        {
            return SB.ToString();
        }

        private void OutputBodyFragment(Fragment f)
        {
            SB.Append(GetText(f));
        }

        private void PrepareBodyOutput()
        {
            SB = new StringBuilder();
        }

        private StringBuilder SB { get; set; }

        private static FragmentType GetFragmentType(Fragment fragment)
        {
            var fragmentTypeName = fragment.GetType().Name;
            FragmentType fragmentType;
            if (!Enum.TryParse(fragmentTypeName, out fragmentType))
                throw new GeneratorException("Unknown fragment type: " + fragmentTypeName, GenErrorType.Assertion);
            return fragmentType;
        }
    }
}
