using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Minimal;

namespace org.xpangen.Generator.Test
{
    [TestFixture]
    public class ApplicationBaseTests : GenDataTestsBase
    {
        /// <summary>
        /// Verify that generator model data can be reordered correctly.
        /// </summary>
        [TestCase(Description="Tests the reordering of application list data")]
        public void ApplicationListReorderTest()
        {
            var d = CreateOrderedGenData();
            Assert.IsFalse(d.Changed, "Class creation should not change Changed flag");
            var c = new Class(d.GenDataDef) {GenObject = d.Context[ClassClassId].Context};

            // Verify initial subclass order
            Assert.IsFalse(d.Changed, "Class creation should not change Changed flag");
            CheckOrder(c, "123", "Verify initial subclass order");
            MoveItem(c, ListMove.Up, 2, "132", true, "Move last subclass up one place");
            MoveItem(c, ListMove.ToTop, 2, "213", true, "Move last subclass to top");
            MoveItem(c, ListMove.ToTop, 1, "123", true, "Move second subclass to top");
            MoveItem(c, ListMove.ToBottom, 0, "231", true, "Move first subclass to bottom");
            MoveItem(c, ListMove.ToTop, 0, "231", false, "Move first subclass to top (should have no effect)");
            MoveItem(c, ListMove.ToBottom, 2, "231", false, "Move last subclass to bottom (should have no effect)");
            MoveItem(c, ListMove.Up, 0, "231", false, "Move first subclass up (should have no effect)");
            MoveItem(c, ListMove.Down, 2, "231", false, "Move last subclass down (should have no effect)");
            MoveItem(c, ListMove.Down, 0, "321", true, "Move first subclass down");
            MoveItem(c, ListMove.Down, 1, "312", true, "Move second subclass down");
            MoveItem(c, ListMove.Up, 1, "132", true, "Move second subclass up");
            MoveItem(c, ListMove.ToBottom, 1, "123", true, "Move second subclass to bottom");
        }

        private static void MoveItem(Class c, ListMove move, int itemIndex, string order, bool changedExpected, string action)
        {
            c.GenObject.GenData.Changed = false;
            c.SubClassList.Move(move, itemIndex);
            Assert.AreEqual(changedExpected, c.GenObject.GenData.Changed, "Data changed flag");
            CheckOrder(c, order, action);
        }

        private static GenData CreateOrderedGenData()
        {
            var f = GenDataDef.CreateMinimal();
            var d = new GenData(f);

            CreateNamedClass(d, "", "Class", "Class");
            CreateNamedClass(d, "Class", "SubClass", "SubClass1");
            CreateNamedClass(d, "Class", "SubClass", "SubClass2");
            CreateNamedClass(d, "Class", "SubClass", "SubClass3");
            d.First(ClassClassId);
            return d;
        }

        private static void CheckOrder(Class c, string order, string action)
        {
            c.GenObject.GenData.First(SubClassClassId);
            var context = c.GenObject.GenData.Context[SubClassClassId];
            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual("SubClass" + order[i], c.SubClassList[i].Name, action + " in list - item " + (i + 1));
                Assert.AreEqual("SubClass" + order[i], context.Context.Attributes[0], action + " in generator data - item " + (i + 1));
                c.GenObject.GenData.Next(SubClassClassId);
            }
        }
    }
}
