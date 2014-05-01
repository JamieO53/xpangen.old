// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator data functionality
    /// </summary>
	[TestFixture]
    public class GenDataTests : GenDataTestsBase
    {
        /// <summary>
        /// Tests the general functionality of GenData
        /// </summary>
        [TestCase(Description="Generator data tests")]
        public void GenDataTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            CreateClass(d, "Class");

            var id = f.GetId("Class.Name");
            Assert.AreEqual("Class", d.GetValue(id));

            id = f.GetId("Property.Name");
            Assert.AreEqual("Name", d.GetValue(id));

            CreateClass(d, "SubClass");

            id = f.GetId("Class.Name");
            Assert.AreEqual("SubClass", d.GetValue(id));

            CreateProperty(d, "Reference");

            id = f.GetId("Property.Name");
            Assert.AreEqual("Reference", d.GetValue(id));

            CreateClass(d, "Property");

            id = f.GetId("Class.Name");
            Assert.AreEqual("Property", d.GetValue(id));

            id = f.GetId("Property.Name");
            Assert.AreEqual("Name", d.GetValue(id));

            CreateClass(d, "FieldFilter");

            id = f.GetId("Class.Name");
            Assert.AreEqual("FieldFilter", d.GetValue(id));

            CreateProperty(d, "Operand");

            d.Context[ClassClassId].First();
            CreateSubClass(d, "SubClass");

            id = f.GetId("SubClass.Name");
            Assert.AreEqual("SubClass", d.GetValue(id));

            CreateSubClass(d, "Property");
            
            id = f.GetId("SubClass.Name");
            Assert.AreEqual("Property", d.GetValue(id));

            ValidateMinimalData(d);
        }

        /// <summary>
        /// Tests the generator data subclass functionality
        /// </summary>
        [TestCase(Description="Generator data subclass tests")]
        public void GenSubClassTests()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            var a = new GenAttributes(f);
            var o = d.CreateObject("", "Class");
            a.GenObject = o;
            a.SetString("Name", "Class");
            a.SaveFields();

            var i = f.IndexOfSubClass(f.Classes.IndexOf("Class"), f.Classes.IndexOf("SubClass"));
            var s = new GenObjectList(o.SubClass[i], d.GenDataBase);
            o = s.CreateObject();
            a.GenObject = o;
            a.SetString("Name", "SubClass");
            a.SaveFields();
            d.Context[f.Classes.IndexOf("SubClass")].Last();

            s.Last();
        }

        /// <summary>
        /// Ensure that the 'self' reference is automatically cached.
        /// </summary>
        [TestCase(Description = "Ensure that the 'self' reference is automatically cached")]
        public void ReferenceCacheSelfTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            var self = d.Cache["Minimal", "self"];
            Assert.AreSame(d, self);
        }

        /// <summary>
        /// Ensure that simple named references are correctly cached
        /// </summary>
        [TestCase(Description = "Ensure that the 'self' simple named reference cannot be cached internally")]
        [ExpectedException(exceptionType: typeof(ArgumentException), ExpectedMessage = "The 'self' generator data cannot be added explicitly to the cache\r\nParameter name: name")]
        public void ReferenceCacheSelfLocalPathTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            var d0 = GenDataDef.CreateMinimal().AsGenData();
            d.Cache.Internal("Minimal", "self", d0);
        }

        [TestCase(Description = "Ensure that the base data references and their definitions can be retrieved.")]
        public void GetGenDataBaseReferencesTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            d.GenDataBase.References.Add("Data", "Def");
            d.GenDataBase.References.Add("Data", "Def");
            var references = d.GenDataBase.References.ReferenceList;
            Assert.AreEqual(3, references.Count);
            Assert.AreEqual("data", references[0].Data);
            Assert.AreEqual("def", references[0].Definition);
            Assert.AreEqual("def", references[1].Data);
            Assert.AreEqual("minimal", references[1].Definition);
            Assert.AreEqual("minimal", references[2].Data);
            Assert.AreEqual("minimal", references[2].Definition);
        }

        [TestCase(Description = "Ensure that the base data references and their definitions are correctly loaded.")]
        public void GetGenDataBaseReferencesLoadTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            d.GenDataBase.References.Add("Data/Definition", "Minimal");
            d.LoadCache();
            Assert.That(d.Cache.Contains("minimal"));
            Assert.That(d.Cache.Contains("data\\definition"));
        }

        /// <summary>
        /// Ensure that simple named references are correctly cached
        /// </summary>
        [TestCase(Description = "Ensure that simple named references are correctly cached")]
        public void ReferenceCacheLocalPathTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            var d0 = GenDataDef.CreateMinimal().AsGenData();
            var d1 = d0.DuplicateContext();
            var lookup0 = d.Cache.Internal("Minimal", "d0", d0);
            var lookup1 = d.Cache.Internal("Minimal", "d1", d1);
            Assert.AreSame(d0, lookup0);
            Assert.AreSame(d1, lookup1);
            lookup0 = d.Cache["Minimal", "d0"];
            lookup1 = d.Cache["Minimal", "d1"];
            Assert.AreSame(d0, lookup0);
            Assert.AreSame(d1, lookup1);
            Assert.AreNotSame(lookup1, lookup0);
        }

        /// <summary>
        /// Ensure that simple named references are correctly cached
        /// </summary>
        [TestCase(Description = "Ensure that simple named references are correctly cached")]
        public void ReferenceCacheFilePathTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            d.GenDataBase.References.Add("Data/Minimal", "Data/Minimal");
            d.GenDataBase.References.Add("Data/Definition", "Data/Minimal");
            d.LoadCache();
            var minimal = d.Cache["Data/Minimal", "Data/Minimal"];
            var definition = d.Cache["Data/Minimal", "Data/Definition"];
            Assert.AreNotSame(minimal, definition);
        }

        [TestCase(Description = "Verify that the SetUpParentChildData method works as expected")]
        public void VerifySetUpParentChildDataMethod()
        {
            var d = SetUpParentChildData("Parent", "Child", "Child");
            var f = d.GenDataDef;
            Assert.AreEqual(3, f.Classes.Count);
            Assert.AreEqual("Parent", f.Classes[1].Name);
            Assert.AreEqual("Child", f.Classes[2].Name);
            Assert.AreEqual(1, f.Classes[1].SubClasses.Count);
            Assert.AreEqual(2, f.Classes[1].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual("", f.Classes[1].SubClasses[0].Reference);
            Assert.AreEqual("Parent", d.Context[1].GenObject.Attributes[0]);
            Assert.AreEqual("Child", d.Context[2].GenObject.Attributes[0]);
        }

        [TestCase(Description = "Verify that the SetUpParentChildReferenceData method works as expected")]
        public void VerifySetUpParentChildReferenceDataMethod()
        {
            var dataChild = SetUpParentChildData("Child", "Grandchild", "Grandchild");
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "Child", "Child", dataChild);
            Assert.AreEqual(1, dataParent.Root.SubClass.Count);
            Assert.AreEqual(4, dataParent.Context.Count);
            Assert.AreEqual("Parent", dataParent.Context[1].GenObject.Attributes[0]);
            Assert.That(dataParent.Cache.Contains("Child"));
            Assert.AreEqual(1, dataParent.Context[1].GenObject.SubClass.Count);
            Assert.IsFalse(dataParent.Eol(2));
            Assert.AreEqual(2, dataParent.Context[2].ClassId);
            Assert.AreEqual("Child", dataParent.Context[2].GenObject.Attributes[0]);
            Assert.AreEqual(1, dataParent.Context[2].DefClass.RefClassId);
            Assert.AreEqual("Grandchild", dataChild.Context[2].GenObject.Attributes[0]);
            Assert.AreEqual("Grandchild", dataParent.Context[3].GenObject.Attributes[0]);
        }

        [TestCase(Description = "Verify that the SetUpParentChildReferenceData method sets up the data definitions as expected")]
        public void VerifyParentChildReferenceDataDef()
        {
            var dataChild = SetUpParentChildData("Child", "Grandchild", "Grandchild");
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "Child", "Child", dataChild);
            var p = dataParent.GenDataDef.CreateProfile();
            Assert.AreEqual(ReferenceGenDataSaveProfile, p);
        }

        [TestCase(Description = "Verify that data context works as expected with reference data")]
        public void ContextWithReferenceTests()
        {
            var dataChild = SetUpParentChildData("Child", "Grandchild", "Grandchild");
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "Child", "Child", dataChild);
            dataParent.First(1);
            for (var i = 0; i < dataParent.GenDataDef.Classes.Count; i++ )
            {
                Assert.IsNotNull(dataParent.Context[i].GenObject, i + ": " + dataParent.Context[i].DefClass);
                Assert.AreEqual(dataParent.Context[i].RefClassId, dataParent.Context[i].GenObject.ClassId);
            }
        }

        [TestCase(Description = "Verify that duplicated data context works as expected with reference data")]
        public void DuplicatedContextWithReferenceTests()
        {
            var f = GenData.DataLoader.LoadData("ProgramDefinition").AsDef();
            var d = GenData.DataLoader.LoadData(f, "GeneratorDefinitionModel");
            d.Context[2].MoveItem(ListMove.Down, 0);
            d.First(2); d.Next(2);
            Assert.AreEqual("Definition", d.Context[2].GenObject.SubClass[0].Reference);
            d.Next(5);
            Assert.AreEqual("Class", d.Context[3].GenObject.Attributes[0]);
            Assert.AreEqual("Title", d.Context[5].GenObject.Attributes[0]);
            var duplicate = d.DuplicateContext();
            duplicate.Next(2);
            duplicate.Prior(2);
            duplicate.Next(3);
            duplicate.Next(5);
            Assert.AreEqual("SubClass", duplicate.Context[3].GenObject.Attributes[0]);
            Assert.AreEqual("Reference", duplicate.Context[5].GenObject.Attributes[0]);
            d.First(3);
            d.Next(5);
            Assert.AreEqual("Class", d.Context[3].GenObject.Attributes[0]);
            Assert.AreEqual("Title", d.Context[5].GenObject.Attributes[0]);

        }

        [TestCase(Description = "Verify that the SetUpParentChildReferenceData method works as expected")]
        public void VerifyNestedSetUpParentChildReferenceDataMethod()
        {
            var dataGrandchildhild = SetUpParentChildData("Grandchild", "Greatgrandchild", "Greatgrandchild");
            var dataChild = SetUpParentChildReferenceData("Child", "Grandchild", "GrandchildDef", "GrandChild", dataGrandchildhild);
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "ChildDef", "Child", dataChild);
            Assert.AreEqual(1, dataParent.Root.SubClass.Count);
            Assert.AreEqual(5, dataParent.Context.Count);
            Assert.AreEqual("Parent", dataParent.Context[1].GenObject.Attributes[0]);
            Assert.That(dataParent.Cache.Contains("Child"));
            Assert.AreEqual(1, dataParent.Context[1].GenObject.SubClass.Count);
            Assert.IsFalse(dataParent.Eol(2));
            Assert.AreEqual(2, dataParent.Context[2].ClassId);
            Assert.AreEqual("Child", dataParent.Context[2].GenObject.Attributes[0]);
            Assert.AreEqual(3, dataParent.Context[3].ClassId);
            Assert.AreEqual("Grandchild", dataParent.Context[3].GenObject.Attributes[0]);
            Assert.AreEqual(2, dataParent.Context[3].DefClass.RefClassId);
        }

        /// <summary>
        /// Tests the attribute functionality of generator classes
        /// </summary>
        [TestCase(Description = "Generator Attribute Property tests")]
        public void GenAttributePropertyTests()
        {
            var f = new GenDataDef();
            var classId = f.AddClass("", "Class");
            f.Classes[classId].Properties.Add("Prop1");
            f.Classes[classId].Properties.Add("Prop2");
            var d = new GenData(f);
            var o = d.CreateObject("", "Class");
            var a = new GenAttributes(f) {GenObject = o};
            a.SetString("Prop1", "Prop1");
            a.SetString("Prop2", "Prop2");
            a.SaveFields();
            Assert.AreEqual(2, f.Classes[classId].Properties.Count);
        }

        /// <summary>
        /// Tests the attribute functionality of generator classes
        /// </summary>
        [TestCase(Description="Generator Attribute tests")]
        public void GenAttributeTests()
        {
            var f = GenDataDef.CreateMinimal();
            var a = new GenAttributes(f);
            var d = new GenData(f);
            var o = d.CreateObject("", "Class");
            a.GenObject = o;
            a.SetString("Name", "Class");
            a.SaveFields();

            o = d.CreateObject("Class", "SubClass");
            a.GenObject = o;
            a.SetString("Name", "SubClass");
            a.SaveFields();

            VerifyDataCreation(d);
        }

        /// <summary>
        /// Tests the generator data context functionality
        /// </summary>
        [TestCase(Description="Generator context tests")]
        public void GenContextTests()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            SetUpData(d);
            d.First(ClassClassId);
            d.Last(SubClassClassId);
            Assert.AreEqual("Property", d.Context[SubClassClassId].GenObject.Attributes[0]);
            var c = d.DuplicateContext();
            Assert.AreEqual("Property", c.Context[SubClassClassId].GenObject.Attributes[0]);
            c.Context[SubClassClassId].First();
            Assert.AreEqual("SubClass", c.Context[SubClassClassId].GenObject.Attributes[0]);
            Assert.AreEqual("Property", d.Context[SubClassClassId].GenObject.Attributes[0]);
        }

        [TestCase(Description="Test First property - property does not exist; first record")]
        public void ContextNoFirstPropertyExistsFirstTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.First");
            Assert.AreEqual("True", d.GetValue(id));
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
            Assert.AreEqual("", d.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is empty")]
        public void ContextEmptyFirstPropertyExistsTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.Classes[1].Properties.Add("First");
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.First");
            Assert.AreEqual("", d.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is not empty")]
        public void ContextFirstPropertyExistsWithValueTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.Classes[1].Properties.Add("First");
            var d = new GenData(f);

            SetUpData(d);
            d.Context[1].GenObject.Attributes[1] = "First value";
            var id = f.GetId("Class.First");
            Assert.AreEqual("First value", d.GetValue(id));
        }

        [TestCase(Description = "Test Reference property - property does not exist and there is no reference")]
        public void ContextNoReferencePropertyExistsNoReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("", d.GetValue(id));
        }

        [TestCase(Description = "Test Reference property - property does not exist and there is a reference")]
        public void ContextNoReferencePropertyExistsWithReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            SetUpData(d);
            d.Context[1].Reference = "Class reference";
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("Class reference", d.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is empty")]
        public void ContextEmptyReferencePropertyExistsTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.Classes[1].Properties.Add("Reference");
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            Assert.AreEqual("", d.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is not empty")]
        public void ContextReferencePropertyExistsWithValueTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.Classes[1].Properties.Add("Reference");
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            d.Context[1].GenObject.Attributes[1] = "Reference value";
            Assert.AreEqual("Reference value", d.GetValue(id));
        }

        [TestCase(Description = "Test First property - property exists and is not empty and a reference exists")]
        public void ContextReferencePropertyExistsWithValueAndReferenceTest()
        {
            var f = GenDataDef.CreateMinimal();
            f.Classes[1].Properties.Add("Reference");
            var d = new GenData(f);

            SetUpData(d);
            var id = f.GetId("Class.Reference");
            d.Context[1].Reference = "Class reference";
            d.Context[1].GenObject.Attributes[1] = "Reference value";
            Assert.AreEqual("Reference value", d.GetValue(id));
        }

        /// <summary>
        /// Tests the subclass reordering functionality
        /// </summary>
        [TestCase(Description="Generator data reorder tests")]
        public void GenDataReorderTests()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            CreateNamedClass(d, "", "Class", "Class");
            CreateNamedClass(d, "Class", "SubClass", "SubClass1");
            CreateNamedClass(d, "Class", "SubClass", "SubClass2");
            CreateNamedClass(d, "Class", "SubClass", "SubClass3");
            
            // Verify initial subclass order - Index is last added item
            CheckOrder(d, 2, "123", "Verify initial subclass order");
            // Make and verify moves
            MoveItem(d, ListMove.Up, 2, 1, "132", "Move last subclass up one place");
            MoveItem(d, ListMove.ToTop, 2, 0, "213", "Move last subclass to top");
            MoveItem(d, ListMove.ToTop, 1, 0, "123", "Move second subclass to top");
            MoveItem(d, ListMove.ToBottom, 0, 2, "231", "Move first subclass to bottom");
            MoveItem(d, ListMove.ToTop, 0, 0, "231", "Move first subclass to top (should have no effect)");
            MoveItem(d, ListMove.ToBottom, 2, 2, "231", "Move last subclass to bottom (should have no effect)");
            MoveItem(d, ListMove.Up, 0, 0, "231", "Move first subclass up (should have no effect)");
            MoveItem(d, ListMove.Down, 2, 2, "231", "Move last subclass down (should have no effect)");
            MoveItem(d, ListMove.Down, 0, 1, "321", "Move first subclass down");
            MoveItem(d, ListMove.Down, 1, 2, "312", "Move second subclass down");
            MoveItem(d, ListMove.Up, 1, 0, "132", "Move second subclass up");
            MoveItem(d, ListMove.ToBottom, 1, 2, "123", "Move second subclass to bottom");
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
