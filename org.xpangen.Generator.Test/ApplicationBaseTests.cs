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
            var d = CreateOrderedGenData(6);
            Assert.IsFalse(d.Changed, "Class creation should not change Changed flag");
            var c = new Class(d) {GenObject = d.Root.SubClass[0][0]};

            // Verify initial subclass order
            Assert.IsFalse(d.Changed, "Class creation should not change Changed flag");
            CheckOrder(c, "123456", "Verify initial subclass order");
            MoveItem(c, ListMove.Up, 5, "123465", true, "Move last subclass up one place");
            MoveItem(c, ListMove.ToTop, 5, "512346", true, "Move last subclass to top");
            MoveItem(c, ListMove.ToTop, 1, "152346", true, "Move second subclass to top");
            MoveItem(c, ListMove.ToBottom, 0, "523461", true, "Move first subclass to bottom");
            MoveItem(c, ListMove.ToTop, 0, "523461", false, "Move first subclass to top (should have no effect)");
            MoveItem(c, ListMove.ToBottom, 5, "523461", false, "Move last subclass to bottom (should have no effect)");
            MoveItem(c, ListMove.Up, 0, "523461", false, "Move first subclass up (should have no effect)");
            MoveItem(c, ListMove.Down, 5, "523461", false, "Move last subclass down (should have no effect)");
            MoveItem(c, ListMove.Down, 0, "253461", true, "Move first subclass down");
            MoveItem(c, ListMove.Down, 1, "235461", true, "Move second subclass down");
            MoveItem(c, ListMove.Up, 1, "325461", true, "Move second subclass up");
            MoveItem(c, ListMove.ToBottom, 1, "354612", true, "Move second subclass to bottom");
        }

        /// <summary>
        /// Verify that generator model data can be reordered correctly.
        /// </summary>
        [TestCase(Description="Tests the reordering of renamed application list data")]
        public void ApplicationListReorderWithRenameTest()
        {
            var d = CreateOrderedGenData(6);
            Assert.IsFalse(d.Changed, "Class creation should not change Changed flag");
            var c = new Class(d) {GenObject = d.Root.SubClass[0][0]};
            c.SubClassList[5].Name = "SubClass7";
            d.Changed = false;

            // Verify initial subclass order
            Assert.IsFalse(d.Changed, "Class creation should not change Changed flag");
            CheckOrder(c, "123457", "Verify initial subclass order");
            MoveItem(c, ListMove.Up, 5, "123475", true, "Move last subclass up one place");
            MoveItem(c, ListMove.ToTop, 5, "512347", true, "Move last subclass to top");
            MoveItem(c, ListMove.ToTop, 1, "152347", true, "Move second subclass to top");
            MoveItem(c, ListMove.ToBottom, 0, "523471", true, "Move first subclass to bottom");
            MoveItem(c, ListMove.ToTop, 0, "523471", false, "Move first subclass to top (should have no effect)");
            MoveItem(c, ListMove.ToBottom, 5, "523471", false, "Move last subclass to bottom (should have no effect)");
            MoveItem(c, ListMove.Up, 0, "523471", false, "Move first subclass up (should have no effect)");
            MoveItem(c, ListMove.Down, 5, "523471", false, "Move last subclass down (should have no effect)");
            MoveItem(c, ListMove.Down, 0, "253471", true, "Move first subclass down");
            MoveItem(c, ListMove.Down, 1, "235471", true, "Move second subclass down");
            MoveItem(c, ListMove.Up, 1, "325471", true, "Move second subclass up");
            MoveItem(c, ListMove.ToBottom, 1, "354712", true, "Move second subclass to bottom");
        }

        private static void MoveItem(Class c, ListMove move, int itemIndex, string order, bool changedExpected, string action)
        {
            c.GenObject.GenDataBase.Changed = false;
            c.SubClassList.Move(move, itemIndex);
            Assert.AreEqual(changedExpected, c.GenObject.GenDataBase.Changed, "Data changed flag");
            CheckOrder(c, order, action);
        }

        private static GenDataBase CreateOrderedGenData(int subClassCount)
        {
            var m = new Minimal();
            var c = m.AddClass("Class", "Class");
            for (var i = 1; i <= subClassCount; i++)
                c.AddSubClass("SubClass" + i);
            m.GenDataBase.Changed = false;
            return m.GenDataBase;
        }

        private static void CheckOrder(Class c, string order, string action)
        {
            for (var i = 0; i < c.SubClassList.Count; i++)
                Assert.AreEqual("SubClass" + order[i], c.SubClassList[i].Name, action + " in list - item " + (i + 1));
            for (var i = 0; i < c.SubClassList.Count; i++)
                Assert.AreEqual(i, c.SubClassList.IndexOf("SubClass" + order[i]), action + " in list - item " + (i + 1));
            for (var i = 0; i < ((GenObject) c.GenObject).SubClass[0].Count; i++)
                Assert.AreEqual("SubClass" + order[i], ((GenObject) c.GenObject).SubClass[0][i].Attributes[0],
                    action + " in GenObject.SubClass - item " + (i + 1));
        }
    }
}
