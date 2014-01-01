// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
            Assert.AreEqual("", f0.Classes[0].Name);
            Assert.AreEqual(0, f0.Classes[0].Properties.Count);
            Assert.AreEqual(0, f0.Classes[0].SubClasses.Count);
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
            Assert.AreEqual("", f0.Classes[0].Name);
            Assert.AreEqual(0, f0.Classes[0].Properties.Count);
            Assert.AreEqual(1, f0.Classes[0].SubClasses.Count);

            // Defined Class
            Assert.AreEqual(0, f.Classes.IndexOf(""));
            Assert.AreEqual("Class", f.Classes[1].Name);
            Assert.AreEqual(1, f0.Classes[0].SubClasses[0].SubClass.ClassId);
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
