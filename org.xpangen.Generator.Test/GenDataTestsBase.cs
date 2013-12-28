// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using System.Threading;
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
        protected const int FieldFilterClassId = 4;


        protected const string GenDataSaveText =
            @"Class=Class
Field={Name,Title}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference}
SubClass=FieldFilter
Class=FieldFilter
Field={Name,Operand}
Class=Property
Field=Name
.
Class=Class[Title='Class object']
SubClass=SubClass[]
SubClass=Property[]
Property=Name
Class=SubClass[]
SubClass=FieldFilter[]
Property=Name
Property=Reference
Class=Property[]
Property=Name
Class=FieldFilter[]
Property=Name
Property=Operand
";

        protected static void ValidateMinimalData(GenData d)
        {
            var a = new GenAttributes(d.GenDataDef);
            Assert.AreEqual(5, d.Context.Count);

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
            Assert.IsFalse(d.Eol(SubClassClassId));
            Assert.IsFalse(d.Eol(PropertyClassId));
            Assert.IsTrue(d.Eol(FieldFilterClassId));
            Assert.IsTrue(d.Context[ClassClassId].IsFirst());
            Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            Assert.IsTrue(d.Context[PropertyClassId].IsFirst());
            Assert.IsNull(d.Context[FieldFilterClassId]);

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
            Assert.AreEqual(1, d.Context[SubClassClassId].Context.SubClass.Count);

            // SubClass class tests - Property
            d.Next(SubClassClassId);
            Assert.IsTrue(d.Context[SubClassClassId].IsLast());
            a.GenObject = d.Context[SubClassClassId].Context;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[PropertyClassId].Context.SubClass.Count);

            // SubClass class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].Context;
            Assert.AreEqual("SubClass", a.AsString("Name"));
            Assert.AreEqual(1, d.Context[SubClassClassId].Count);
            Assert.AreEqual(2, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].Context;
            Assert.AreEqual("Name", a.AsString("Name"));
            d.Next(PropertyClassId);
            a.GenObject = d.Context[PropertyClassId].Context;
            Assert.AreEqual("Reference", a.AsString("Name"));

            // SubClass class tests - FieldFilter
            Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            a.GenObject = d.Context[SubClassClassId].Context;
            Assert.AreEqual("FieldFilter", a.AsString("Name"));

            // Property class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].Context;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(1, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].Context;
            Assert.AreEqual("Name", a.AsString("Name"));

            // FieldFilter class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].Context;
            Assert.AreEqual("FieldFilter", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(2, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].Context;
            Assert.AreEqual("Name", a.AsString("Name"));
            d.Next(PropertyClassId);
            a.GenObject = d.Context[PropertyClassId].Context;
            Assert.AreEqual("Operand", a.AsString("Name"));
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

        internal static void CreateProperty(GenData d, string name)
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
            Assert.AreEqual(ClassClassId, f.Classes.IndexOf("Class"));
            Assert.AreEqual(SubClassClassId, f.Classes.IndexOf("SubClass"));
            Assert.AreEqual(PropertyClassId, f.Classes.IndexOf("Property"));
            Assert.AreEqual(FieldFilterClassId, f.Classes.IndexOf("FieldFilter"));
            Assert.AreEqual(1, f.SubClasses[0].Count);
            Assert.AreEqual(2, f.SubClasses[ClassClassId].Count);
            Assert.AreEqual(1, f.SubClasses[SubClassClassId].Count);
            Assert.AreEqual(0, f.SubClasses[PropertyClassId].Count);
            Assert.AreEqual(0, f.SubClasses[FieldFilterClassId].Count);
            Assert.AreEqual(ClassClassId, f.SubClasses[0][0]);
            Assert.AreEqual(SubClassClassId, f.SubClasses[ClassClassId][0]);
            Assert.AreEqual(PropertyClassId, f.SubClasses[ClassClassId][1]);
            Assert.AreEqual(FieldFilterClassId, f.SubClasses[SubClassClassId][0]);
        }

        protected static void VerifyDataCreation(GenData d)
        {
            d.First(1);
            d.First(2);
            d.First(3);
            
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
            CreateProperty(genData, "Reference");
            CreateClass(genData, "Property");
            CreateClass(genData, "FieldFilter");
            CreateProperty(genData, "Operand");
            genData.Context[ClassClassId].First();
            CreateSubClass(genData, "SubClass");
            CreateSubClass(genData, "Property");
            genData.Context[ClassClassId].Next();
            CreateSubClass(genData, "FieldFilter");
            genData.First(ClassClassId);
        }

        protected static void CreateNamedClass(GenData d, string parentClassName, string className, string name)
        {
            var o = d.CreateObject(parentClassName, className);
            o.Attributes[0] = name;
        }

        protected static void CreateGenDataSaveText(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, GenDataSaveText);
        }

        protected static GenData SetUpLookupContextData()
        {
            var f = new GenDataDef();
            var parentId = f.AddClass("", "Parent");
            f.Properties[parentId].Add("Name");
            var childId = f.AddClass("Parent", "Child");
            f.Properties[childId].Add("Name");
            f.Properties[childId].Add("Lookup");
            var lookupId = f.AddClass("Parent", "Lookup");
            f.Properties[lookupId].Add("Name");

            var a = new GenAttributes(f);
            var d = new GenData(f);
            a.GenObject = d.CreateObject("", "Parent");
            a.SetString("Name", "Parent");
            a.SaveFields();

            a.GenObject = d.CreateObject("Parent", "Child");
            a.SetString("Name", "Child1");
            a.SetString("Lookup", "Valid");
            a.SaveFields();

            a.GenObject = d.CreateObject("Parent", "Child");
            a.SetString("Name", "Child2");
            a.SetString("Lookup", "Invalid");
            a.SaveFields();

            a.GenObject = d.CreateObject("Parent", "Lookup");
            a.SetString("Name", "Valid");
            a.SaveFields();

            return d;
        }
    }
}