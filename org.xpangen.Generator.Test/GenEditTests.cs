using System;
using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the processing of generator parameter files
    /// </summary>
	[TestFixture]
    public class GenEditTests
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
            Assert.AreEqual("Class", Data.GenDataDef.Classes[1], "First data loaded");
            LoadData("AppGen.dcb");
            Assert.AreEqual("Program", Data.GenDataDef.Classes[1], "Second data loaded");
        }

        /// <summary>
        /// Tests that the base component is bound to the generator data
        /// </summary>
        [TestCase(Description="Confirm that the base component is bound to the data")]
        public void BaseClassTest()
        {
            throw new NotImplementedException("BaseClassTest is not implemented");
        }

        /// <summary>
        /// Tests that the settings data is handled correctly
        /// </summary>
        [TestCase(Description="Confirm that the settings data is handled correctly")]
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
            throw new NotImplementedException("GenDataFileSaveTest is not implemented");
        }

        private void LoadData(string filePath)
        {
            Data.GenDataStore.SetData(filePath);
        }

        private void LoadBase(string filePath)
        {
            Data.GenDataStore.SetBase(filePath);
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
