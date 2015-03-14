// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using System.Reflection;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

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
        [Test(Description = "Generator empty definition extract test")]
        public void EmptyGenDataAsDefTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenDataBase(f);
            var f0 = d.AsDef();
            
            Assert.AreEqual(1, f0.Classes.Count); // Root class only
            Assert.AreEqual("", f0.GetClassName(0));
            Assert.AreEqual(0, f0.GetClassProperties(0).Count);
            Assert.AreEqual(0, f0.GetClassSubClasses(0).Count);
        }

        /// <summary>
        /// Tests the extraction of an empty definition
        /// </summary>
        [Test(Description = "Generator definition with single class extract test")]
        public void EmptyClassGenDataAsDefTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenDataBase(f);
            CreateClass(d, "Class");
            var f0 = d.AsDef();
            Assert.AreEqual(2, f0.Classes.Count);
            
            // Root class
            Assert.AreEqual("", f0.GetClassName(0));
            Assert.AreEqual(0, f0.GetClassProperties(0).Count);
            Assert.AreEqual(1, f0.GetClassSubClasses(0).Count);

            // Defined Class
            Assert.AreEqual(0, f.GetClassId(""));
            Assert.AreEqual("Class", f.GetClassName(1));
            Assert.AreEqual(1, f0.GetClassSubClasses(0)[0].SubClass.ClassId);
        }

        /// <summary>
        /// Tests the extraction of an minimal definition
        /// </summary>
        [Test(Description = "Generator minimal definition to data extract test")]
        public void MinimalGenDataAsDefTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = f.AsGenDataBase();
            var f0 = d.AsDef();
            VerifyAsDef(f0);
        }
        
        /// <summary>
        /// Tests the extraction of an minimal definition
        /// </summary>
        [Test(Description = "Generator definition with a reference to data extract test")]
        public void ReferenceGenDataAsDefTest()
        {
            EnsureFileExists("ChildDef.dcb", ".");
            EnsureFileExists("GrandchildDef.dcb", ".");
            var fGrandchild = SetUpParentChildDef("Grandchild", "Greatgrandchild");
            fGrandchild.DefinitionName = "GrandchildDef";
            var fChild = SetUpParentChildReferenceDef("Child", "Grandchild", "GrandchildDef", fGrandchild);
            fChild.DefinitionName = "ChildDef";
            var fParent = SetUpParentChildReferenceDef("Parent", "Child", "ChildDef", fChild);
            var f = fParent.AsGenDataBase().AsDef();
            f.DefinitionName = "Parent";
            CompareGenDataDef(fParent, f, "Parent");
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
