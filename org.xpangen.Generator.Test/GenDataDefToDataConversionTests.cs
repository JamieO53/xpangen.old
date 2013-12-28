// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator data definition to generator data conversion functionality
    /// </summary>
    [TestFixture]
    public class GenDataDefToDataConversionTests : GenDataTestsBase
    {
        /// <summary>
        /// Tests the extraction of an empty definition
        /// </summary>
        [TestCase(Description = "Generator empty definition extract test")]
        public void EmptyGenDefExtractTest()
        {
            var f = new GenDataDef();
            var d = f.AsGenData(); // Creates from a minimal definition, i.e. Root class, Class class, SubClass class, Properties class and FieldFilter class
            var a = new GenAttributes(f);
            Assert.AreEqual(5, d.Context.Count);
            
            Assert.IsFalse(d.Eol(RootClassId));
            Assert.IsTrue(d.Eol(ClassClassId));
            Assert.IsTrue(d.Eol(PropertyClassId));
            Assert.IsTrue(d.Eol(SubClassClassId));
           
            Assert.AreEqual(RootClassId, d.Context[RootClassId].ClassId);
            Assert.IsNull(d.Context[ClassClassId].Context);
            Assert.IsNull(d.Context[SubClassClassId]);
            Assert.IsNull(d.Context[PropertyClassId]);
            Assert.IsNull(d.Context[FieldFilterClassId]);

            Assert.IsTrue(d.Context[RootClassId].IsFirst());
            a.GenObject = d.Context[RootClassId].Context;
            Assert.AreEqual(0, d.Context[RootClassId].Context.Attributes.Count);
        }

        /// <summary>
        /// Tests the extraction of a definition of a single class without properties or subclasses
        /// </summary>
        [TestCase(Description = "Generator empty class definition extract test")]
        public void EmptyClassGenDefExtractTest()
        {
            var f = new GenDataDef();
            f.AddClass("", "Class");
            var d = f.AsGenData();
            var a = new GenAttributes(f);

            Assert.AreEqual(1, d.Context[RootClassId].Context.SubClass.Count);
            d.First(ClassClassId);
            Assert.AreEqual(1, d.Context[ClassClassId].Count);
            Assert.AreEqual(2, d.Context[ClassClassId].Context.SubClass.Count);
            Assert.AreEqual(1, d.Context[ClassClassId].Context.Attributes.Count);

            Assert.IsNull(d.Context[SubClassClassId].Context);
            Assert.IsNull(d.Context[PropertyClassId].Context);
            a.GenObject = d.Context[ClassClassId].Context;
            Assert.AreEqual("Class", a.AsString("Name"));
        }

        /// <summary>
        /// Tests the extraction of a definition of a single class with properties
        /// </summary>
        [TestCase(Description = "Generator class with property definition extract test")]
        public void ClassWithPropertyGenDefExtractTest()
        {
            var f = new GenDataDef();
            f.AddClass("", "Class");
            f.AddClass("Class", "Property");
            var d = f.AsGenData();
            var a = new GenAttributes(f);

            Assert.AreEqual(1, d.Context[RootClassId].Context.SubClass.Count);
            d.First(ClassClassId);
            Assert.AreEqual(2, d.Context[ClassClassId].Count);
            Assert.AreEqual(2, d.Context[ClassClassId].Context.SubClass.Count);
            Assert.AreEqual(1, d.Context[ClassClassId].Context.Attributes.Count);
            Assert.AreEqual(ClassClassId, d.Context[ClassClassId].Context.ClassId);

            Assert.AreEqual(1, d.Context[SubClassClassId].Count);
            a.GenObject = d.Context[SubClassClassId].Context;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.IsNull(d.Context[PropertyClassId].Context);
        }

        /// <summary>
        /// Tests the extraction of the minimal definition
        /// </summary>
        [TestCase(Description = "Generator minimal definition extract test")]
        public void MinimalGenDefExtractTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = f.AsGenData();
            ValidateMinimalData(d);
        }
    }
}
