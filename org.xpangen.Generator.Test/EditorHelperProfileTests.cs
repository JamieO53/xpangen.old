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
        [Test(Description = "")]
        public void NewProfileSetupTest()
        {
            const string newProfileText = "Profile text";
            const string fileGroup = "Definition";
            const string newProfile = "NewProfile";
            const string newProfileTitle = "New Profile Title";
            var geData = CreateNewProfile(fileGroup, newProfile, newProfileTitle, newProfileText);
            
            Assert.IsTrue(geData.Settings.BaseFile.ProfileList.Contains(newProfile));
            Assert.IsTrue(geData.Settings.Model.GenDataBase.Changed);
            Assert.AreEqual(FragmentType.Segment, geData.Profile.Fragment.FragmentType);
            var segment = (Segment) geData.Profile.Fragment;
            Assert.AreEqual("Class", segment.ClassName());
            Assert.AreEqual("", geData.Profile.GenObject.ClassName);
            Assert.AreEqual(newProfileText, geData.Profile.GetNodeExpansionText(geData.GenDataBase, null));
            var nodeProfileText = geData.Profile.GetNodeProfileText();
            Assert.AreEqual("`[Class>:" + newProfileText + "`]", nodeProfileText);
            Assert.AreSame(nodeProfileText, geData.Profile.ProfileText);
        }

        private static GeData CreateNewProfile(string fileGroup, string newProfile, string newProfileTitle, string newProfileText)
        {
            var geData = GeData.GetDefaultGeData(true);
            var settings = PopulateGenSettings();
            geData.Settings = new GeSettings(settings);
            geData.SetFileGroup(fileGroup);
            geData.Profile.CreateNewProfile(newProfile, newProfileTitle, newProfileText);
            return geData;
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
