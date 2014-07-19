// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator profile fragment functionality
    /// </summary>
    [TestFixture]
    public class ProfileDataTests : GenProfileFragmentsTestBase
    {
        /// <summary>
        /// Ensure that each fragment automatically has its profile definition data created and set
        /// </summary>
        [TestCase(Description = "Ensure that each fragment automatically has its profile definition data created and set")]
        public void NoProfileDefintionTest()
        {
            var data = new GeData();
            data.Settings = data.GetDefaultSettings();
            data.SetFileGroup("GeneratorDefinitionModel");
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
