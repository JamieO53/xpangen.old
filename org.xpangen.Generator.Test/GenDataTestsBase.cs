﻿// This Source Code Form is subject to the terms of the Mozilla Public
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

        protected const string GenDataSaveText =
            @"Definition=Minimal
Class=Class
Field={Name,Title}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference}
Class=Property
Field=Name
.
Class=Class[Title='Class object']
SubClass=SubClass[]
SubClass=Property[]
Property=Name
Class=SubClass[]
Property=Name
Property=Reference
Class=Property[]
Property=Name
";

        protected static void ValidateMinimalData(GenData d)
        {
            var a = new GenAttributes(d.GenDataDef);
            Assert.AreEqual("Minimal", d.GenDataDef.Definition);
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
            Assert.AreEqual(1, d.Context[RootClassId].GenObject.SubClass.Count);

            // Class class tests
            d.First(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            Assert.IsFalse(d.Eol(SubClassClassId));
            Assert.IsFalse(d.Eol(PropertyClassId));
            Assert.IsTrue(d.Context[ClassClassId].IsFirst());
            Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            Assert.IsTrue(d.Context[PropertyClassId].IsFirst());

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
            Assert.AreEqual(0, d.Context[SubClassClassId].GenObject.SubClass.Count);

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
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(2, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", a.AsString("Name"));
            d.Next(PropertyClassId);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Reference", a.AsString("Name"));

            // Property class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            a.GenObject = d.Context[ClassClassId].GenObject;
            Assert.AreEqual("Property", a.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(1, d.Context[PropertyClassId].Count);
            a.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", a.AsString("Name"));
        }

        protected static void CreateClass(GenData d, string name)
        {
            CreateGenObject(d, "", "Class", name);
            CreateProperty(d, "Name");
        }

        protected static GenObject CreateGenObject(GenData d, string parentClassName, string className, string name)
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
            Assert.AreEqual(1, f.Classes[0].SubClasses.Count);
            Assert.AreEqual(2, f.Classes[ClassClassId].SubClasses.Count);
            Assert.AreEqual(0, f.Classes[SubClassClassId].SubClasses.Count);
            Assert.AreEqual(0, f.Classes[PropertyClassId].SubClasses.Count);
            Assert.AreEqual(ClassClassId, f.Classes[0].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual(SubClassClassId, f.Classes[ClassClassId].SubClasses[0].SubClass.ClassId);
            Assert.AreEqual(PropertyClassId, f.Classes[ClassClassId].SubClasses[1].SubClass.ClassId);
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
            genData.Context[ClassClassId].First();
            CreateSubClass(genData, "SubClass");
            CreateSubClass(genData, "Property");
            genData.Context[ClassClassId].Next();
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

        protected static GenData SetUpParentChildReferenceData(string parentClassName, string childClassName, string childDefName, GenData dataChild)
        {
            var def = SetUpParentChildReferenceDef(parentClassName, childClassName, childDefName, dataChild.GenDataDef);
            var data = new GenData(def);
            CreateGenObject(data, "", parentClassName, "First " + parentClassName.ToLowerInvariant());
            SetUpParentReference(data, dataChild, childClassName + "Def", parentClassName, childClassName, childClassName);
            return data;
        }

        protected static GenDataDef SetUpParentChildReferenceDef(string parentClassName, string childClassName,
                                                               string childDefName, GenDataDef defChild)
        {
            var def = new GenDataDef();
            def.Definition = parentClassName;
            def.Cache.Internal(childDefName, defChild);
            def.AddSubClass("", parentClassName);
            def.Classes[1].Properties.Add("Name");
            def.AddSubClass(parentClassName, childClassName, childDefName);
            return def;
        }

        protected static GenData SetUpParentChildData(string parentClassName, string childClassName)
        {
            var def = SetUpParentChildDef(parentClassName, childClassName);
            var data = new GenData(def);
            CreateGenObject(data, "", parentClassName, parentClassName);
            CreateGenObject(data, parentClassName, childClassName, parentClassName + "'s first " + childClassName.ToLowerInvariant());
            //data.First(0);
            return data;
        }

        protected static GenDataDef SetUpParentChildDef(string parentClassName, string childClassName)
        {
            var def = new GenDataDef();
            def.Definition = parentClassName;
            def.AddSubClass("", parentClassName);
            def.Classes[1].Properties.Add("Name");
            def.AddSubClass(parentClassName, childClassName);
            def.Classes[2].Properties.Add("Name");
            return def;
        }

        private static void SetUpParentReference(GenData dataParent, GenData dataChild, string childDefName,
                                                 string parentClassName, string childClassName, string childName)
        {
            dataParent.Cache.Internal("Minimal", childDefName, dataChild.GenDataDef.AsGenData());
            dataParent.Cache.Internal(childDefName, childName, dataChild);
            dataParent.Cache.Merge();
            SetSubClassReference(dataParent, parentClassName, childClassName, childName);
            dataParent.First(1);
        }

        /// <summary>
        /// Set the subclass reference.
        /// </summary>
        /// <param name="data">The generator data.</param>
        /// <param name="className">The parent class name.</param>
        /// <param name="subClassName">The childe class name.</param>
        /// <param name="reference">The reference path.</param>
        protected static void SetSubClassReference(GenData data, string className, string subClassName, string reference)
        {
            var classes = data.GenDataDef.Classes;
            var classId = classes.IndexOf(className);
            var subClassIndex = classes[classId].SubClasses.IndexOf(classes.IndexOf(subClassName));
            var sub = data.Context[classId].GenObject.SubClass[subClassIndex] as GenObjectListReference;
            if (sub != null)
                sub.Reference = reference.ToLowerInvariant();
        }

        protected static void MoveItem(GenData d, ListMove move, int itemIndex, int newItemIndex, string order, string action)
        {
            d.Context[SubClassClassId].Index = itemIndex;
            d.Context[SubClassClassId].MoveItem(move, itemIndex);
            CheckOrder(d, newItemIndex, order, action);
        }

        protected static void CheckOrder(GenData d, int itemIndex, string order, string action)
        {
            var id = d.GenDataDef.GetId("SubClass.Name");
            Assert.AreEqual(itemIndex, d.Context[id.ClassId].Index, "Expected index value");
            d.First(ClassClassId);
            d.First(SubClassClassId);
            Assert.AreEqual("SubClass" + order[0], d.GetValue(id), action + " first item");
            d.Next(SubClassClassId);
            Assert.AreEqual("SubClass" + order[1], d.GetValue(id), action + " second item");
            d.Next(SubClassClassId);
            Assert.AreEqual("SubClass" + order[2], d.GetValue(id), action + " third item");
        }
    }
}