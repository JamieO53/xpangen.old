using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data.Definition;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests that Generator Data Models can be manipulated properly
    /// </summary>
    [TestFixture]
    public class DataModelTests
    {
        [TestCase(Description="Verifies that the Definition Data Model is loaded correctly.")]
        public void DefinitionDataModelLoadTest()
        {
            GenParameters data;
            using (Stream stream = new FileStream(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Data\definition.dcb") , FileMode.Open, FileAccess.Read))
                data = new GenParameters(stream) { DataName = "definition" };
            var model = new Definition(data) {GenObject = data.Root};
            Assert.AreEqual(3, model.ClassList.Count);
            Assert.AreEqual("Class", model.ClassList[0].Name);
            Assert.AreEqual("SubClass", model.ClassList[1].Name);
            Assert.AreEqual("Property", model.ClassList[2].Name);
            Assert.AreSame(model.ClassList[0], model.ClassList.Find("Class"));
            Assert.IsNotEmpty(model.ClassList[0].Title);
            Assert.IsNotEmpty(model.ClassList[1].Title);
            Assert.IsNotEmpty(model.ClassList[2].Title);
            Assert.AreEqual(2, model.ClassList[0].SubClassList.Count);
            Assert.AreEqual("SubClass", model.ClassList[0].SubClassList[0].Name);
            Assert.AreEqual("Property", model.ClassList[0].SubClassList[1].Name);
            Assert.AreEqual(0, model.ClassList[1].SubClassList.Count);
            Assert.AreEqual(0, model.ClassList[2].SubClassList.Count);
            Assert.AreEqual(3, model.ClassList[0].PropertyList.Count);
            Assert.AreEqual(3, model.ClassList[1].PropertyList.Count);
            Assert.AreEqual(7, model.ClassList[2].PropertyList.Count);
            Assert.AreEqual("Name", model.ClassList[0].PropertyList[0].Name);
            Assert.AreEqual("Title", model.ClassList[0].PropertyList[1].Name);
            Assert.AreEqual("Name", model.ClassList[1].PropertyList[0].Name);
            Assert.AreEqual("Reference", model.ClassList[1].PropertyList[1].Name);
            Assert.AreEqual("Name", model.ClassList[2].PropertyList[0].Name);
            Assert.AreEqual("Title", model.ClassList[2].PropertyList[1].Name);
            Assert.AreEqual("DataType", model.ClassList[2].PropertyList[2].Name);
            Assert.AreEqual("Default", model.ClassList[2].PropertyList[3].Name);
            Assert.AreEqual("LookupType", model.ClassList[2].PropertyList[4].Name);
            Assert.AreEqual("LookupDependence", model.ClassList[2].PropertyList[5].Name);
            Assert.AreEqual("LookupTable", model.ClassList[2].PropertyList[6].Name);
        }
    }
}
