// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Data;

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
            Assert.AreEqual("", def.GetClassName(0), "Dummy root class name");
            Assert.AreEqual(0, def.GetClassId(""), "Index of dummy root");
            Assert.AreEqual(0, def.GetClassSubClasses(0).Count, "Subclasses of dummy root");
            Assert.IsNull(def.GetClassParent(0), "Parent of dummy root");
            Assert.IsEmpty(def.GetClassProperties(0), "Dummy root has no properties");
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

            var d = new GenDataDef {DefinitionName = "Root"};
            var i = d.AddClass("", "Root");
            Assert.AreEqual(1, i);
            Assert.AreEqual(i, d.GetClassId("Root"));
            Assert.AreEqual(0, d.Classes[0].IndexOfSubClass("Root"));
            var j = d.AddClassInstanceProperty(i, "Id");
            Assert.AreEqual(j, d.GetClassProperties(i).IndexOf("Id"));
            Assert.AreEqual(rootProfile, GenDataDefProfile.CreateProfile(d));

            var id = d.GetId("Root.Id");
            Assert.AreEqual(i, id.ClassId);
            Assert.AreEqual(j, id.PropertyId);

            i = d.AddClass("Root", "Sub");
            Assert.AreEqual(2, i);
            Assert.AreEqual(i, d.GetClassId("Sub"));
            Assert.AreEqual(0, d.Classes[1].IndexOfSubClass("Sub"));
            Assert.AreEqual(1, d.GetClassParent(i).ClassId);
            j = d.AddClassInstanceProperty(i, "SubId");
            Assert.AreEqual(0, j);
            Assert.AreEqual(j, d.GetClassProperties(i).IndexOf("SubId"));

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
            Assert.AreEqual(0, f.GetClassId(""));
            Assert.AreEqual(1, f.GetClassId("Class"));
            Assert.AreEqual(2, f.GetClassId("SubClass"));
            Assert.AreEqual(3, f.GetClassId("Property"));
            Assert.AreEqual(1, f.GetClassSubClasses(0).Count);
            Assert.AreEqual(2, f.GetClassSubClasses(1).Count);
            Assert.AreEqual(0, f.GetClassSubClasses(2).Count);
            Assert.AreEqual(0, f.GetClassSubClasses(3).Count);
            Assert.AreEqual(1, f.GetClassSubClasses(0)[0].SubClass.ClassId);
            Assert.AreEqual(2, f.GetClassSubClasses(1)[0].SubClass.ClassId);
            Assert.AreEqual(3, f.GetClassSubClasses(1)[1].SubClass.ClassId);
        }

        [TestCase(Description = "Verify that the SetUpParentChildReferenceDef method works as expected")]
        public void VerifySetUpParentChildReferenceDefMethod()
        {
            var fChild = SetUpParentChildDef("Child", "Grandchild");
            var fParent = SetUpParentChildReferenceDef("Parent", "Child", "ChildDef", fChild);
            Assert.AreEqual(4, fParent.Classes.Count);
            Assert.AreEqual(1, fParent.GetClassSubClasses(0).Count);
            Assert.AreEqual("Parent", fParent.GetClassName(1));
            Assert.AreEqual("Child", fParent.GetClassName(2));
            Assert.AreEqual("Grandchild", fParent.GetClassName(3));
            Assert.AreEqual(1, fParent.GetClassSubClasses(1).Count);
            Assert.AreEqual(2, fParent.GetClassSubClasses(1)[0].SubClass.ClassId);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(1)[0].Reference);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(1)[0].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(2)[0].Reference);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(2)[0].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(1)[0].SubClass.Reference);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(1)[0].SubClass.ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(2)[0].SubClass.Reference);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(2)[0].SubClass.ReferenceDefinition);
            Assert.AreEqual(fParent.GetClassDef(2).RefClassId, fChild.GetClassDef(1).ClassId);
            Assert.AreEqual(fParent.GetClassDef(3).RefClassId, fChild.GetClassDef(2).ClassId);
            Assert.AreEqual("ChildDef", fParent.GetClassDef(2).ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassDef(3).ReferenceDefinition);
            Assert.AreEqual(fParent.GetClassName(2), fChild.GetClassName(1));
            Assert.AreEqual(fParent.GetClassName(3), fChild.GetClassName(2));
            Assert.AreSame(fParent.GetClassDef(0), fParent.GetClassParent(1));
            Assert.AreSame(fParent.GetClassDef(1), fParent.GetClassParent(2));
            Assert.AreSame(fParent.GetClassDef(2), fParent.GetClassParent(3));
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
            Assert.AreEqual(1, fParent.GetClassSubClasses(0).Count);
            Assert.AreEqual("Parent", fParent.GetClassName(1));
            Assert.AreEqual("Child", fParent.GetClassName(2));
            Assert.AreEqual("Grandchild", fParent.GetClassName(3));
            Assert.AreEqual("Greatgrandchild", fParent.GetClassName(4));
            Assert.AreEqual("Name", fParent.GetClassProperties(1)[0]);
            Assert.AreEqual("Name", fParent.GetClassProperties(2)[0]);
            Assert.AreEqual("Name", fParent.GetClassProperties(3)[0]);
            Assert.AreEqual("Name", fParent.GetClassProperties(4)[0]);
            Assert.AreEqual(1, fParent.GetClassSubClasses(1).Count);
            Assert.AreEqual(2, fParent.GetClassSubClasses(1)[0].SubClass.ClassId);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(1)[0].Reference);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(1)[0].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(2)[0].Reference);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(2)[0].ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(1)[0].SubClass.Reference);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(1)[0].SubClass.ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(2)[0].SubClass.Reference);
            Assert.AreEqual("ChildDef", fParent.GetClassSubClasses(2)[0].SubClass.ReferenceDefinition);
            Assert.AreEqual(fParent.GetClassDef(2).RefClassId, fChild.GetClassDef(1).ClassId);
            Assert.AreEqual(fParent.GetClassDef(3).RefClassId, fChild.GetClassDef(2).ClassId);
            Assert.AreEqual(fParent.GetClassDef(4).RefClassId, fChild.GetClassDef(3).ClassId);
            Assert.AreEqual("ChildDef", fParent.GetClassDef(2).ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassDef(3).ReferenceDefinition);
            Assert.AreEqual("ChildDef", fParent.GetClassDef(4).ReferenceDefinition);
            Assert.AreEqual(fParent.GetClassName(2), fChild.GetClassName(1));
            Assert.AreEqual(fParent.GetClassName(3), fChild.GetClassName(2));
            Assert.AreEqual(fParent.GetClassName(4), fChild.GetClassName(3));
            Assert.AreEqual(fParent.GetClassName(3), fGrandchild.GetClassName(1));
            Assert.AreEqual(fParent.GetClassName(4), fGrandchild.GetClassName(2));
            Assert.AreSame(fParent.GetClassDef(0), fParent.GetClassParent(1));
            Assert.AreSame(fParent.GetClassDef(1), fParent.GetClassParent(2));
            Assert.AreSame(fParent.GetClassDef(2), fParent.GetClassParent(3));
            Assert.AreSame(fParent.GetClassDef(3), fParent.GetClassParent(4));
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            
        }

        /// <summary>
        /// Tear down the Generator data definition tests
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {

        }
    }
}
