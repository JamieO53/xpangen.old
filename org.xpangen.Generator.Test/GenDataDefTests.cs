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
    public class GenDataDefTests
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
            Assert.AreEqual("", def.Classes[0], "Dummy root class name");
            Assert.AreEqual(0, def.Classes.IndexOf(""), "Index of dummy root");
            Assert.AreEqual(0, def.SubClasses[0].Count, "Subclasses of dummy root");
            Assert.AreEqual(-1, def.Parents[0], "Parent of dummy root");
            Assert.IsEmpty(def.Properties[0], "Dummy root has no properties");
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
                "`[Root:Root[`?Id:Id`?Id<>'True':=`@StringOrName:`{`Root.Id``]`]`]`]]\r\n" +
                "`]";
            //const string subProfile =
            //    "Class=Root\r\n" +
            //    "Field=Id\r\n" +
            //    "SubClass=Sub\r\n" +
            //    "Class=Sub\r\n" +
            //    "Field=SubId\r\n" +
            //    ".\r\n" +
            //    "`[Root:Root[`?Id:Id`?Id<>'True':=`@StringOrName:`{`Root.Id``]`]`]`]]\r\n" +
            //    "`[Sub:Sub[`?SubId:SubId`?SubId<>'True':=`@StringOrName:`{`Sub.SubId``]`]`]`]]\r\n" +
            //    "`]`]";
            //const string subReportProfile =
            //    "`[Root:  Root\r\n" +
            //    "    Id\t`Root.Id`\r\n" +
            //    "`[Sub:    Sub\r\n" +
            //    "      SubId\t`Sub.SubId`\r\n" +
            //    "`]`]";
            var d = new GenDataDef();
            d.Definition = "Root";
            var i = d.AddClass("", "Root");
            Assert.AreEqual(1, i);
            Assert.AreEqual(i, d.Classes.IndexOf("Root"));
            Assert.AreEqual(0, d.IndexOfSubClass(0, i));
            var j = d.Properties[i].Add("Id");
            Assert.AreEqual(j, d.Properties[i].IndexOf("Id"));
            var s = d.CreateProfile();
            Assert.AreEqual(rootProfile, s);

            var id = d.GetId("Root.Id");
            Assert.AreEqual(i, id.ClassId);
            Assert.AreEqual(j, id.PropertyId);

            i = d.AddClass("Root", "Sub");
            Assert.AreEqual(2, i);
            Assert.AreEqual(i, d.Classes.IndexOf("Sub"));
            Assert.AreEqual(0, d.IndexOfSubClass(1, i));
            Assert.AreEqual(1, d.Parents[i]);
            j = d.Properties[i].Add("SubId");
            Assert.AreEqual(0, j);
            Assert.AreEqual(j, d.Properties[i].IndexOf("SubId"));
            //s = d.CreateProfile();
            //Assert.AreEqual(subProfile, s);
            //s = d.ReportProfile();
            //Assert.AreEqual(subReportProfile, s);

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
            GenDataDef f = GenDataDef.CreateMinimal();
            Assert.AreEqual(0, f.Classes.IndexOf(""));
            Assert.AreEqual(1, f.Classes.IndexOf("Class"));
            Assert.AreEqual(2, f.Classes.IndexOf("SubClass"));
            Assert.AreEqual(3, f.Classes.IndexOf("Property"));
            Assert.AreEqual(4, f.Classes.IndexOf("FieldFilter"));
            Assert.AreEqual(1, f.SubClasses[0].Count);
            Assert.AreEqual(2, f.SubClasses[1].Count);
            Assert.AreEqual(1, f.SubClasses[2].Count);
            Assert.AreEqual(0, f.SubClasses[3].Count);
            Assert.AreEqual(0, f.SubClasses[4].Count);
            Assert.AreEqual(1, f.SubClasses[0][0]);
            Assert.AreEqual(2, f.SubClasses[1][0]);
            Assert.AreEqual(3, f.SubClasses[1][1]);
            Assert.AreEqual(4, f.SubClasses[2][0]);
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
