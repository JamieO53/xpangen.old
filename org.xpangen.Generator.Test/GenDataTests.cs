// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
        /// Tests the generator data subclass functionality
        /// </summary>
        [TestCase(Description="Generator data subclass tests")]
        public void GenSubClassTests()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            var c = CreateGenObject(d, d.Root, "Class", "Class");//d.CreateObject("", "Class");

            var i = f.Classes[f.GetClassId("Class")].IndexOfSubClass("SubClass");
            var s = new GenObjectList(c.SubClass[i], d.GenDataBase, d.Context[f.GetClassId("Class")],
                                      f.GetClassSubClasses("Class")[i]);
            var sc = CreateGenObject(d, c, "SubClass", "SubClass");
            d.Context[f.GetClassId("SubClass")].Last();
            Assert.AreEqual(d.Context[f.GetClassId("SubClass")].GenObject, sc);
            Assert.AreEqual(s.GenObject, sc);
        }

        [TestCase(Description = "Verify that the SetUpParentChildData method works as expected")]
        public void VerifySetUpParentChildDataMethod()
        {
            var d = SetUpParentChildData("Parent", "Child", "Child");
            var f = d.GenDataDef;
            Assert.AreEqual(3, f.Classes.Count);
            Assert.AreEqual("Parent", f.GetClassName(1));
            Assert.AreEqual("Child", f.GetClassName(2));
            Assert.AreEqual(1, f.GetClassSubClasses(1).Count);
            Assert.AreEqual(2, f.GetClassSubClasses(1)[0].SubClass.ClassId);
            Assert.AreEqual("", f.GetClassSubClasses(1)[0].Reference);
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

        [TestCase(Description = "Verify that a referenced object can get a value from the referencing data")]
        public void  VerifyParentChildReferenceGetValue()
        {
            var dataChild = SetUpParentChildData("Child", "Grandchild", "Grandchild");
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "Child", "Child", dataChild);
            var genObject = dataParent.Context[3].GenObject;
            var id = dataParent.GenDataDef.GetId("Parent", "Name");
            Assert.AreEqual("Parent", genObject.GetValue(id));
        }

        [TestCase(Description = "Verify that the SetUpParentChildReferenceData method sets up the data definitions as expected")]
        public void VerifyParentChildReferenceDataDef()
        {
            var dataChild = SetUpParentChildData("Child", "Grandchild", "Grandchild");
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "Child", "Child", dataChild);
            Assert.AreEqual(ReferenceGenDataSaveProfile, GenDataDefProfile.CreateProfile(dataParent.GenDataDef));
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
            var dataChild = SetUpParentChildReferenceData("Child", "Grandchild", "Grandchild", "GrandChild", dataGrandchildhild);
            var dataParent = SetUpParentChildReferenceData("Parent", "Child", "Child", "Child", dataChild);
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
            f.AddClassInstanceProperty(classId, "Prop1");
            f.AddClassInstanceProperty(classId, "Prop2");
            var d = new GenData(f);
            var o = CreateGenObject(d, d.Root, "Class");
            var a = new GenAttributes(f, 1) {GenObject = o};
            a.SetString("Prop1", "Prop1");
            a.SetString("Prop2", "Prop2");
            a.SaveFields();
            Assert.AreEqual(2, f.GetClassProperties(classId).Count);
        }

        /// <summary>
        /// Tests the attribute functionality of generator classes
        /// </summary>
        [TestCase(Description="Generator Attribute tests")]
        public void GenAttributeTests()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);
            var o = CreateGenObject(d, d.Root, "Class", "Class");
            CreateGenObject(d, o, "SubClass", "SubClass");

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

            var c = CreateGenObject(d, d.Root, "Class", "Class");
            CreateGenObject(d, c, "SubClass", "SubClass1");
            CreateGenObject(d, c, "SubClass", "SubClass2");
            CreateGenObject(d, c, "SubClass", "SubClass3");
            
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
