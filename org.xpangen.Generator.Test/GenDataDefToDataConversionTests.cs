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
        [Test(Description = "Generator empty definition extract test")]
        public void EmptyGenDefExtractTest()
        {
            var f = new GenDataDef();
            var d = f.AsGenDataBase(); // Creates from a minimal definition, i.e. Root class, Class class, SubClass class, Properties class and FieldFilter class
            new GenAttributes(f, RootClassId);
            Assert.AreEqual(4, d.GenDataDef.Classes.Count);

            var r = d.Root;
            Assert.IsNotNull(r);
            Assert.AreEqual(0, r.SubClass[0].Count);
            Assert.AreEqual(RootClassId, r.ClassId);
            Assert.AreEqual(0, r.Attributes.Count);
        }

        /// <summary>
        /// Tests the extraction of a definition of a single class without properties or subclasses
        /// </summary>
        [Test(Description = "Generator empty class definition extract test")]
        public void EmptyClassGenDefExtractTest()
        {
            var f = new GenDataDef();
            f.AddClass("", "Class");
            var d = f.AsGenDataBase();
            var a = new GenAttributes(d.GenDataDef, ClassClassId);

            var r = d.Root;
            Assert.AreEqual(1, r.SubClass.Count);
            Assert.AreEqual(1, r.SubClass[0].Count);

            var c = GetFirstObject(d);
            Assert.AreEqual(2, c.SubClass.Count);
            Assert.AreEqual(2, c.Attributes.Count);

            var s = GetFirstObjectOfSubClass(c, "SubClass");
            Assert.IsNull(s);
            var p = GetFirstObjectOfSubClass(c, "Property");
            Assert.IsNull(p);
            a.GenObject = c;
            Assert.AreEqual("Class", a.AsString("Name"));
        }

        /// <summary>
        /// Tests the extraction of a definition of a single class with properties
        /// </summary>
        [Test(Description = "Generator class with property definition extract test")]
        public void ClassWithPropertyGenDefExtractTest()
        {
            var f = new GenDataDef();
            f.AddClass("", "Class");
            f.AddClass("Class", "Property");
            var d = f.AsGenDataBase();
            var a = new GenAttributes(d.GenDataDef, SubClassClassId);

            Assert.AreEqual(1, d.Root.SubClass.Count);

            Assert.AreEqual(2, d.Root.SubClass[0].Count);
            
            var c = GetFirstObject(d);
            Assert.AreEqual(2, c.SubClass.Count);
            Assert.AreEqual(2, c.Attributes.Count);
            Assert.AreEqual(ClassClassId, c.ClassId);

            var s = GetFirstObjectOfSubClass(c, "SubClass");
            Assert.AreEqual(1, c.GetSubClass("SubClass").Count);
            a.GenObject = s;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.IsNull(GetFirstObjectOfSubClass(c, "Property"));
        }

        /// <summary>
        /// Tests the extraction of the minimal definition
        /// </summary>
        [Test(Description = "Generator minimal definition extract test")]
        public void MinimalGenDefExtractTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = f.AsGenDataBase();
            ValidateMinimalData(d);
        }

        /// <summary>
        /// Tests the extraction of a definition of a single class with properties
        /// </summary>
        [Test(Description = "Generator class with property definition extract test")]
        public void ReferenceGenDefExtractTest()
        {
            var fChild = SetUpParentChildDef("Child", "Grandchild");
            var fParent = SetUpParentChildReferenceDef("Parent", "Child", "ChildDef", fChild);
            var d = fParent.AsGenDataBase();
            var a = new GenAttributes(fParent, ClassClassId);

            var c = GetFirstObject(d);
            Assert.AreEqual(1, d.Root.SubClass[0].Count);

            a.GenObject = c;
            Assert.AreEqual("Parent", a.AsString("Name"));
            a = new GenAttributes(d.GenDataDef, SubClassClassId) {GenObject = GetFirstObjectOfSubClass(c, "SubClass")};
            Assert.AreEqual("Child", a.AsString("Name"));
            Assert.AreEqual("ChildDef", a.AsString("Reference"));
        }
    }
}
