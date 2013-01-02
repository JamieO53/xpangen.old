using NUnit.Framework;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator data to generator data definition conversion functionality
    /// </summary>
    [TestFixture]
    class GenDataToDefConversionTests : GenDataTestsBase
    {
        /// <summary>
        /// Tests the extraction of an empty definition
        /// </summary>
        [TestCase(Description = "Generator empty definition extract test")]
        public void EmptyGenDataAsDefTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            var f0 = d.AsDef();
            
            Assert.AreEqual(1, f0.Classes.Count); // Root class only
            Assert.AreEqual("", f0.Classes[0]);
            Assert.AreEqual(0, f0.Properties[0].Count);
            Assert.AreEqual(0, f0.SubClasses[0].Count);
        }

        /// <summary>
        /// Tests the extraction of an empty definition
        /// </summary>
        [TestCase(Description = "Generator definition with single class extract test")]
        public void EmptyClassGenDataAsDefTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            CreateClass(d, "Class");
            var f0 = d.AsDef();
            Assert.AreEqual(2, f0.Classes.Count);
            
            // Root class
            Assert.AreEqual("", f0.Classes[0]);
            Assert.AreEqual(0, f0.Properties[0].Count);
            Assert.AreEqual(1, f0.SubClasses[0].Count);

            // Defined Class
            Assert.AreEqual(0, f.Classes.IndexOf(""));
            Assert.AreEqual("Class", f.Classes[1]);
            Assert.AreEqual(1, f0.SubClasses[0][0]);
        }

        /// <summary>
        /// Tests the extraction of an minimal definition
        /// </summary>
        [TestCase(Description = "Generator minimal definition to data extract test")]
        public void MinimalGenDataAsDefTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = f.AsGenData();
            var f0 = d.AsDef();
            VerifyAsDef(f0);
        }
        
    }
}
