// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Test
{
    public class GenDataTestsBase
    {
        protected const int RootClassId = 0;
        protected const int ClassClassId = 1;
        protected const int SubClassClassId = 2;
        protected const int PropertyClassId = 3;


        protected const string GenDataSaveText =
            @"Class=Class
Field={Name,Title}
SubClass={SubClass,Property}
Class=SubClass
Field=Name
Class=Property
Field=Name
.
Class=Class[Title='Class object']
SubClass=SubClass
SubClass=Property
Property=Name
Class=SubClass[]
Property=Name
Class=Property[]
Property=Name
";

        protected static void ValidateMinimalData(GenData d)
        {
            var a = new GenAttributes(d.GenDataDef);
            Assert.AreEqual(4, d.Context.Count);

            Assert.IsFalse(d.Eol(RootClassId));
            Assert.IsFalse(d.Eol(ClassClassId));
            Assert.IsFalse(d.Eol(SubClassClassId));
            Assert.IsFalse(d.Eol(PropertyClassId));

            Assert.AreEqual(RootClassId, d.Context[RootClassId].ClassId);
            Assert.AreEqual(ClassClassId, d.Context[ClassClassId].ClassId);
            Assert.AreEqual(SubClassClassId, d.Context[SubClassClassId].ClassId);
            Assert.AreEqual(PropertyClassId, d.Context[PropertyClassId].ClassId);

            Assert.IsTrue(d.Context[RootClassId].IsFirst());
            Assert.IsTrue(d.Context[RootClassId].IsLast());
            Assert.AreEqual(1, d.Context[RootClassId].Context.SubClass.Count);

            // Class class tests
            d.First(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            Assert.IsFalse(d.Eol(PropertyClassId));
            Assert.IsFalse(d.Eol(PropertyClassId));
            Assert.IsTrue(d.Context[ClassClassId].IsFirst());
            Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            Assert.IsTrue(d.Context[PropertyClassId].IsFirst());

            a.GenObject = d.Context[ClassClassId].Context;
            Assert.AreEqual("Class", a.AsString("Name"));
            Assert.AreEqual(2, d.Context[ClassClassId].Context.SubClass.Count);
            Assert.AreEqual(2, d.Context[SubClassClassId].Count);
            Assert.AreEqual(1, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].Context;
            Assert.AreEqual("Name", a.AsString("Name"));

            // SubClass class tests - SubClass
            Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            a.GenObject = d.Context[SubClassClassId].Context;
            Assert.AreEqual("SubClass", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Context.SubClass.Count);

            // SubClass class tests - Property
            d.Next(SubClassClassId);
            Assert.IsTrue(d.Context[SubClassClassId].IsLast());
            a.GenObject = d.Context[SubClassClassId].Context;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Context.SubClass.Count);

            // SubClass class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].Context;
            Assert.AreEqual("SubClass", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(1, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].Context;
            Assert.AreEqual("Name", a.AsString("Name"));

            // Property class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].Context;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(1, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].Context;
            Assert.AreEqual("Name", a.AsString("Name"));
        }

        protected static void CreateClass(GenData d, string name)
        {
            CreateGenObject(d, "", "Class", name);
            CreateProperty(d, "Name");
        }

        private static void CreateGenObject(GenData d, string parentClassName, string className, string name)
        {
            var o = d.CreateObject(parentClassName, className);
            o.Attributes[0] = name;
        }

        private static void CreateProperty(GenData d, string name)
        {
            CreateGenObject(d, "Class", "Property", name);
        }

        protected static void CreateSubClass(GenData d, string name)
        {
            CreateGenObject(d, "Class", "SubClass", name);
        }

        protected static void VerifyAsDef(GenDataDef f)
        {
            Assert.AreEqual(0, f.Classes.IndexOf(""));
            Assert.AreEqual(1, f.Classes.IndexOf("Class"));
            Assert.AreEqual(2, f.Classes.IndexOf("SubClass"));
            Assert.AreEqual(3, f.Classes.IndexOf("Property"));
            Assert.AreEqual(1, f.SubClasses[0].Count);
            Assert.AreEqual(2, f.SubClasses[1].Count);
            Assert.AreEqual(0, f.SubClasses[2].Count);
            Assert.AreEqual(0, f.SubClasses[3].Count);
            Assert.AreEqual(1, f.SubClasses[0][0]);
            Assert.AreEqual(2, f.SubClasses[1][0]);
            Assert.AreEqual(3, f.SubClasses[1][1]);
        }

        protected static void VerifyDataCreation(GenData d)
        {
            var a = new GenAttributes(d.GenDataDef);
            var id = d.GenDataDef.GetId("Class.Name");
            Assert.AreEqual("Class", d.GetValue(id));

            id = d.GenDataDef.GetId("SubClass.Name");
            Assert.AreEqual("SubClass", d.GetValue(id));

            var o = d.Root.SubClass[0][0];
            a.GenObject = o;
            Assert.AreEqual("Class", a.AsString("Name"));

            o = d.Context[d.GenDataDef.Classes.IndexOf("SubClass")][0];
            a.GenObject = o;
            Assert.AreEqual("SubClass", a.AsString("Name"));
        }

        protected static void SetUpData(GenData genData)
        {
            CreateClass(genData, "Class");
            CreateClass(genData, "SubClass");
            CreateClass(genData, "Property");
            genData.Context[ClassClassId].First();
            CreateSubClass(genData, "SubClass");
            CreateSubClass(genData, "Property");
        }

        protected static void CreateNamedClass(GenData d, string parentClassName, string className, string name)
        {
            var o = d.CreateObject(parentClassName, className);
            o.Attributes[0] = name;
        }

        protected static void CreateGenDataSaveText()
        {
            if (File.Exists("GenDataSaveData.txt"))
                File.Delete("GenDataSaveData.txt");
            File.WriteAllText("GenDataSaveData.txt", GenDataSaveText);
        }
    }
}