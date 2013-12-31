// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Data;

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
            var s = o.SubClass[i];
            o = s.CreateObject();
            a.GenObject = o;
            a.SetString("Name", "SubClass");
            a.SaveFields();
            d.EstablishContext(o);
            d.Context[f.Classes.IndexOf("SubClass")].Last();

            s.Last();
        }

        /// <summary>
        /// Tests the generator data subclass functionality
        /// </summary>
        [TestCase(Description = "Generator data subclass for a local reference tests")]
        public void GenSubClassLocalReferenceTestTests()
        {
            var d = SetUpLookupData();
            var f = d.GenDataDef;
            f.AddSubClass("SubClass", "SubClassClass", "self:Class.Name=SubClass.Name");
            // todo: create data anologous to Lookup profile fragment test
        }

        /// <summary>
        /// Tests the attribute functionality of generator classes
        /// </summary>
        [TestCase(Description = "Generator Attribute Property tests")]
        public void GenAttributePropertyTests()
        {
            var f = new GenDataDef();
            var classId = f.AddClass("", "Class");
            f.Properties[classId].Add("Prop1");
            f.Properties[classId].Add("Prop2");
            var d = new GenData(f);
            var o = d.CreateObject("", "Class");
            var a = new GenAttributes(f) {GenObject = o};
            a.SetString("Prop1", "Prop1");
            a.SetString("Prop2", "Prop2");
            a.SaveFields();
            Assert.AreEqual(2, f.Properties[classId].Count);
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
            var c = new GenContext(d);

            SetUpData(d);
            d.First(ClassClassId);
            d.Last(SubClassClassId);
            //Assert.IsTrue(d.Eol(SubClassClassId));
            Assert.AreEqual("Property", d.Context[SubClassClassId].Context.Attributes[0]);
            c.SaveContext();
            d.Context[SubClassClassId].First();
            Assert.AreEqual("SubClass", d.Context[SubClassClassId].Context.Attributes[0]);
            c.RestoreContext();
            //Assert.IsTrue(d.Eol(SubClassClassId));
            Assert.AreEqual("Property", d.Context[SubClassClassId].Context.Attributes[0]);
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

        private static void MoveItem(GenData d, ListMove move, int itemIndex, int newItemIndex, string order, string action)
        {
            d.Context[SubClassClassId].Index = itemIndex;
            d.Context[SubClassClassId].MoveItem(move, itemIndex);
            CheckOrder(d, newItemIndex, order, action);
        }

        private static void CheckOrder(GenData d, int itemIndex, string order, string action)
        {
            var id = d.GenDataDef.GetId("SubClass.Name");
            Assert.AreEqual(itemIndex, d.Context[id.ClassId].Index, "Expected index value");
            d.First(ClassClassId);
            d.First(SubClassClassId);
            Assert.AreEqual("SubClass" + order[0], d.GetValue(id), action + " first item");
            d.Next(SubClassClassId);
            Assert.AreEqual("SubClass" + order[1], d.GetValue(id), action + " second item");
            d.Next(SubClassClassId);
            Assert.AreEqual("SubClass" + order[2], d.GetValue(id), action + " third item");
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            
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
