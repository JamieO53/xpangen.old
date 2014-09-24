// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Definition;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator data functionality
    /// </summary>
    [TestFixture]
    public class GenDataInheritanceTests : GenProfileFragmentsTestBase
    {
        [TestCase(Description = "Tests the setting up of a definition with inheritance")]
        public void InheritanceDefinitionSetupTest()
        {
            var df = SetUpVirtualDefinition();
            var d = (new GenData(df.GenDataBase));
            var def = d.AsDef();
            Assert.AreEqual(VirtualDefinitionProfile, GenDataDefProfile.CreateProfile(def));
            var data = def.AsGenData();
            CompareGenData(d, data.GenDataBase);
        }

        [TestCase(Description = "Tests the creation of a data profile with inheritance")]
        public void InheritanceDataProfileTest()
        {
            var df = SetUpVirtualDefinition();
            var d = (new GenData(df.GenDataBase));
            var p = GenParameters.CreateProfile(d.AsDef());
            var profileText = p.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary).Replace(">:", ":");
            Assert.AreEqual(VirtualDefinitionProfile, profileText);
        }

        [TestCase(Description = "Tests the saving of data with inheritance")]
        public void InheritanceDataSaveTest()
        {
            var dataFile = GetTestDataFileName("InheritanceDataSaveTest");
            var d = PopulateInheritanceData(dataFile);
            var p = GenParameters.CreateProfile(d.GenDataDef);
            p.GenObject = d.Root;
            var text = GenFragmentExpander.Expand(d.GenDataDef, d.Root, p.Fragment);
            Assert.AreEqual(VirtualDefinitionData, text);
        }

        [TestCase(Description = "Tests the expansion of data with inheritance")]
        public void InheritanceDataExpansionTest()
        {
            var dataFile = GetTestDataFileName("InheritanceDataExpansionTest");
            var d = PopulateInheritanceData(dataFile);
            GenParameters.SaveToFile(d.GenDataBase, dataFile);
            var text = File.ReadAllText(dataFile);
            Assert.AreEqual(VirtualDefinitionData, text);
        }

        [TestCase(Description = "Tests the loading of data with inheritance")]
        public void InheritanceDataLoadTest()
        {
            var dataFile = GetTestDataFileName("InheritanceDataLoadTest");
            SetUpParametersFile(dataFile, VirtualDefinitionData);
            var d = PopulateInheritanceData(dataFile);
            var x = GenData.DataLoader.LoadData(d.GenDataDef, dataFile);
            CompareGenData(d, x);
        }

        [TestCase(Description = "Tests the loading of data with inheritance without a definition")]
        public void InheritanceDataLoadSansDefinitionTest()
        {
            var dataFile = GetTestDataFileName("InheritanceDataLoadSansDefinitionTest");
            SetUpParametersFile(dataFile, VirtualDefinitionData);
            var d = PopulateInheritanceData(dataFile);
            var x = GenData.DataLoader.LoadData(dataFile);
            CompareGenData(d, x);
        }

        [TestCase(Description = "Tests the expansion of a class with inheritance")]
        public void InheritanceClassExpansionTest()
        {
            var dataFile = GetTestDataFileName("InheritanceClassExpansionTest");
            var d = PopulateInheritanceData(dataFile);
            var p = new GenCompactProfileParser(d.GenDataDef, "", InheritanceProfile);
            var text = GenFragmentExpander.Expand(d.GenDataDef, d.Root, p.Fragment);
            Assert.AreEqual(InheritanceProfileResult, text);
        }

        [TestCase(Description = "Tests the generation of a class with inheritance")]
        public void InheritanceClassGenerationTest()
        {
            var dataFile = GetTestDataFileName("InheritanceClassGenerationTest");
            var d = PopulateInheritanceData(dataFile);
            var p = new GenCompactProfileParser(d.GenDataDef, "", InheritanceProfile);
            var text = GenerateFragment(d, p);
            Assert.AreEqual(InheritanceProfileResult, text);
        }

        [TestCase(Description = "Tests the loading of nested data with inheritance")]
        public void NestedInheritanceDataLoadTest()
        {
            var data = LoadVirtualParentData();
            var expectedData = SetUpParentOfVirtualData();
            CompareGenData(expectedData, data);
        }

        [TestCase(Description = "Tests the saving of nested data with inheritance")]
        public void NestedInheritanceDataSaveTest()
        {
            SaveVirtualAndParentData();
            var def = File.ReadAllText(VirtualDefinitionFile);
            Assert.AreEqual(VirtualDefinition, def);
            var data = File.ReadAllText(VirtualDataFile);
            Assert.AreEqual(VirtualDefinitionData, data);
            def = File.ReadAllText(VirtualParentDefinitionFile);
            Assert.AreEqual(VirtualParentDefinition, def);
            data = File.ReadAllText(VirtualParentDataFile);
            Assert.AreEqual(VirtualParentData, data);
        }

        [TestCase(Description = "Tests the expansion of a nested class with inheritance")]
        public void NestedInheritanceClassExpansionTest()
        {
            var d = LoadVirtualParentData();
            var p = new GenCompactProfileParser(d.GenDataDef, "", NestedInheritanceProfile);
            var text = GenFragmentExpander.Expand(d.GenDataDef, d.Root, p.Fragment);
            Assert.AreEqual(InheritanceProfileResult, text);
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
