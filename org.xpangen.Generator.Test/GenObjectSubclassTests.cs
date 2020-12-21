using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    [TestFixture]
    public class GenObjectSubclassTests: GenDataTestsBase
    {
        [Test(Description="Tests the retrieval of a class' subclass")]
        public void GenObjectSubClassLookup()
        {
            var f = SetUpParentChildDef("Parent", "Child");
            f.AddSubClass("Parent", "SecondChild");
            f.AddClassInstanceProperty(3, "Name");
            var d = new GenDataBase(f);
            var parent = CreateGenObject(d.Root, "Parent", "Parent");
            CreateGenObject(parent, "Child", "FirstChild");
            CreateGenObject(parent, "SecondChild", "SecondChild");
            var sc = parent.GetSubClass("SecondChild");
            Assert.AreEqual("SecondChild", sc[0].Attributes[0]);
            sc = parent.GetSubClass("Child");
            Assert.AreEqual("FirstChild", sc[0].Attributes[0]);
        }

        [Test(Description="Tests the retrieval of a class' inherited subclass")]
        public void GenObjectSubClassLookupWithInheritance()
        {
            var f = SetUpParentChildDef("Parent", "Child");
            f.AddInheritor("Child", "FirstVirtualChild");
            f.AddInheritor("Child", "SecondVirtualChild");
            var d = new GenDataBase(f);
            var parent = CreateGenObject(d.Root, "Parent", "Parent");
            CreateGenObject(parent, "FirstVirtualChild", "FirstChild");
            CreateGenObject(parent, "SecondVirtualChild", "SecondChild");
            var sc = parent.GetSubClass("Child");
            Assert.AreEqual("FirstChild", sc[0].Attributes[0]);
            Assert.AreEqual("SecondChild", sc[1].Attributes[0]);
            sc = parent.GetSubClass("FirstVirtualChild");
            Assert.AreEqual("FirstChild", sc[0].Attributes[0]);
            sc = parent.GetSubClass("SecondVirtualChild");
            Assert.AreEqual("SecondChild", sc[0].Attributes[0]);
        }

        [Test(Description="Tests the retrieval of a class' nested inherited subclass")]
        public void GenObjectSubClassLookupWithNestedInheritance()
        {
            var f = SetUpParentChildDef("Parent", "Child");
            f.AddInheritor("Child", "FirstVirtualChild");
            f.AddInheritor("Child", "SecondVirtualChild");
            f.AddInheritor("SecondVirtualChild", "FirstVirtualGrandchildOfSecond");
            var d = new GenDataBase(f);
            var parent = CreateGenObject(d.Root, "Parent", "Parent");
            CreateGenObject(parent, "FirstVirtualChild", "FirstChild");
            CreateGenObject(parent, "FirstVirtualGrandchildOfSecond", "FirstGrandChild");
            CreateGenObject(parent, "FirstVirtualGrandchildOfSecond", "SecondGrandChild");
            var sc = parent.GetSubClass("Child");
            Assert.AreEqual("FirstChild", sc[0].Attributes[0]);
            Assert.AreEqual("FirstGrandChild", sc[1].Attributes[0]);
            Assert.AreEqual("SecondGrandChild", sc[2].Attributes[0]);
            sc = parent.GetSubClass("FirstVirtualChild");
            Assert.AreEqual("FirstChild", sc[0].Attributes[0]);
            sc = parent.GetSubClass("SecondVirtualChild");
            Assert.AreEqual("FirstGrandChild", sc[0].Attributes[0]);
            Assert.AreEqual("SecondGrandChild", sc[1].Attributes[0]);
        }

        [Test(Description = "Tests the retrieval of a class' inherited subclass")]
        public void GenObjectSubClassLookupWithInheritanceSubclass()
        {
            var f = SetUpParentChildDef("Parent", "Child");
            f.AddInheritor("Child", "FirstVirtualChild");
            f.AddInheritor("Child", "SecondVirtualChild");
            f.AddSubClass("Child", "GrandChild");
            var d = new GenDataBase(f);
            var parent = CreateGenObject(d.Root, "Parent", "Parent");
            var child1 = CreateGenObject(parent, "FirstVirtualChild", "FirstChild");
            var child2 = CreateGenObject(parent, "SecondVirtualChild", "SecondChild");
            CreateGenObject(child1, "GrandChild", "FirstGrandchild");
            CreateGenObject(child2, "GrandChild", "SecondGrandchild");
            var sc = parent.GetSubClass("Child");
            Assert.AreEqual(1, sc[0].SubClass.Count);
            Assert.AreEqual("FirstGrandchild", sc[0].SubClass[0][0].Attributes[0]);
            Assert.AreEqual("SecondGrandchild", sc[1].SubClass[0][0].Attributes[0]);
        }

        [Test(Description = "Tests the retrieval of a class' referenced subclass")]
        public void GenObjectSubClassLookupWithReferenceSubclass()
        {
            var dataGrandchildhild = SetUpParentChildData("Grandchild", "Greatgrandchild", "Greatgrandchild");
            var dataChild = SetUpParentChildReferenceData("Child", "Grandchild", "GrandchildDef", "Grandchild", dataGrandchildhild);
            var child = dataChild.Root.GetSubClass("Child")[0];
            var sc = child.GetSubClass("Grandchild");
            Assert.AreEqual(1, sc[0].SubClass.Count);
            Assert.AreEqual("Greatgrandchild", sc[0].SubClass[0][0].Attributes[0]);
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            GenDataLoader.Register();
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