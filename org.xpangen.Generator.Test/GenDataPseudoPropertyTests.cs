using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    [TestFixture]
    public class GenDataPseudoPropertyTests : GenDataTestsBase
    {
        [Test(Description="Test First property - property does not exist; first record")]
        public void ContextNoFirstPropertyExistsFirstTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenDataBase(f);

            SetUpData(d);
            var genObject = GenObject.GetContext(d.Root, "Class");
            var id = f.GetId("Class.First");
            Assert.AreEqual("True", genObject.GetValue(id));
        }

        [Test(Description = "Test First property - property does not exist; second record")]
        public void ContextNoFirstPropertyExistsSecondTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenDataBase(f);

            SetUpData(d);
            CreateGenObject(d.Root, "Class", "Class0");
            var genObject = GetNextObjectInSubClass(GenObject.GetContext(d.Root, "Class"));
            var id = f.GetId("Class.First");
            Assert.AreEqual("", genObject.GetValue(id));
        }

        [Test(Description = "Test First property - property exists and is empty")]
        public void ContextEmptyFirstPropertyExistsTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(1, "First");
            var d = new GenDataBase(f);

            SetUpData(d);
            var genObject = GenObject.GetContext(d.Root, "Class");
            var id = f.GetId("Class.First");
            Assert.AreEqual("", genObject.GetValue(id));
        }

        [Test(Description = "Test First property - property exists and is not empty")]
        public void ContextFirstPropertyExistsWithValueTest()
        {
            var f = GenDataDef.CreateMinimal();
            var idx = f.AddClassInstanceProperty(1, "First");
            var d = new GenDataBase(f);

            SetUpData(d);
            var genObject = GenObject.GetContext(d.Root, "Class");
            genObject.Attributes[idx] = "First value";
            var id = f.GetId("Class.First");
            Assert.AreEqual("First value", genObject.GetValue(id));
        }

        [Test(Description = "Test Reference property - property does not exist and there is no reference")]
        public void ContextNoReferencePropertyExistsNoReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenDataBase(f);

            SetUpData(d);
            var genObject = GenObject.GetContext(d.Root, "Class");
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("", genObject.GetValue(id));
        }

        [Test(Description = "Test Reference property - property does not exist and there is a reference")]
        public void ContextNoReferencePropertyExistsWithReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenDataBase(f);

            SetUpData(d);
            var genObject = GenObject.GetContext(d.Root, "Class");
            genObject.ParentSubClass.Reference = "Class reference";
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("Class reference", genObject.GetValue(id));
        }

        [Test(Description = "Test First property - property exists and is empty")]
        public void ContextEmptyReferencePropertyExistsTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.AddClassInstanceProperty(1, "Reference");
            var d = new GenDataBase(f);

            SetUpData(d);
            var genObject = GenObject.GetContext(d.Root, "Class");
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("", genObject.GetValue(id));
        }

        [Test(Description = "Test First property - property exists and is not empty")]
        public void ContextReferencePropertyExistsWithValueTest()
        {
            var f = GenDataDef.CreateMinimal();
            var idx = f.AddClassInstanceProperty(1, "Reference");
            var d = new GenDataBase(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            var genObject = GenObject.GetContext(d.Root, "Class");
            genObject.Attributes[idx] = "Reference value";
            Assert.AreEqual("Reference value", genObject.GetValue(id));
        }

        [Test(Description = "Test First property - property exists and is not empty and a reference exists")]
        public void ContextReferencePropertyExistsWithValueAndReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            var idx = f.AddClassInstanceProperty(1, "Reference");
            var d = new GenDataBase(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            var genObject = GenObject.GetContext(d.Root, "Class");
            genObject.ParentSubClass.Reference = "Class reference";
            genObject.Attributes[idx] = "Reference value";
            Assert.AreEqual("Reference value", genObject.GetValue(id));
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            GenDataLoader.Register();
        }

        /// <summary>
        /// Tear down the Generator data definition tests
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {

        }
    }
}