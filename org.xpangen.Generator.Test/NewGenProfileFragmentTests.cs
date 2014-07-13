using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using org.xpangen.Generator.Data.Model.NewProfile;
using org.xpangen.Generator.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests profile fragments using the Profile Definition data and visitor pattern
    /// </summary>
    [TestFixture]
    public class NewGenProfileFragmentTests: GenProfileFragmentsTestBase
    {
        class FragmentData
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
                            return typeof(Data.Model.NewProfile.Text);
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

        class FragmentDataList: List<FragmentData>
        {
            public FragmentDataList()
            {
                for (var fragmentType = (FragmentType)0; fragmentType < FragmentType.TextBlock; fragmentType++)
                {
                    Add(new FragmentData(fragmentType));
                }
            } 
        }
        [TestCase(Description="Verifies that the fragment factory creates the correct class fragments.")]
        public void FactoryTest()
        {
            var p = CreateAllFragmentProfileDefinition();
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
            return p;
        }
    }
}
