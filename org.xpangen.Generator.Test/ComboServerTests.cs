// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the discovery of context from a specified object
    /// </summary>
    [TestFixture]
    public class ComboServerTests
    {
        [TestCase(Description = "Verifies the expected run time items are available for YesNo combo")]
        public void VerifyDesignTimeYesNoCombo()
        {
            VerifyDesignTimeCombos("YesNo", new[] {new[] {"Yes", "True"}, new[] {"No", ""}});
        }

        [TestCase(Description = "Verifies the expected run time items are available for DataType combo")]
        public void VerifyDesignTimeDataTypeCombo()
        {
            VerifyDesignTimeCombos("DataType",
                new[]
                {
                    new[] {"String", "String"}, new[] {"Integer", "Integer"}, new[] {"Boolean", "Boolean"},
                    new[] {"Identifier", "Identifier"}
                });
        }

        private static void VerifyDesignTimeCombos(string listName, IList<string[]> expectedValues)
        {
            var combos = GeData.GetDesignTimeComboServer();
            var comboItems = combos.GetComboItems(listName);
            Assert.AreEqual(comboItems.Count, expectedValues.Count);
            for (var i = 0; i < expectedValues.Count; i++)
                VerifyComboItem(comboItems[i], expectedValues[i]);
        }

        private static void VerifyComboItem(GeComboItem item, IList<string> expectedValues)
        {
            Assert.AreEqual(expectedValues[0], item.DisplayValue);
            Assert.AreEqual(expectedValues[1], item.DataValue);
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
