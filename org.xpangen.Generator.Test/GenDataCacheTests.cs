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
            f.AddClassInstanceProperty(f.GetClassId("Parent"), "Name");
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
            Assert.AreEqual(7, definition.Context[3].Count);
            Assert.AreSame(minimal, d.Cache.Internal("definition", "Minimal", minimal));
            Assert.AreSame(basic, d.Cache.Internal("definition", "Basic", basic));
            var newDefinition = d.Cache.Internal("definition", "Definition", definition);
            Assert.AreNotSame(definition, newDefinition);
            Assert.AreSame(minimal, d.Cache.Internal("definition", "Minimal", minimal));
            Assert.AreSame(basic, d.Cache.Internal("definition", "Basic", basic));
            Assert.AreSame(newDefinition, d.Cache.Internal("definition", "Definition", definition));
            Assert.AreSame(minimal, d.Cache["Minimal"]);
            Assert.AreSame(basic, d.Cache["Basic"]);
            Assert.AreSame(newDefinition, d.Cache["Definition"]);
            d.Cache.Merge();

            CreateGenObject(d, "", "Parent", "Minimal");
            ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Minimal";
            CreateGenObject(d, "", "Parent", "Basic");
            ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Basic";
            CreateGenObject(d, "", "Parent", "Definition");
            ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Definition";

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
            Assert.AreEqual(7, d.Context[4].Count);
            d.First(0);
        }

        [TestCase(Description = "Test for repeated navigation of data with references.")]
        public void DataNavigationTest()
        {
            var f = new GenDataDef();
            f.AddSubClass("", "Parent");
            f.AddClassInstanceProperty(f.GetClassId("Parent"), "Name");
            f.AddSubClass("Parent", "Class", "Definition");

            var d = new GenData(f);

            CreateGenObject(d, "", "Parent", "Minimal");
            ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Minimal";
            CreateGenObject(d, "", "Parent", "Basic");
            ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Basic";
            CreateGenObject(d, "", "Parent", "Definition");
            ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Definition";
            //d.Cache.Merge();

            //var minimal = d.Cache["definition", "Minimal"];
            //var basic = d.Cache["definition", "Basic"];
            //var newDefinition = d.Cache["definition", "Definition"];

            var d1 = d.DuplicateContext();
            Navigate(d1, 0);
            Navigate(d1, 0);
        }

        //[TestCase(Description = "Test for restoring context with references.")]
        //public void DataRestoreContextTest()
        //{
        //    var f = new GenDataDef();
        //    f.AddSubClass("", "Parent");
        //    f.Classes[f.GetClassId("Parent")].AddInstanceProperty("Name");
        //    f.AddSubClass("Parent", "Class", "Definition");

        //    var d = new GenData(f);

        //    CreateGenObject(d, "", "Parent", "Minimal");
        //    ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Minimal";
        //    CreateGenObject(d, "", "Parent", "Basic");
        //    ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Basic";
        //    CreateGenObject(d, "", "Parent", "Definition");
        //    ((SubClassReference)d.Context[1].GenObject.SubClass[0]).Reference = "Definition";
            
        //    d.First(1);
        //    var o = d.Context[4].GenObject;
        //    var sc = d.SaveContext(4);
        //    Assert.AreEqual("minimal", o.GenDataBase.ToString());
        //    Assert.AreEqual("Minimal", d.Context[4].Reference);
        //    Assert.AreEqual("Minimal", d.Context[3].Reference);
        //    Assert.AreEqual("Minimal", d.Context[2].Reference);
        //    Assert.AreEqual("", d.Context[1].Reference);
        //    d.Last(1);
        //    Assert.AreEqual("definition", d.Context[4].GenObject.GenDataBase.ToString());
        //    Assert.AreEqual("Definition", d.Context[4].Reference);
        //    Assert.AreEqual("Definition", d.Context[3].Reference);
        //    Assert.AreEqual("Definition", d.Context[2].Reference);
        //    Assert.AreEqual("", d.Context[1].Reference);
        //    sc.EstablishContext();
        //    Assert.AreSame(o, d.Context[4].GenObject);
        //    Assert.AreEqual("Minimal", d.Context[4].Reference);
        //    Assert.AreEqual("Minimal", d.Context[3].Reference);
        //    Assert.AreEqual("Minimal", d.Context[2].Reference);
        //    Assert.AreEqual("", d.Context[1].Reference);
        //}

        private static void Navigate(GenData d, int classId)
        {
            d.First(classId);
            while (!d.Eol(classId))
            {
                foreach (GenDataDefSubClass subClass in d.Context[classId].DefClass.SubClasses)
                {
                    Navigate(d, subClass.SubClass.ClassId);
                }
                d.Next(classId);
            }
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
