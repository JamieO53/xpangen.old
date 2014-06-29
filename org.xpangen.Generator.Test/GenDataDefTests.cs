// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the generator data definition functionality
    /// </summary>
	[TestFixture]
    public class GenDataDefTests : GenDataTestsBase
    {
        /// <summary>
        /// Tests the TextList functionality
        /// </summary>
        [TestCase(Description="Text list test")]
        public void TextListTest()
        {
            var textList = new TextList();
            Assert.AreEqual(0, textList.Count);
            var i = textList.Add("Sample");
            Assert.AreEqual(0, i);
            Assert.AreEqual(0, textList.IndexOf("Sample"));
            Assert.AreEqual(-1, textList.IndexOf("SAMPLE"));
            Assert.AreEqual(-1, textList.IndexOf("Junk"));
            Assert.AreEqual(1, textList.Count);
        }

        /// <summary>
        /// Tests the NameList functionality
        /// </summary>
        [TestCase(Description="Name List Test")]
        public void NameListTest()
        {
            var textList = new NameList();
            Assert.AreEqual(0, textList.Count);
            var i = textList.Add("Sample");
            Assert.AreEqual(0, i);
            Assert.AreEqual(0, textList.IndexOf("Sample"));
            Assert.AreEqual(0, textList.IndexOf("SAMPLE"));
            Assert.AreEqual(-1, textList.IndexOf("Junk"));
            Assert.AreEqual(1, textList.Count);
        }

        /// <summary>
        /// Tests the new GenDataDef functionality
        /// </summary>
        [TestCase(Description = "New Generator Data Defintion Test")]
        public void NewGenDataDefTest()
        {
            var def = new GenDataDef();
            Assert.AreEqual(1, def.Classes.Count, "Classes in a new def");
            Assert.AreEqual("", def.Classes[0].Name, "Dummy root class name");
            Assert.AreEqual(0, def.Classes.IndexOf(""), "Index of dummy root");
            Assert.AreEqual(0, def.Classes[0].SubClasses.Count, "Subclasses of dummy root");
            Assert.IsNull(def.Classes[0].Parent, "Parent of dummy root");
            Assert.IsEmpty(def.Classes[0].Properties, "Dummy root has no properties");
        }

        /// <summary>
        /// Tests the GenDataDef functionality
        /// </summary>
        [TestCase(Description="Generator Data Defintion Test")]
        public void GenDataDefTest()
        {
            const string rootProfile =
                "Definition=Root\r\n" +
                "Class=Root\r\n" +
                "Field=Id\r\n" +
                ".\r\n" +
                "`[Root:Root[`?Root.Id:Id`?Root.Id<>True:=`@StringOrName:`{`Root.Id``]`]`]`]]\r\n" +
                "`]";

            var d = new GenDataDef();
            d.Definition = "Root";
            var i = d.AddClass("", "Root");
            Assert.AreEqual(1, i);
            Assert.AreEqual(i, d.Classes.IndexOf("Root"));
            Assert.AreEqual(0, d.IndexOfSubClass(0, i));
            var j = d.Classes[i].InstanceProperties.Add("Id");
            Assert.AreEqual(j, d.Classes[i].Properties.IndexOf("Id"));
            Assert.AreEqual(rootProfile, GenDataDefProfile.CreateProfile(d));

            var id = d.GetId("Root.Id");
            Assert.AreEqual(i, id.ClassId);
            Assert.AreEqual(j, id.PropertyId);

            i = d.AddClass("Root", "Sub");
            Assert.AreEqual(2, i);
            Assert.AreEqual(i, d.Classes.IndexOf("Sub"));
            Assert.AreEqual(0, d.IndexOfSubClass(1, i));
            Assert.AreEqual(1, d.Classes[i].Parent.ClassId);
            j = d.Classes[i].InstanceProperties.Add("SubId");
            Assert.AreEqual(0, j);
            Assert.AreEqual(j, d.Classes[i].Properties.IndexOf("SubId"));

            id = d.GetId("Sub.SubId");
            Assert.AreEqual(i, id.ClassId);
            Assert.AreEqual(j, id.PropertyId);
        }

        /// <summary>
        /// Tests the functionality that creates a minimum definition
        /// </summary>
        [TestCase(Description="Create minimal definition test")]
        public void CreateMinimalTest()
        {
            var f = GenDataDef.CreateMinimal();
            Assert.AreEqual(0, f.Classes.IndexOf(""));
            Assert.AreEqual(1, f.Classes.IndexOf("Class"));
            Assert.AreEqual(2, f.Classes.IndexOf("SubClass"));
            Assert.AreEqual(3, f.Classes.IndexOf("Property"));
            Assert.AreEqual(1, f.Classes[0].SubClasses.Count);
            Assert.AreEqual(2, f.Classes[1].SubClasses.Count);
            Assert.AreEqual(0, f.Classes[2].SubClasses.Count);
            Assert.AreEqual(0, f.Classes[3].SubClasses.Count);
            Assert.AreEqual(1, f.Classes[0].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual(2, f.Classes[1].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual(3, f.Classes[1].SubClasses[1].SubClass.ClassId);
        }

        [TestCase(Description = "Verify that the SetUpParentChildReferenceDef method works as expected")]
        public void VerifySetUpParentChildReferenceDefMethod()
        {
            var fChild = SetUpParentChildDef("Child", "Grandchild");
            var fParent = SetUpParentChildReferenceDef("Parent", "Child", "ChildDef", fChild);
            Assert.AreEqual(4, fParent.Classes.Count);
            Assert.AreEqual(1, fParent.Classes[0].SubClasses.Count);
            Assert.AreEqual("Parent", fParent.Classes[1].Name);
            Assert.AreEqual("Child", fParent.Classes[2].Name);
            Assert.AreEqual("Grandchild", fParent.Classes[3].Name);
            Assert.AreEqual(1, fParent.Classes[1].SubClasses.Count);
            Assert.AreEqual(2, fParent.Classes[1].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual("ChildDef", fParent.Classes[1].SubClasses[0].Reference);
            Assert.AreEqual("ChildDef", fParent.Classes[1].SubClasses[0].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[2].SubClasses[0].Reference);
            Assert.AreEqual("ChildDef", fParent.Classes[2].SubClasses[0].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[1].SubClasses[0].SubClass.Reference);
            Assert.AreEqual("ChildDef", fParent.Classes[1].SubClasses[0].SubClass.ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[2].SubClasses[0].SubClass.Reference);
            Assert.AreEqual("ChildDef", fParent.Classes[2].SubClasses[0].SubClass.ReferenceDefinition);
            Assert.AreEqual(fParent.Classes[2].RefClassId, fChild.Classes[1].ClassId);
            Assert.AreEqual(fParent.Classes[3].RefClassId, fChild.Classes[2].ClassId);
            Assert.AreEqual("ChildDef", fParent.Classes[2].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[3].ReferenceDefinition);
            Assert.AreEqual(fParent.Classes[2].Name, fChild.Classes[1].Name);
            Assert.AreEqual(fParent.Classes[3].Name, fChild.Classes[2].Name);
            Assert.AreSame(fParent.Classes[0], fParent.Classes[1].Parent);
            Assert.AreSame(fParent.Classes[1], fParent.Classes[2].Parent);
            Assert.AreSame(fParent.Classes[2], fParent.Classes[3].Parent);
        }

private const string Profile = @"Definition=Parent
Class=Parent
Field=Name
SubClass=Child[Reference='ChildDef']
.
`[Parent:Parent=`Parent.Name`
`[Child@:`]`]";

        [TestCase(Description = "Verify that the SetUpParentChildReferenceDef method generates a profile as expected")]
        public void VerifySetUpParentChildReferenceDefProfile()
        {
            var fChild = SetUpParentChildDef("Child", "Grandchild");
            var fParent = SetUpParentChildReferenceDef("Parent", "Child", "ChildDef", fChild);
            Assert.AreEqual(Profile, GenDataDefProfile.CreateProfile(fParent));
        }

        [TestCase(Description = "Verify that the SetUpParentChildReferenceDef method works as expected when nested")]
        public void VerifyNestedSetUpParentChildReferenceDefMethod()
        {
            var fGrandchild = SetUpParentChildDef("Grandchild", "Greatgrandchild");
            var fChild = SetUpParentChildReferenceDef("Child", "Grandchild", "GrandchildDef", fGrandchild);
            var fParent = SetUpParentChildReferenceDef("Parent", "Child", "ChildDef", fChild);
            Assert.AreEqual(5, fParent.Classes.Count);
            Assert.AreEqual(1, fParent.Classes[0].SubClasses.Count);
            Assert.AreEqual("Parent", fParent.Classes[1].Name);
            Assert.AreEqual("Child", fParent.Classes[2].Name);
            Assert.AreEqual("Grandchild", fParent.Classes[3].Name);
            Assert.AreEqual("Greatgrandchild", fParent.Classes[4].Name);
            Assert.AreEqual("Name", fParent.Classes[1].Properties[0]);
            Assert.AreEqual("Name", fParent.Classes[2].Properties[0]);
            Assert.AreEqual("Name", fParent.Classes[3].Properties[0]);
            Assert.AreEqual("Name", fParent.Classes[4].Properties[0]);
            Assert.AreEqual(1, fParent.Classes[1].SubClasses.Count);
            Assert.AreEqual(2, fParent.Classes[1].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual("ChildDef", fParent.Classes[1].SubClasses[0].Reference);
            Assert.AreEqual("ChildDef", fParent.Classes[1].SubClasses[0].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[2].SubClasses[0].Reference);
            Assert.AreEqual("ChildDef", fParent.Classes[2].SubClasses[0].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[1].SubClasses[0].SubClass.Reference);
            Assert.AreEqual("ChildDef", fParent.Classes[1].SubClasses[0].SubClass.ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[2].SubClasses[0].SubClass.Reference);
            Assert.AreEqual("ChildDef", fParent.Classes[2].SubClasses[0].SubClass.ReferenceDefinition);
            Assert.AreEqual(fParent.Classes[2].RefClassId, fChild.Classes[1].ClassId);
            Assert.AreEqual(fParent.Classes[3].RefClassId, fChild.Classes[2].ClassId);
            Assert.AreEqual(fParent.Classes[4].RefClassId, fChild.Classes[3].ClassId);
            Assert.AreEqual("ChildDef", fParent.Classes[2].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[3].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.Classes[4].ReferenceDefinition);
            Assert.AreEqual(fParent.Classes[2].Name, fChild.Classes[1].Name);
            Assert.AreEqual(fParent.Classes[3].Name, fChild.Classes[2].Name);
            Assert.AreEqual(fParent.Classes[4].Name, fChild.Classes[3].Name);
            Assert.AreEqual(fParent.Classes[3].Name, fGrandchild.Classes[1].Name);
            Assert.AreEqual(fParent.Classes[4].Name, fGrandchild.Classes[2].Name);
            Assert.AreSame(fParent.Classes[0], fParent.Classes[1].Parent);
            Assert.AreSame(fParent.Classes[1], fParent.Classes[2].Parent);
            Assert.AreSame(fParent.Classes[2], fParent.Classes[3].Parent);
            Assert.AreSame(fParent.Classes[3], fParent.Classes[4].Parent);
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
