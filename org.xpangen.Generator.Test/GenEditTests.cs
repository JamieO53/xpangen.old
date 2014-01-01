// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
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
        public GeData Data { get; set; }

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
            Assert.AreEqual("Class", Data.GenDataDef.Classes[1].Name, "First data loaded");
            LoadData("GeneratorDefinitionModel.dcb");
            Assert.AreEqual("Solution", Data.GenDataDef.Classes[1].Name, "Second data loaded");
        }

        /// <summary>
        /// Tests that the settings data is handled correctly
        /// </summary>
        [TestCase(Description="Confirm that the settings data is handled correctly", Ignore=true)]
        public void SettingsTest()
        {
            throw new NotImplementedException("SettingsTest is not implemented");
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
            var stream = new FileStream(fileName, FileMode.Open);
            var d1 = new GenParameters(stream);

            VerifyDataCreation(d1);
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
            Data = new GeData();
        }

        /// <summary>
        /// Tear down the Generator data definition tests
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            Data = null;
        }
    }
}
