using NUnit.Framework;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Profile;

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
            Assert.AreEqual("`Class.Name`", t.GetText(fragments[0]));
            Assert.AreEqual("Some text", t.GetText(fragments[1]));
            Assert.AreEqual("`[Class>:`]", t.GetText(fragments[2]));
            Assert.AreEqual("`{`]", t.GetText(fragments[3]));
            Assert.AreEqual("`?Class.Name=Class:`]", t.GetText(fragments[4]));
            Assert.AreEqual("`%Class.Name=Class.Name:`]", t.GetText(fragments[5]));
            Assert.AreEqual("`@Map:`]", t.GetText(fragments[6]));
            Assert.AreEqual("", t.GetText(fragments[7]));
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

        private static ProfileDefinition CreateEmptyProfileDefinition()
        {
            var p = new ProfileDefinition();
            p.Setup();
            return p;
        }
    }
}
