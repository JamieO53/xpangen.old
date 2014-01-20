// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
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

            d.Context[ClassClassId].Next();
            CreateSubClass(d, "FieldFilter");

            id = f.GetId("FieldFilter.Name");
            Assert.IsTrue(d.Eol(id.ClassId));

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
            var s = new GenObjectList(o.SubClass[i]);
            o = s.CreateObject();
            a.GenObject = o;
            a.SetString("Name", "SubClass");
            a.SaveFields();
            d.EstablishContext(o);
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
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "The 'self' generator data cannot be added explicitly to the cache\r\nParameter name: name")]
        public void ReferenceCacheSelfLocalPathTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            var d0 = GenDataDef.CreateMinimal().AsGenData();
            d.Cache.Internal("Minimal", "self", d0);
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
            var minimal = AddToCache(d, "Minimal");
            var definition = AddToCache(d, "Definition");
            Assert.AreNotSame(minimal, definition);
        }

        [TestCase(Description = "Confirm that reference context is set up correctly.")]
        public void InternalReferenceUnfilteredContextTest()
        {
            var d = SetUpUnfilteredReferenceData();
            Assert.IsTrue(d.Cache.Contains("Child"));
            Assert.AreEqual("Parent", d.Context[1].GenObject.Definition.Name);
            Assert.AreEqual("Child", d.Context[2].GenObject.Definition.Name);
            Assert.AreEqual("Grandchild", d.Context[3].GenObject.Definition.Name);
            Assert.AreEqual(2, d.Context[2].ClassId);
            Assert.AreEqual(3, d.Context[3].ClassId);
        }

        [TestCase(Description = "Confirm that reference data gets included.")]
        public void InternalReferenceUnfilteredTest()
        {
            var d = SetUpUnfilteredReferenceData();
            Assert.AreEqual(1, d.Context[1].GenObject.Definition.SubClasses.Count);
            Assert.IsTrue(d.Cache.Contains("Child"));
            Assert.AreEqual("Child", d.Context[1].GenObject.Definition.SubClasses[0].SubClass.Name);
            Assert.AreEqual("Child", d.Context[1].GenObject.Definition.SubClasses[0].Reference);
            Assert.IsNotNull(d.Context[2]);
            d.First(2);
            Assert.AreEqual("Child", d.Context[2].GenObject.Definition.Name);
            d.First(1);
        }

        [TestCase(Description = "Confirm that resetting the context works.")]
        public void InternalReferenceSubsetSetResetTest()
        {
            var d = SetUpUnfilteredReferenceData();
            d.First(1);
            Assert.AreEqual("First child", d.Context[2].GenObject.Attributes[0]);
        }

        [TestCase(Description = "Confirm that multiple referenced data gets accessed.")]
        public void InternalReferenceUnfilteredMultipleTest()
        {
            var d = SetUpUnfilteredReferenceData();
            d.First(1);
            Assert.AreEqual("First child", d.Context[2].GenObject.Attributes[0]);
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
#pragma warning disable 168
            // Reference to initialize static data
            var loader = new GenDataLoader();
#pragma warning restore 168
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
