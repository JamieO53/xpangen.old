// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Tests the discovery of context from a specified object
    /// </summary>
    [TestFixture]
    public class GenDataContextTests : GenDataTestsBase
    {
        [Test(Description = "Test the identification of an object with the same class")]
        public void TestObjectContextWithSameClass()
        {
            var d = SetUpParentChildData("Parent", "Child", "FirstChild");
            var parent = GetFirstObject(d);
            var child = GetFirstObjectOfSubClass(parent, "Child");
            
            var childContext = d.GenDataBase.GetContext(child, "Child");
            Assert.AreSame(child, childContext);
            childContext = d.GenDataBase.GetContext(child, "CHILD"); // Case independent search
            Assert.AreSame(child, childContext);
        }

        [Test(Description = "Test the identification of the parent object")]
        public void TestObjectContextOfParent()
        {
            var d = SetUpParentChildData("Parent", "Child", "FirstChild");
            var parent = GetFirstObject(d);
            var child = GetFirstObjectOfSubClass(parent, "Child");
            
            var childContext = d.GenDataBase.GetContext(child, "Parent");
            Assert.AreSame(parent, childContext);
            childContext = d.GenDataBase.GetContext(child, "PARENT"); // Case independent search
            Assert.AreSame(parent, childContext);
        }

        [Test(Description = "Test the identification of the child object")]
        public void TestObjectContextOfChild()
        {
            var d = SetUpParentChildData("Parent", "Child", "FirstChild");
            var parent = GetFirstObject(d);
            var child = GetFirstObjectOfSubClass(parent, "Child");
            
            var parentContext = d.GenDataBase.GetContext(parent, "Child");
            Assert.AreSame(child, parentContext);
            parentContext = d.GenDataBase.GetContext(parent, "CHILD"); // Case independent search
            Assert.AreSame(child, parentContext);
        }

        [Test(Description = "Test the identification of the grandchild object")]
        public void TestObjectContextOfGrandchild()
        {
            var f = SetUpParentChildDef("Parent", "Child");
            SetUpChildDef("Child", "GrandChild", f);
            var d = new GenData(f) { DataName = "Parent"};
            var parent = CreateGenObject(d, d.Root, "Parent", "Parent");
            var child = CreateGenObject(d, parent, "Child", "FirstChild");
            var grandChild = CreateGenObject(d, child, "GrandChild", "FirstGrandchild");
            
            var grandchildContext = d.GenDataBase.GetContext(parent, "GrandChild");
            Assert.AreSame(grandChild, grandchildContext);
            grandchildContext = d.GenDataBase.GetContext(parent, "GRANDCHILD"); // Case independent search
            Assert.AreSame(grandChild, grandchildContext);
        }

        [Test(Description = "Test the identification of the grandparent object")]
        public void TestObjectContextOfGrandparent()
        {
            var f = SetUpParentChildDef("Parent", "Child");
            SetUpChildDef("Child", "GrandChild", f);
            var d = new GenData(f) { DataName = "Parent"};
            var parent = CreateGenObject(d, d.Root, "Parent", "Parent");
            var child = CreateGenObject(d, parent, "Child", "FirstChild");
            var grandChild = CreateGenObject(d, child, "GrandChild", "FirstGrandchild");
            
            var grandparentContext = d.GenDataBase.GetContext(grandChild, "Parent");
            Assert.AreSame(parent, grandparentContext);
            grandparentContext = d.GenDataBase.GetContext(grandChild, "PARENT"); // Case independent search
            Assert.AreSame(parent, grandparentContext);
        }

        [Test(Description = "Test the identification of a sibling object")]
        public void TestObjectContextOfSibling()
        {
            var f = SetUpParentChildDef("Parent", "FirstChild");
            SetUpChildDef("Parent", "SecondChild", f);
            var d = new GenData(f) { DataName = "Parent"};
            var parent = CreateGenObject(d, d.Root, "Parent", "Parent");
            var firstChild = CreateGenObject(d, parent, "FirstChild", "FirstChild");
            var secondChild = CreateGenObject(d, parent, "SecondChild", "SecondChild");
            
            var siblingContext = d.GenDataBase.GetContext(firstChild, "SecondChild");
            Assert.AreSame(secondChild, siblingContext);
            siblingContext = d.GenDataBase.GetContext(firstChild, "SECONDCHILD"); // Case independent search
            Assert.AreSame(secondChild, siblingContext);
        }

        [Test(Description = "Test the identification of a child object with inheritance")]
        public void TestObjectContextOfSiblingWithInheritance()
        {
            var dataFile = GetTestDataFileName("InheritanceDataSaveTest");
            var d = PopulateInheritanceData(dataFile);
            var container = GetFirstObject(d);
            var @virtual1 = GetFirstObjectOfSubClass(container, "Abstract");
            var containerContext = d.GenDataBase.GetContext(container, "Abstract");
            Assert.AreSame(@virtual1, containerContext);
        }

        [Test(Description = "Test the identification of a parent from an object with inheritance")]
        public void TestObjectContextOfParentWithInheritance()
        {
            var dataFile = GetTestDataFileName("InheritanceDataSaveTest");
            var d = PopulateInheritanceData(dataFile);
            var container = GetFirstObject(d);
            var @virtual1 = GetFirstObjectOfSubClass(container, "Abstract");
            var child = GetFirstObjectOfSubClass(@virtual1, "Child");
            var containerContext = d.GenDataBase.GetContext(child, "Container");
            Assert.AreSame(container, containerContext);
        }

        [Test(Description = "Test the identification of a parent from an reference object")]
        [Ignore("Jamie is lazy")]
        public void TestObjectContextOfParentFromAReferenceObject()
        {
            Assert.Fail("To be implemented");
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            GenDataLoader.Register();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            
        }
    }
}
