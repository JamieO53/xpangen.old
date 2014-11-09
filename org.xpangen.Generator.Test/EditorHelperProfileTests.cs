// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the profile editor functionality
    /// </summary>
    [TestFixture]
    public class EditorHelperProfileTests : GenDataTestsBase
    {
        [Test(Description = "Test the creation of a new profile from text.")]
        public void NewProfileSetupTest()
        {
            const string newProfileText = "Profile text";
            const string newProfile = "NewProfile";
            const string expectedProfileText = "`[Class>:" + newProfileText + "`]";

            var geData = CreateNewProfile(newProfileText);
            
            Assert.IsTrue(geData.Settings.BaseFile.ProfileList.Contains(newProfile));
            Assert.IsTrue(geData.Settings.Model.GenDataBase.Changed);
            Assert.AreEqual(FragmentType.Segment, geData.Profile.Fragment.FragmentType);
            var segment = (Segment) geData.Profile.Fragment;
            Assert.AreEqual("Class", segment.ClassName());
            Assert.AreEqual("", geData.Profile.GenObject.ClassName);
            VerifyProfile(geData, newProfileText, expectedProfileText);
        }

        [Test(Description = "Test the substitution of a placeholder for given text.")]
        public void PlaceholderSubstitutionTest()
        {
            const string newProfileText =
                "Replace this Class with a placeholder but not this class. Here is another replacement Class.";
            const string expectedProfileText =
                "`[Class>:Replace this `Class.Name` with a placeholder but not this class. Here is another replacement `Class.Name`.`]";
            var geData = CreateNewProfile(newProfileText);
            var textBlock = (TextBlock) ((Segment) geData.Profile.Fragment).Body().FragmentList[0];
            geData.Profile.SubstitutePlaceholder(textBlock, "Class", geData.GenDataDef.GetId("Class.Name"));
            VerifyProfile(geData, newProfileText, expectedProfileText);
        }

        [Test(Description = "Test the substitution of two placeholders for given text.")]
        public void MultiplePlaceholderSubstitutionTest()
        {
            const string newProfileText =
                "Replace this Class with a placeholder but not this class. It's title is Class Definition";
            const string expectedProfileText =
                "`[Class>:Replace this `Class.Name` with a placeholder but not this class. It's title is `Class.Title``]";
            var geData = CreateNewProfile(newProfileText);
            var textBlock = (TextBlock)((Segment)geData.Profile.Fragment).Body().FragmentList[0];
            geData.Profile.SubstitutePlaceholder(textBlock, "Class Definition", geData.GenDataDef.GetId("Class.Title")); 
            geData.Profile.SubstitutePlaceholder(textBlock, "Class", geData.GenDataDef.GetId("Class.Name"));
                // Substitutions must be done in this order, because the first contains the second.
            VerifyProfile(geData, newProfileText, expectedProfileText);
        }

        private static GeData CreateNewProfile(string newProfileText, string fileGroup = "Definition",
            string newProfile = "NewProfile", string newProfileTitle = "New Profile Title")
        {
            var geData = GeData.GetDefaultGeData(true);
            var settings = PopulateGenSettings();
            geData.Settings = new GeSettings(settings);
            geData.SetFileGroup(fileGroup);
            geData.Profile.CreateNewProfile(newProfile, newProfileTitle, newProfileText);
            return geData;
        }

        private static void VerifyProfile(GeData geData, string newProfileText, string expectedProfileText)
        {
            Assert.AreEqual(newProfileText, geData.Profile.GetNodeExpansionText(geData.GenDataBase, null));
            var nodeProfileText = geData.Profile.GetNodeProfileText();
            Assert.AreEqual(expectedProfileText, nodeProfileText);
            Assert.AreSame(nodeProfileText, geData.Profile.ProfileText);
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            GenDataLoader.Register();
        }

        /// <summary>
        /// Tear down the Generator data definition tests
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {

        }
    }
}
