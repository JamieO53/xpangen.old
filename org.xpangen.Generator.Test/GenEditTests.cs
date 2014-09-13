// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the processing of generator parameter files
    /// </summary>
	[TestFixture]
    public class GenEditTests : GenProfileFragmentsTestBase
    {
        private GeData Data { get; set; }

        /// <summary>
        /// Tests that the view model is created correctly
        /// </summary>
        [TestCase(Description="Data creation test")]
        public void DataCreationTest()
        {
            Assert.IsNotNull(Data, "GEData object not created");
            Assert.IsNotNull(Data.GenDataStore, "Generator data object created on the fly");
            Assert.IsNull(Data.DefGenData, "The base does not exist to start with");
            Assert.IsNull(Data.GenData, "The data does not exist to start with");
            Assert.IsFalse(Data.GenDataStore.Changed, "No data to have changed");
        }

        /// <summary>
        /// Tests that the base generator data file is opened correctly
        /// </summary>
        [TestCase(Description="Base File Open test")]
        public void GenBaseFileOpenTest()
        {
            LoadBase("Minimal.dcb");
            Assert.IsNotNull(Data.GenData, "An empty Data object should be created");
            Assert.IsFalse(Data.GenDataStore.Changed, "Newly created data should be unchanged");
            VerifyGenDataNotLoaded();
            Data.GenDataStore.SetBase("");
            Assert.IsNull(Data.DefGenData, "The data should now be gone");
            Assert.IsNotNull(Data.GenData, "The new Data object should still be there");
        }

        /// <summary>
        /// Tests that the base and data generator data files are opened correctly
        /// </summary>
        [TestCase(Description="Base and Data File Open test")]
        public void GenBaseAndDataFileOpenTest()
        {
            LoadBase("Minimal.dcb");
            LoadData("Basic.dcb");
            Assert.IsNotNull(Data.DefGenData, "The base should be loaded when needed");
            Assert.IsNotNull(Data.GenData, "A data object should be loaded when needed");
            Assert.IsFalse(Data.GenDataStore.Changed, "Newly loaded data should be unchanged");
            VerifyGenDataLoaded();
        }

        /// <summary>
        /// Tests that the data generator data file is reopened correctly
        /// </summary>
        [TestCase(Description="Data File Reopen test")]
        public void GenDataFileReopenTest()
        {
            LoadData("Minimal.dcb");
            Assert.AreEqual("Class", Data.GenDataDef.GetClassName(1), "First data loaded");
            LoadData("GeneratorDefinitionModel.dcb");
            Assert.AreEqual("Solution", Data.GenDataDef.GetClassName(1), "Second data loaded");
        }

        /// <summary>
        /// Tests that the settings data is handled correctly
        /// </summary>
        [TestCase(Description="Confirm that the settings data is handled correctly")]
        public void SettingsTest()
        {
            const string name = "Definition";
            Assert.AreNotEqual(0, Data.Settings.GetFileGroups().Count);
            var definitionFileGroup = Data.Settings.FindFileGroup(name);
            Assert.IsNotNull(definitionFileGroup);
            Assert.IsNotNull(Data.Settings.FindBaseFile(name));
            Assert.AreNotEqual(0, Data.Settings.GetFileGroups().IndexOf(definitionFileGroup));
            Assert.AreSame(definitionFileGroup, Data.Settings.GetFileGroup(name));
            Assert.AreEqual(0, Data.Settings.GetFileGroups().IndexOf(definitionFileGroup));
        }

        /// <summary>
        /// Tests that the settings data is handled correctly
        /// </summary>
        [TestCase(Description = "Confirm that the a new file group is created correctly")]
        public void SettingsNewFileGroupTest()
        {
            const string name = "Test";
            if (File.Exists("TestData\\" + name)) File.Delete("TestData\\" + name);
            var d = PopulateGenSettings().GenData;
            Data.Settings = Data.LoadSettingsFromData(d);
            Assert.IsNull(Data.Settings.FindFileGroup(name));
            var added = Data.NewFileGroup();
            added.Name = name;
            added.FilePath = "TestData";
            added.FileName = name + ".dcb";
            Assert.AreEqual("Definition", added.BaseFileName);
            Data.CreateFile(added);
            Assert.IsNotNull(Data.Settings.FindFileGroup(name));
            Assert.That(File.Exists("TestData\\" + name + ".dcb"));
            Assert.IsNotNull(Data.GenData);
            Assert.AreEqual(name, Data.GenData.DataName);
        }

        /// <summary>
        /// Tests that the generator data is saved correctly
        /// </summary>
        [TestCase(Description="Confirm that the generator data is saved correctly")]
        public void GenDataFileSaveTest()
        {
            const string fileName = "GenProfileTest.dcb";
            var f = GenDataDef.CreateMinimal();
            var d = f.AsGenData();

            if (File.Exists(fileName))
                File.Delete(fileName);

            GenParameters.SaveToFile(d, fileName);
            Assert.IsTrue(File.Exists(fileName));
            GenParameters d1;
            using (var stream = new FileStream(fileName, FileMode.Open))
                d1 = new GenParameters(stream) { DataName = "GenProfileTest" };

            VerifyDataCreation(d1);
        }

        /// <summary>
        /// Tests that an empty settings file is correctly initialized
        /// </summary>
        [TestCase(Description="Tests that an empty settings file is correctly initialized")]
        public void EmptySettingsTest()
        {
            var f = GenData.DataLoader.LoadData("GeneratorEditor").AsDef();
            var d = new GenData(f);
            GenParameters.SaveToFile(d, "Settings.dcb");
            var data = new GeData();
            data.Settings = data.GetDefaultSettings();
            Assert.AreEqual(0, data.Settings.GetFileGroups().Count);
            Assert.AreEqual(1, data.Settings.GetBaseFiles().Count);
        }

        /// <summary>
        /// Tests that an empty settings file is correctly initialized
        /// </summary>
        [TestCase(Description = "Tests that design time settings are correctly initialized")]
        public void DesignTimeSettingsTest()
        {
            Data.GetDesignTimeSettings();
            var data = new GeData();
            data.Settings = data.GetDesignTimeSettings();
            Assert.AreEqual(1, data.Settings.GetFileGroups().Count);
            Assert.AreEqual(1, data.Settings.GetBaseFiles().Count);
        }

        private void LoadData(string filePath)
        {
            Data.GenDataStore.SetData(@"Data\" + filePath);
        }

        private void LoadBase(string filePath)
        {
            Data.GenDataStore.SetBase(@"Data\" + filePath);
        }

        private void VerifyGenDataLoaded()
        {
            Data.GenData.First(1);
            Assert.IsFalse(Data.GenData.Context[1].Eol, "Data not loaded");
        }

        private void VerifyGenDataNotLoaded()
        {
            Data.GenData.First(1);
            Assert.IsTrue(Data.GenData.Context[1].Eol, "Data loaded");
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            GenDataLoader.Register();
            Data = new GeData();
            Data.Settings = Data.GetDefaultSettings();
        }

        /// <summary>
        /// Tear down the Generator data definition tests
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            Data = null;
            if (File.Exists("Settings.dcb"))
                File.Delete("Settings.dcb");
        }
    }
}
