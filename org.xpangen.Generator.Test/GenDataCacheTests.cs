using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Test data loader functionality
    /// </summary>
    [TestFixture]
    public class GenDataCacheTests: GenDataTestsBase
    {
        /// <summary>
        /// Test the static data loader functionality
        /// </summary>
        [TestCase(Description = "Verify that data loaded consecutively is loaded correctly")]
        public void ConsecutiveDataLoadTest()
        {
            var f = new GenDataDef();
            f.AddSubClass("", "Parent");
            f.Classes[f.Classes.IndexOf("Parent")].Properties.Add("Name");
            f.AddSubClass("Parent", "Class", "Definition");

            var d = new GenData(f);
            var minimal = GenData.DataLoader.LoadData("Minimal");
            minimal.Last(1);
            Assert.AreEqual("Property", minimal.Context[1].GenObject.Attributes[0]);
            Assert.AreEqual(1, minimal.Context[3].Count);
            var basic = GenData.DataLoader.LoadData("Basic");
            basic.Last(1);
            Assert.AreEqual("Property", basic.Context[1].GenObject.Attributes[0]);
            Assert.AreEqual(2, basic.Context[3].Count);
            var definition = GenData.DataLoader.LoadData("Definition");
            definition.Last(1);
            Assert.AreEqual("Property", definition.Context[1].GenObject.Attributes[0]);
            Assert.AreEqual(4, definition.Context[3].Count);
            Assert.AreSame(minimal, d.Cache.Internal("definition", "Minimal", minimal));
            Assert.AreSame(basic, d.Cache.Internal("definition", "Basic", basic));
            var newDefinition = d.Cache.Internal("definition", "Definition", definition);
            Assert.AreNotSame(definition, newDefinition);
            Assert.AreSame(minimal, d.Cache.Internal("definition", "Minimal", minimal));
            Assert.AreSame(basic, d.Cache.Internal("definition", "Basic", basic));
            Assert.AreSame(newDefinition, d.Cache.Internal("definition", "Definition", definition));
            Assert.AreSame(minimal, d.Cache["definition", "Minimal"]);
            Assert.AreSame(basic, d.Cache["definition", "Basic"]);
            Assert.AreSame(newDefinition, d.Cache["definition", "Definition"]);
            d.Cache.Merge();

            CreateGenObject(d, "", "Parent", "Minimal");
            ((GenObjectListReference)d.Context[1].GenObject.SubClass[0]).Reference = "Minimal";
            CreateGenObject(d, "", "Parent", "Basic");
            ((GenObjectListReference)d.Context[1].GenObject.SubClass[0]).Reference = "Basic";
            CreateGenObject(d, "", "Parent", "Definition");
            ((GenObjectListReference)d.Context[1].GenObject.SubClass[0]).Reference = "Definition";

            d.First(1);
            Assert.AreEqual("Minimal", d.Context[2].Reference);
            Assert.AreSame(minimal.GenDataBase, d.Context[2].GenObject.GenDataBase);
            d.Last(2);
            Assert.AreEqual("Property", d.Context[2].GenObject.Attributes[0]);
            Assert.AreEqual(1, d.Context[4].Count);
            d.Next(1);
            Assert.AreEqual("Basic", d.Context[2].Reference);
            Assert.AreSame(basic.GenDataBase, d.Context[2].GenObject.GenDataBase);
            d.Last(2);
            Assert.AreEqual("Property", d.Context[2].GenObject.Attributes[0]);
            Assert.AreEqual(2, d.Context[4].Count);
            d.Next(1);
            Assert.AreEqual("Definition", d.Context[2].Reference);
            Assert.AreSame(newDefinition.GenDataBase, d.Context[2].GenObject.GenDataBase);
            d.Last(2);
            Assert.AreEqual("Property", d.Context[2].GenObject.Attributes[0]);
            Assert.AreEqual(4, d.Context[4].Count);
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
