using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the Editor Helper functionality
    /// </summary>
    [TestFixture]
    public class EditorHelperTests : GenDataTestsBase
    {
        [Test(Description = "Verifies that the test settings are set up correctly.")]
        public void SettingsLoadTest()
        {
            var model = PopulateGenSettings();
            Assert.AreEqual(1, model.GenSettingsList.Count);
            Assert.AreEqual(4, model.GenSettingsList[0].BaseFileList.Count);
            Assert.AreEqual(6, model.GenSettingsList[0].FileGroupList.Count);
            var fileGroup = model.GenSettingsList[0].FileGroupList[5];
            Assert.AreEqual("GeneratorDefinitionModel", fileGroup.Name);
        }

        [Test(Description = "Access file group properties test.")]
        public void GeSettingsAccessTest()
        {
            var geData = new GeData();
            var model = PopulateGenSettings();
            geData.Settings = new GeSettings(model);
            var fileGroups = geData.Settings.GetFileGroups();
            var baseFiles = geData.Settings.GetBaseFiles();
            Assert.AreEqual("Minimal", fileGroups[0].Name, "Expected top file group.");
            var fileGroup = geData.Settings.GetFileGroup("ProgramDefinition");
            Assert.IsNotNull(fileGroup);
            Assert.AreEqual("ProgramDefinition", fileGroups[0].Name, "Selected file group expected at top");
            Assert.AreEqual("Minimal", fileGroups[1].Name, "Previous top file group expected second.");
            Assert.AreSame(geData.Settings.FileGroup, fileGroup);
            Assert.AreSame(fileGroup, fileGroups[0]);
            Assert.AreEqual("ProgramDefinition", fileGroup.Name);
            Assert.AreEqual("Definition", geData.Settings.BaseFile.Name);
            Assert.AreEqual("Data/ProgramDefinition.dcb", geData.Settings.FilePath);
            Assert.Contains(geData.Settings.BaseFile, baseFiles);
        }

        [Test(Description = "Access file group properties test.")]
        public void GeSettingsSaveTest()
        {
            var model = PopulateGenSettings();
            GenParameters.SaveToFile(model.GenDataBase, "TestData/Settings.dcb");
        }

        [Test(Description = "Verifies that a base file is created when creating a definition")]
        public void GeDefinitionSaveTest()
        {
            if (File.Exists(@"Test\NewDefinition")) File.Delete(@"Test\NewDefinition.dcb");
            var geData = new GeData();
            var model = PopulateGenSettings();
            geData.Settings = new GeSettings(model);
            var fileGroups = geData.Settings.GetFileGroups();
            var baseFiles = geData.Settings.GetBaseFiles();
            Assert.IsFalse(fileGroups.Contains("NewDefinition"), "Settings already contain NewDefinition");
            Assert.IsFalse(baseFiles.Contains("NewDefinition"), "BaseFiles already contain NewDefinition");
            var baseFile = baseFiles.Find("Definition");
            var fileGroup = model.GenSettingsList[0].AddFileGroup("NewDefinition", "NewDefinition.dcb", "Test", "Definition");
            geData.CreateFile(fileGroup);
            Assert.IsTrue(fileGroups.Contains("NewDefinition"), "Settings already contain NewDefinition");
            Assert.IsTrue(baseFiles.Contains("NewDefinition"), "BaseFiles already contain NewDefinition");
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
