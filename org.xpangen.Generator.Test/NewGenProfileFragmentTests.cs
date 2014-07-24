using System;
using System.Text;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Definition;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Profile;
using Text = org.xpangen.Generator.Profile.Profile.Text;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests profile fragments using the Profile Definition data and visitor pattern
    /// </summary>
    [TestFixture]
    public class NewGenProfileFragmentTests: GenProfileFragmentsTestBase
    {
        [TestCase(Description = "Verifies that the fragment text is produced correctly from the fragment data.")]
        public void FragmentTextTest()
        {
            var p = CreateAllFragmentProfileDefinition();
            var profile = p.Profile();
            var fragments = profile.Body().FragmentList;
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
            Assert.AreEqual("`Class.Name`Some text`[Class>:`]`{`]`?Class.Name=Class:`]`%Class.Name=Class.Name:`]`@Map:`]", t.GetText(profile));
        }

        private static ProfileDefinition CreateAllFragmentProfileDefinition()
        {
            var p = CreateEmptyProfileDefinition();
            var pb = p.Profile().Body();
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

        [TestCase(Description = "Verifies that segment classes are expanded in sequence.")]
        public void ClassExpandText()
        {
            var p = CreateEmptyProfileDefinition();
            var pb = p.Profile().Body();
            var def = DefinitionCreator.Create();
            var cf = pb.AddSegment("Class", "All");
            var tb = pb.AddTextBlock();
            tb.Body().AddPlaceholder("class.name", "Class", "Name");
            tb.Body().AddText(" ");
            var op = new ProfileTextBase();
            var segmentExpander = new SegmentExpander(op, cf, def.ClassList.Find("Class"), def);
            var classes = (string) segmentExpander.Expand();
            Assert.AreEqual("Class SubClass Property ", classes);
        }
        
        private static ProfileDefinition CreateEmptyProfileDefinition()
        {
            var p = new ProfileDefinition();
            var r = p.AddProfileRoot("ProfileRoot");
            var fb = r.AddFragmentBody("ProfileRoot");
            var pr = fb.AddProfile();
            pr.AddFragmentBody("Profile");
            return p;
        }
    }

    public class SegmentExpander : ProfileOutputBase
    {
        public Segment Segment { get; set; }
        public Class Definition { get; set; }
        public GenApplicationBase Data { get; set; }

        public SegmentExpander(ProfileOutputBase output, Segment segment, Class definition, GenApplicationBase data) : base(output)
        {
            Segment = segment;
            Definition = definition;
            Data = data;
        }

        public object Expand()
        {
            var list = Data.Lists[Segment.Class + "List"];
            foreach (var item in list)
            {
                
            }
            return GetOutput();
        }
    }

    public abstract class ProfileOutputBase
    {
        private ProfileOutputBase Output { get; set; }

        public ProfileOutputBase()
        {
            
        }

        public ProfileOutputBase(ProfileOutputBase profileOutputBase)
        {
            Output = profileOutputBase;
        }

        protected virtual object GetOutput()
        {
            if (Output == null)
                throw new NotImplementedException();
            return Output.GetOutput();
        }

        protected virtual void PrepareOutput()
        {
            if (Output == null)
                throw new NotImplementedException();
            Output.PrepareOutput();
        }

        protected static FragmentType GetFragmentType(Fragment fragment)
        {
            var fragmentTypeName = fragment.GetType().Name;
            FragmentType fragmentType;
            if (!Enum.TryParse(fragmentTypeName, out fragmentType))
                throw new GeneratorException("Unknown fragment type: " + fragmentTypeName, GenErrorType.Assertion);
            return fragmentType;
        }

        protected virtual void OutputFragment(Fragment fragment)
        {
            if (Output == null)
                throw new NotImplementedException();
            Output.OutputFragment(fragment);
        }

        protected void OutputBody(FragmentBody body)
        {
            foreach (var f in body.FragmentList)
                OutputFragment(f);
        }
    }

    public class ProfileTextBase : ProfileOutputBase
    {
        public string GetText(Fragment fragment)
        {
            PrepareOutput();
            OutputFragment(fragment);
            return (string) GetOutput();
        }

        public string GetText(Profile.Profile.Profile profile)
        {
            PrepareOutput();
            OutputBody(profile.Body());
            return (string)GetOutput();
        }

        protected void OutputText(string text)
        {
            SB.Append(text);
        }

        protected override object GetOutput()
        {
            return SB.ToString();
        }

        protected override void PrepareOutput()
        {
            SB = new StringBuilder();
        }

        private StringBuilder SB { get; set; }

        protected string GetBodyText(ProfileTextBase pt, FragmentBody body)
        {
            pt.PrepareOutput();
            pt.OutputBody(body);
            return (string) pt.GetOutput();
        }
    }

    public class ProfileText : ProfileTextBase
    {
        private ProfileFragmentSyntaxDictionary Dictionary { get; set; }
        public ProfileText(ProfileFragmentSyntaxDictionary dictionary)
        {
            Dictionary = dictionary;
        }

        protected override void OutputFragment(Fragment fragment)
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

        protected virtual string GetBodyText(FragmentBody body)
        {
            ProfileTextBase pt = new ProfileText(Dictionary);
            return GetBodyText(pt, body);
        }
    }
}
