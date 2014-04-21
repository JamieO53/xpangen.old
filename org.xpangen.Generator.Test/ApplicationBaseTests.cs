// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;

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
            var c = new Class(d) {GenObject = d.Context[ClassClassId].GenObject};

            // Verify initial subclass order
            Assert.IsFalse(d.Changed, "Class creation should not change Changed flag");
            CheckOrder(d, c, "123", "Verify initial subclass order");
            MoveItem(d, c, ListMove.Up, 2, "132", true, "Move last subclass up one place");
            MoveItem(d, c, ListMove.ToTop, 2, "213", true, "Move last subclass to top");
            MoveItem(d, c, ListMove.ToTop, 1, "123", true, "Move second subclass to top");
            MoveItem(d, c, ListMove.ToBottom, 0, "231", true, "Move first subclass to bottom");
            MoveItem(d, c, ListMove.ToTop, 0, "231", false, "Move first subclass to top (should have no effect)");
            MoveItem(d, c, ListMove.ToBottom, 2, "231", false, "Move last subclass to bottom (should have no effect)");
            MoveItem(d, c, ListMove.Up, 0, "231", false, "Move first subclass up (should have no effect)");
            MoveItem(d, c, ListMove.Down, 2, "231", false, "Move last subclass down (should have no effect)");
            MoveItem(d, c, ListMove.Down, 0, "321", true, "Move first subclass down");
            MoveItem(d, c, ListMove.Down, 1, "312", true, "Move second subclass down");
            MoveItem(d, c, ListMove.Up, 1, "132", true, "Move second subclass up");
            MoveItem(d, c, ListMove.ToBottom, 1, "123", true, "Move second subclass to bottom");
        }

        private static void MoveItem(GenData genData, Class c, ListMove move, int itemIndex, string order, bool changedExpected, string action)
        {
            c.GenObject.GenDataBase.Changed = false;
            c.SubClassList.Move(move, itemIndex);
            Assert.AreEqual(changedExpected, c.GenObject.GenDataBase.Changed, "Data changed flag");
            CheckOrder(genData, c, order, action);
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

        private static void CheckOrder(GenData genData, Class c, string order, string action)
        {
            
            genData.First(SubClassClassId);
            var context = genData.Context[SubClassClassId];
            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual("SubClass" + order[i], c.SubClassList[i].Name, action + " in list - item " + (i + 1));
                Assert.AreEqual("SubClass" + order[i], context.GenObject.Attributes[0], action + " in generator data - item " + (i + 1));
                genData.Next(SubClassClassId);
            }
        }
    }
}
