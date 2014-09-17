using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    [TestFixture]
    public class GenDataPseudoPropertyTests : GenDataTestsBase
    {
        [TestCase(Description="Test First property - property does not exist; first record")]
        public void ContextNoFirstPropertyExistsFirstTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.First");
            Assert.AreEqual("True", d.Context.GetValue(id));
        }

        [TestCase(Description = "Test First property - property does not exist; second record")]
        public void ContextNoFirstPropertyExistsSecondTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            SetUpData(d);
            d.Root.SubClass[0].Add(new GenObject(d.Root, d.Root.SubClass[0], 1));
            d.Context[1].Next();
            var id = f.GetId("Class.First");
            Assert.AreEqual("", d.Context.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is empty")]
        public void ContextEmptyFirstPropertyExistsTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(1, "First");
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.First");
            Assert.AreEqual("", d.Context.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is not empty")]
        public void ContextFirstPropertyExistsWithValueTest()
        {
            var f = GenDataDef.CreateMinimal();
            var idx = f.AddClassInstanceProperty(1, "First");
            var d = new GenData(f);

            SetUpData(d);
            d.Context[1].GenObject.Attributes[idx] = "First value";
            var id = f.GetId("Class.First");
            Assert.AreEqual("First value", d.Context.GetValue(id));
        }

        [TestCase(Description = "Test Reference property - property does not exist and there is no reference")]
        public void ContextNoReferencePropertyExistsNoReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("", d.Context.GetValue(id));
        }

        [TestCase(Description = "Test Reference property - property does not exist and there is a reference")]
        public void ContextNoReferencePropertyExistsWithReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            SetUpData(d);
            d.Context[1].Reference = "Class reference";
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("Class reference", d.Context.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is empty")]
        public void ContextEmptyReferencePropertyExistsTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(1, "Reference");
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("", d.Context.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is not empty")]
        public void ContextReferencePropertyExistsWithValueTest()
        {
            var f = GenDataDef.CreateMinimal();
            var idx = f.AddClassInstanceProperty(1, "Reference");
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            d.Context[1].GenObject.Attributes[idx] = "Reference value";
            Assert.AreEqual("Reference value", d.Context.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is not empty and a reference exists")]
        public void ContextReferencePropertyExistsWithValueAndReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            var idx = f.AddClassInstanceProperty(1, "Reference");
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            d.Context[1].Reference = "Class reference";
            d.Context[1].GenObject.Attributes[idx] = "Reference value";
            Assert.AreEqual("Reference value", d.Context.GetValue(id));
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