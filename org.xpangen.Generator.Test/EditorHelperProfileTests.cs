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
        private const string NewProfileText = "Profile text";
        [Test(Description = "")]
        public void NewProfileSetupTest()
        {
            var geData = GeData.GetDefaultGeData(true);
            var model = PopulateGenSettings();
            geData.Settings = new GeSettings(model);
            geData.SetFileGroup("Definition");
            geData.Profile.CreateNewProfile("NewProfile", NewProfileText);
            Assert.AreEqual(FragmentType.Segment, geData.Profile.Fragment.FragmentType);
            var segment = (Segment) geData.Profile.Fragment;
            Assert.AreEqual("Class", segment.ClassName());
            Assert.AreEqual("", geData.Profile.GenObject.ClassName);
            Assert.AreEqual(NewProfileText, geData.Profile.GetNodeExpansionText(geData.GenDataBase, null));
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
