// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the profile editor functionality
    /// </summary>
    [TestFixture]
    public class EditorHelperProfileTests : GenDataTestsBase
    {
        [Test(Description = "")]
        public void EditorSetupTest()
        {
            var geData = new GeData();
            var model = PopulateGenSettings();
            geData.Settings = new GeSettings(model);
            var fileGroup = geData.Settings.GetFileGroup("ProgramDefinition");
            var baseFile = geData.Settings.GetBaseFiles().Find(fileGroup.Profile);
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
