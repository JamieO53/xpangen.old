// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Minimal;
using Class = org.xpangen.Generator.Data.Definition.Class;

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
            var c = new Class(d) {GenObject = d.Root.SubClass[0][0]};

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
            c.GenObject.GenDataBase.Changed = false;
            c.SubClassList.Move(move, itemIndex);
            Assert.AreEqual(changedExpected, c.GenObject.GenDataBase.Changed, "Data changed flag");
            CheckOrder(c, order, action);
        }

        private static GenDataBase CreateOrderedGenData()
        {
            var m = new Minimal();
            var c = m.AddClass("Class", "Class");
            c.AddSubClass("SubClass1");
            c.AddSubClass("SubClass2");
            c.AddSubClass("SubClass3");
            m.GenDataBase.Changed = false;
            return m.GenDataBase;
            //var f = GenDataDef.CreateMinimal();
            //var d = new GenData(f);

            //var c = CreateGenObject(d, d.Root, "Class", "Class");
            //CreateGenObject(d, c, "SubClass", "SubClass1");
            //CreateGenObject(d, c, "SubClass", "SubClass2");
            //CreateGenObject(d, c, "SubClass", "SubClass3");
            //d.First(ClassClassId);
            //d.GenDataBase.Changed = false;
            //return d;
        }

        private static void CheckOrder(Class c, string order, string action)
        {
            for (var i = 0; i < c.SubClassList.Count; i++)
                Assert.AreEqual("SubClass" + order[i], c.SubClassList[i].Name, action + " in list - item " + (i + 1));
        }
    }
}
