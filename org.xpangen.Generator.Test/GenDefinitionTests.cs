using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Definition;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the use of the Definition class as the container of GenDataDef information
    /// </summary>
    [TestFixture]
    class GenDefinitionTests : GenDataTestsBase
    {
        [TestCase(Description = "Tests the bootstrap construction of the Definition definition.")]
        public void TestDefinitionConstruction()
        {
            var def = DefinitionCreator.Create();
            Assert.AreEqual(3, def.ClassList.Count);
            Assert.AreEqual("Class", def.ClassList[0].Name);
            Assert.AreEqual("SubClass", def.ClassList[1].Name);
            Assert.AreEqual("Property", def.ClassList[2].Name);
            CompareGenDataBase(GenDataBase.DataLoader.LoadData("Data/Definition"), def.GenDataBase);
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
