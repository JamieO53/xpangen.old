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
            @"Definition=Minimal
Class=Class
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
            Assert.AreEqual("Minimal", d.GenDataDef.Definition);
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
            Assert.AreEqual(1, d.Context[RootClassId].GenObject.SubClass.Count);

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

            a.GenObject = d.Context[ClassClassId].GenObject;
            Assert.AreEqual("Class", a.AsString("Name"));
            Assert.AreEqual(2, d.Context[ClassClassId].GenObject.SubClass.Count);
            Assert.AreEqual(2, d.Context[SubClassClassId].Count);
            Assert.AreEqual(1, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", a.AsString("Name"));

            // SubClass class tests - SubClass
            Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            a.GenObject = d.Context[SubClassClassId].GenObject;
            Assert.AreEqual("SubClass", a.AsString("Name"));
            Assert.AreEqual(1, d.Context[SubClassClassId].GenObject.SubClass.Count);

            // SubClass class tests - Property
            d.Next(SubClassClassId);
            Assert.IsTrue(d.Context[SubClassClassId].IsLast());
            a.GenObject = d.Context[SubClassClassId].GenObject;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[PropertyClassId].GenObject.SubClass.Count);

            // SubClass class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].GenObject;
            Assert.AreEqual("SubClass", a.AsString("Name"));
            Assert.AreEqual(1, d.Context[SubClassClassId].Count);
            Assert.AreEqual(2, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", a.AsString("Name"));
            d.Next(PropertyClassId);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Reference", a.AsString("Name"));

            // SubClass class tests - FieldFilter
            Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            a.GenObject = d.Context[SubClassClassId].GenObject;
            Assert.AreEqual("FieldFilter", a.AsString("Name"));

            // Property class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].GenObject;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(1, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", a.AsString("Name"));

            // FieldFilter class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].GenObject;
            Assert.AreEqual("FieldFilter", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(2, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", a.AsString("Name"));
            d.Next(PropertyClassId);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Operand", a.AsString("Name"));
        }

        protected static void CreateClass(GenData d, string name)
        {
            CreateGenObject(d, "", "Class", name);
            CreateProperty(d, "Name");
        }

        private static GenObject CreateGenObject(GenData d, string parentClassName, string className, string name)
        {
            var o = d.CreateObject(parentClassName, className);
            o.Attributes[0] = name;
            return o;
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
            Assert.AreEqual(1, f.Classes[0].SubClasses.Count);
            Assert.AreEqual(2, f.Classes[ClassClassId].SubClasses.Count);
            Assert.AreEqual(1, f.Classes[SubClassClassId].SubClasses.Count);
            Assert.AreEqual(0, f.Classes[PropertyClassId].SubClasses.Count);
            Assert.AreEqual(0, f.Classes[FieldFilterClassId].SubClasses.Count);
            Assert.AreEqual(ClassClassId, f.Classes[0].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual(SubClassClassId, f.Classes[ClassClassId].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual(PropertyClassId, f.Classes[ClassClassId].SubClasses[1].SubClass.ClassId);
            Assert.AreEqual(FieldFilterClassId, f.Classes[SubClassClassId].SubClasses[0].SubClass.ClassId);
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
            f.Classes[parentId].Properties.Add("Name");
            var childId = f.AddClass("Parent", "Child");
            f.Classes[childId].Properties.Add("Name");
            f.Classes[childId].Properties.Add("Lookup");
            var lookupId = f.AddClass("Parent", "Lookup");
            f.Classes[lookupId].Properties.Add("Name");

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

        protected static GenData SetUpLookupData()
        {
            var f = GenDataDef.CreateMinimal();

            var d = SetUpLookupData(f);
            return d;
        }

        protected static GenData SetUpReferenceData(string reference)
        {
            var f = CreateSelfReferenceDefinition(reference);

            var d = new GenData(f);
            CreateGenObject(d, "", "Root", "Root");
            CreateGenObject(d, "Root", "ReferenceData", "1").Attributes[1] = "First reference";
            CreateGenObject(d, "Root", "ReferenceData", "2").Attributes[1] = "Second reference";
            CreateGenObject(d, "Root", "BaseData", "DataExists").Attributes[1] = "1";
            CreateGenObject(d, "Root", "BaseData", "DataDoesNotExist").Attributes[1] = "3";
            return d;
        }

        protected static GenDataDef CreateSelfReferenceDefinition(string reference)
        {
            var f = new GenDataDef();
            f.AddSubClass("", "Root");
            f.AddSubClass("Root", "ReferenceData");
            f.AddSubClass("Root", "BaseData");
            f.Classes[f.Classes.IndexOf("Root")].Properties.Add("Name");
            f.Classes[f.Classes.IndexOf("ReferenceData")].Properties.Add("Name");
            f.Classes[f.Classes.IndexOf("ReferenceData")].Properties.Add("Value");
            f.Classes[f.Classes.IndexOf("BaseData")].Properties.Add("Name");
            f.Classes[f.Classes.IndexOf("BaseData")].Properties.Add("ReferenceKey");
            f.AddSubClass("BaseData", "ReferenceLookup", reference);
            return f;
        }

        private static GenData SetUpLookupData(GenDataDef f)
        {
            var d = new GenData(f);
            d.CreateObject("", "Class").Attributes[0] = "Class";
            d.CreateObject("Class", "Property").Attributes[0] = "Name";
            d.CreateObject("", "Class").Attributes[0] = "SubClass";
            d.CreateObject("Class", "Property").Attributes[0] = "Name";
            d.CreateObject("", "Class").Attributes[0] = "Property";
            d.CreateObject("Class", "Property").Attributes[0] = "Name";
            d.First(1);
            d.CreateObject("Class", "SubClass").Attributes[0] = "SubClass";
            d.CreateObject("Class", "SubClass").Attributes[0] = "Property";
            return d;
        }
    }
}