using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Debugging tests - replicate runtime problems
    /// </summary>
    [TestFixture]
    class DebugTests : GenDataTestsBase
    {
        [TestCase(Description = "The DatabaseDefinition data crashes the editor when saved with a minimal change.")]
        public void DatabaseDefinitionSaveTest()
        {
            var data = new GeData();
            data.Settings = data.GetDefaultSettings();
            data.SetFileGroup("DatabaseDefinition");
            var fileGroup = data.Settings.FileGroup;
            Assert.IsNotNull(fileGroup);

            //var def = GenData.DataLoader.LoadData("Definition");
            var f = data.GenDataDef;
            var d = data.GenData;
            d.First(1);
            d.Next(1);
            var c = d.SaveContext(1, null);
            d.First(1);
            c.EstablishContext();
            var o = new GenAttributes(f, c.GenObject.ClassId) { GenObject = c.GenObject };
            Assert.AreEqual("Database schema definition", o.AsString("Title"));
            o.GenObject = d.Context[1].GenObject;
            Assert.AreEqual("Database schema definition", o.AsString("Title"));
        }

        [TestCase(Description = "The GeneratorDefintionModel data crashes the editor when expanding referenced data.")]
        public void ReferenceDataSaveTest()
        {
            var data = new GeData();
            data.Settings = data.GetDefaultSettings();
            data.SetFileGroup("GeneratorDefinitionModel");

            var d = data.GenData;
            d.First(1);
            d.Next(2);
            d.Next(2);
            Assert.AreEqual("GeneratorEditor", d.Context[2].GenObject.Attributes[0]);
            Assert.AreEqual("GenSettings", d.Context[3].GenObject.Attributes[0]);
            var c = d.SaveContext(3);
            d.First(1);
            Assert.AreNotEqual("GeneratorEditor", d.Context[2].GenObject.Attributes[0]);
            c.EstablishContext();
            Assert.AreEqual("GeneratorEditor", d.Context[2].GenObject.Attributes[0]);
            Assert.AreEqual("GenSettings", d.Context[3].GenObject.Attributes[0]);
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
