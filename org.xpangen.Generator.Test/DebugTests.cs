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

            //var def = GenData.DataLoader.LoadData("Definition");
            var f = data.GenDataDef;
            var d = data.GenData;
            var o = new GenAttributes(f) {GenObject = d.Context[1].GenObject};
            o.SetString("Title", "Database definition ");
            o.SaveFields();
            o.SetString("Title", "Database definition");
            o.SaveFields();
            data.SaveFile(fileGroup);
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
