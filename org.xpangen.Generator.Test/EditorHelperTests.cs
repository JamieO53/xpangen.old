using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Editor.Model;
using NUnit.Framework;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the Editor Helper functionality
    /// </summary>
    [TestFixture]
    public class EditorHelperTests : GenDataTestsBase
    {
        [TestCase(Description = "Verifies that the test settings are set up correctly.")]
        public void SettingsLoadTest()
        {
            var model = PopulateGenSettings();
            var d = model.GenData;
            Assert.AreEqual(1, model.GenSettingsList.Count);
            Assert.AreEqual(4, model.GenSettingsList[0].BaseFileList.Count);
            Assert.AreEqual(6, model.GenSettingsList[0].FileGroupList.Count);
            var fileGroup = model.GenSettingsList[0].FileGroupList[5];
            Assert.AreEqual("GeneratorDefinitionModel", fileGroup.Name);
            d.Last(2);
            Assert.AreEqual("GeneratorDefinitionModel", d.Context[2].GenObject.Attributes[0]);
        }

        [TestCase(Description = "Access file group properties test.")]
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

        [TestCase(Description = "Access file group properties test.")]
        public void GeSettingsSaveTest()
        {
            var model = PopulateGenSettings();
            GenParameters.SaveToFile(model.GenData, "TestData/Settings.dcb");
        }

        private static Root PopulateGenSettings()
        {
            var f = GenData.DataLoader.LoadData("data/GeneratorEditor").AsDef();
            var d = new GenData(f);
            var model = new Root(d) {GenObject = d.Root};
            model.SaveFields();
            var settings = new GenSettings(d) {GenObject = d.CreateObject("", "GenSettings"), HomeDir = "."};
            model.GenSettingsList.Add(settings);
            settings.BaseFileList.Add(new BaseFile(d)
                                          {
                                              GenObject = d.CreateObject("GenSettings", "BaseFile"),
                                              Name = "Minimal",
                                              Title = "The simplest definition required by the generator",
                                              FileName = "Minimal.dcb",
                                              FilePath = "Data",
                                              FileExtension = ".dcb"
                                          });
            settings.BaseFileList.Add(new BaseFile(d)
                                          {
                                              GenObject = d.CreateObject("GenSettings", "BaseFile"),
                                              Name = "Definition",
                                              Title = "The definition required by the editor",
                                              FileName = "Definition.dcb",
                                              FilePath = "Data",
                                              FileExtension = ".dcb"
                                          });
            var baseFile = new BaseFile(d)
                               {
                                   GenObject = d.CreateObject("GenSettings", "BaseFile"),
                                   Name = "ProgramDefinition",
                                   Title = "Defines generator editor data models",
                                   FileName = "ProgramDefinition.dcb",
                                   FilePath = "Data",
                                   FileExtension = ".dcb"
                               };
            baseFile.ProfileList.Add(new Editor.Model.Profile(d)
                                         {
                                             GenObject = d.CreateObject("BaseFile", "Profile"),
                                             Name = "GenProfileModel",
                                             Title = "",
                                             FileName = "GenProfileModel.prf",
                                             FilePath = "Data"
                                         });
            settings.BaseFileList.Add(baseFile);
            settings.BaseFileList.Add(new BaseFile(d)
                                          {
                                              GenObject = d.CreateObject("GenSettings", "BaseFile"),
                                              Name = "GeneratorEditor",
                                              Title = "Defines generator editor settings data",
                                              FileName = "GeneratorEditor.dcb",
                                              FilePath = "Data",
                                              FileExtension = ".dcb"
                                          });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "Minimal",
                                               FileName = "Minimal.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "Basic",
                                               FileName = "Basic.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "Definition",
                                               FileName = "Definition.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "ProgramDefinition",
                                               FileName = "ProgramDefinition.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "GeneratorEditor",
                                               FileName = "GeneratorEditor.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "GeneratorDefinitionModel",
                                               FileName = "GeneratorDefinitionModel.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "ProgramDefinition",
                                               Profile = "GenProfileModel"
                                           });
            d.First(1);
            return model;
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
#pragma warning disable 168
            // Reference to initialize static data
            var loader = new GenDataLoader();
#pragma warning restore 168
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
