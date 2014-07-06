// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Settings;

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
Field={Name,Inheritance,Title}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference,Relationship}
Class=Property
Field=Name
.
Class=Class[,Title='Class object']
SubClass=SubClass[]
SubClass=Property[]
Property=Name
Class=SubClass[]
Property=Name
Property=Reference
Class=Property[]
Property=Name
";

        protected const string ReferenceGenDataSaveText =
            @"Definition=Parent
Class=Parent
Field=Name
SubClass=Child[Reference='ChildDef']
.
Parent=First parent
Child[Reference='ChildDef']
";

        protected const string ReferenceGenDataSaveProfile =
            @"Definition=Parent
Class=Parent
Field=Name
SubClass=Child[Reference='Child']
.
`[Parent:Parent=`Parent.Name`
`[Child@:`]`]";

        protected const string ReferenceGrandchildDefText =
            @"Definition=Minimal
Class=Class
Field={Name,Inheritance}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference,Relationship}
Class=Property
Field=Name
.
Class=Grandchild[]
SubClass=Greatgrandchild[]
Property=Name
Class=Greatgrandchild[]
Property=Name
";

        protected const string ReferenceGrandchildText =
            @"Definition=Grandchild
Class=Grandchild
Field=Name
SubClass=Greatgrandchild
Class=Greatgrandchild
Field=Name
.
Grandchild=Grandchild
Greatgrandchild=Greatgrandchild
";

        protected const string ReferenceChildDefText =
            @"Definition=Minimal
Class=Class
Field={Name,Inheritance}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference,Relationship}
Class=Property
Field=Name
.
Class=Child[]
SubClass=Grandchild[Reference=GrandchildDef]
Property=Name
";

        protected const string ReferenceChildText =
            @"Definition=Child
Class=Child
Field=Name
SubClass=Grandchild[Reference='GrandchildDef']
.
Child=Child
Grandchild[Reference='grandchild']
";

        protected const string ReferenceParentDefText =
            @"Definition=Minimal
Class=Class
Field={Name,Inheritance}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference,Relationship}
Class=Property
Field=Name
.
Class=Parent[]
SubClass=Child[Reference=ChildDef]
Property=Name
";

        protected const string ReferenceParentText =
            @"Definition=Parent
Class=Parent
Field=Name
SubClass=Child[Reference='ChildDef']
.
Parent=Parent
Child[Reference='child']
";

        protected static void ValidateMinimalData(GenData d)
        {
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

            var ca = new GenAttributes(d.GenDataDef, ClassClassId);
            var sa = new GenAttributes(d.GenDataDef, SubClassClassId);
            var pa = new GenAttributes(d.GenDataDef, PropertyClassId);
            ca.GenObject = d.Context[ClassClassId].GenObject;
            Assert.AreEqual("Class", ca.AsString("Name"));
            Assert.AreEqual(2, d.Context[ClassClassId].GenObject.SubClass.Count);
            Assert.AreEqual(2, d.Context[SubClassClassId].Count);
            Assert.AreEqual(2, d.Context[PropertyClassId].Count);
            pa.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", pa.AsString("Name"));

            // SubClass class tests - SubClass
            Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            sa.GenObject = d.Context[SubClassClassId].GenObject;
            Assert.AreEqual("SubClass", sa.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].GenObject.SubClass.Count);

            // SubClass class tests - Property
            d.Next(SubClassClassId);
            Assert.IsTrue(d.Context[SubClassClassId].IsLast());
            sa.GenObject = d.Context[SubClassClassId].GenObject;
            Assert.AreEqual("Property", sa.AsString("Name"));
            Assert.AreEqual(0, d.Context[PropertyClassId].GenObject.SubClass.Count);

            // SubClass class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            ca.GenObject = d.Context[ClassClassId].GenObject;
            Assert.AreEqual("SubClass", ca.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(3, d.Context[PropertyClassId].Count);
            pa.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", pa.AsString("Name"));
            d.Next(PropertyClassId);
            pa.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Reference", pa.AsString("Name"));

            // Property class tests
            d.Next(ClassClassId);
            Assert.IsFalse(d.Eol(ClassClassId));
            ca.GenObject = d.Context[ClassClassId].GenObject;
            Assert.AreEqual("Property", ca.AsString("Name"));
            Assert.AreEqual(0, d.Context[SubClassClassId].Count);
            Assert.AreEqual(1, d.Context[PropertyClassId].Count);
            pa.GenObject = d.Context[PropertyClassId].GenObject;
            Assert.AreEqual("Name", pa.AsString("Name"));
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
            
            var a = new GenAttributes(d.GenDataDef, 1);
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
            var text = GenDataSaveText;
            CreateGenDataSaveText(fileName, text);
        }

        public static void CreateGenDataSaveText(string fileName, string text)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, text);
        }

        protected static GenData SetUpLookupContextData()
        {
            var f = new GenDataDef();
            var parentId = f.AddClass("", "Parent");
            f.Classes[parentId].InstanceProperties.Add("Name");
            var childId = f.AddClass("Parent", "Child");
            f.Classes[childId].InstanceProperties.Add("Name");
            f.Classes[childId].InstanceProperties.Add("Lookup");
            var lookupId = f.AddClass("Parent", "Lookup");
            f.Classes[lookupId].InstanceProperties.Add("Name");

            var a = new GenAttributes(f, 1);
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

        private static GenDataDef CreateSelfReferenceDefinition(string reference)
        {
            var f = new GenDataDef();
            f.AddSubClass("", "Root");
            f.AddSubClass("Root", "ReferenceData");
            f.AddSubClass("Root", "BaseData");
            f.Classes[f.Classes.IndexOf("Root")].InstanceProperties.Add("Name");
            f.Classes[f.Classes.IndexOf("ReferenceData")].InstanceProperties.Add("Name");
            f.Classes[f.Classes.IndexOf("ReferenceData")].InstanceProperties.Add("Value");
            f.Classes[f.Classes.IndexOf("BaseData")].InstanceProperties.Add("Name");
            f.Classes[f.Classes.IndexOf("BaseData")].InstanceProperties.Add("ReferenceKey");
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

        protected static GenData SetUpParentChildReferenceData(string parentClassName, string childClassName, string childDefName, string childDataName, GenData dataChild)
        {
            var def = SetUpParentChildReferenceDef(parentClassName, childClassName, childDefName, dataChild.GenDataDef);
            var data = new GenData(def) { DataName = parentClassName };
            SetUpParentOtherChildReferenceData(parentClassName, childClassName, childDataName, dataChild, data);
            return data;
        }

        protected static void SetUpParentOtherChildReferenceData(string parentClassName, string childClassName,
                                                                 string childDataName, GenData dataChild, GenData data)
        {
            CreateGenObject(data, "", parentClassName, parentClassName);
            SetUpParentReference(data, dataChild, childClassName + "Def", parentClassName, childClassName, childDataName);
        }

        protected static GenDataDef SetUpParentChildReferenceDef(string parentClassName, string childClassName,
                                                               string childDefName, GenDataDef defChild)
        {
            var def = new GenDataDef();
            def.Definition = parentClassName;
            def.Cache.Internal(childDefName, defChild);
            def.AddSubClass("", parentClassName);
            def.Classes[1].InstanceProperties.Add("Name");
            def.AddSubClass(parentClassName, childClassName, childDefName);
            return def;
        }

        protected static GenData SetUpParentChildData(string parentClassName, string childClassName, string childDataName)
        {
            var def = SetUpParentChildDef(parentClassName, childClassName);
            var data = new GenData(def) { DataName = parentClassName};
            CreateGenObject(data, "", parentClassName, parentClassName);
            CreateGenObject(data, parentClassName, childClassName, childDataName);
            //data.First(0);
            return data;
        }

        protected static GenDataDef SetUpParentChildDef(string parentClassName, string childClassName)
        {
            var def = new GenDataDef();
            def.Definition = parentClassName;
            def.AddSubClass("", parentClassName);
            def.Classes[1].InstanceProperties.Add("Name");
            def.AddSubClass(parentClassName, childClassName);
            def.Classes[2].InstanceProperties.Add("Name");
            return def;
        }

        protected static void SetUpParentReference(GenData dataParent, GenData dataChild, string childDefName,
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
            var sub = data.Context[classId].GenObject.SubClass[subClassIndex] as SubClassReference;
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

        protected static void CompareGenDataDef(GenDataDef expected, GenDataDef actual, string path)
        {
            //Assert.AreEqual(expected.Definition, actual.Definition);
            Assert.AreEqual(expected.Classes.Count, actual.Classes.Count, path);
            for (var i = 0; i < expected.Classes.Count; i++)
            {
                var expClass = expected.Classes[i];
                var actClass = actual.Classes[i];
                Assert.AreEqual(expClass.ToString(), actClass.ToString(), path);
                Assert.AreEqual(expClass.Properties.Count, actClass.Properties.Count, path + '.' + expClass);
                for (var j = 0; j < expClass.Properties.Count; j++)
                {
                    Assert.AreEqual(expClass.Properties[j], actClass.Properties[j], path + '.' + expClass);
                }
                if (expClass.RefDef != null)
                {
                    Assert.IsNotNull(actClass.RefDef, path + '.' + expClass + " RefDef");
                    Assert.AreEqual(expClass.RefDef.ToString(), actClass.RefDef.ToString(),
                                path + '.' + expClass + " RefDef");
                }
                Assert.AreEqual(expClass.RefClassId, actClass.RefClassId, path + '.' + expClass + " RefClassId");
                Assert.AreEqual(expClass.IsReference, actClass.IsReference, path + '.' + expClass + " RefClassId");
            }
            Assert.AreEqual(expected.Cache.Count, actual.Cache.Count, path);
            var expectedReferences = expected.Cache.References;
            for (var i = 0; i < expectedReferences.Count; i++)
            {
                CompareGenDataDef(expectedReferences[i].GenDataDef, actual.Cache[expectedReferences[i].Path],
                                  expectedReferences[i].Path);
            }
        }

        protected static GeneratorEditor PopulateGenSettings()
        {
            var f = GenData.DataLoader.LoadData("data/GeneratorEditor").AsDef();
            var d = new GenData(f);
            var model = new GeneratorEditor(d) {GenObject = d.Root};
            model.SaveFields();
            var settings = new GenSettings(d) {GenObject = d.CreateObject("", "GenSettings"), HomeDir = "."};
            model.GenSettingsList.Add(settings);
            settings.BaseFileList.Add(new BaseFile(d)
                                          {
                                              GenObject = d.CreateObject("GenSettings", "BaseFile"),
                                              Name = "Minimal",
                                              Title = "The simplest definition required by the generator",
                                              FileName = "Minimal.dcb",
                                              FilePath = "Data",
                                              FileExtension = ".dcb"
                                          });
            settings.BaseFileList.Add(new BaseFile(d)
                                          {
                                              GenObject = d.CreateObject("GenSettings", "BaseFile"),
                                              Name = "Definition",
                                              Title = "The definition required by the editor",
                                              FileName = "Definition.dcb",
                                              FilePath = "Data",
                                              FileExtension = ".dcb"
                                          });
            var baseFile = new BaseFile(d)
                               {
                                   GenObject = d.CreateObject("GenSettings", "BaseFile"),
                                   Name = "ProgramDefinition",
                                   Title = "Defines generator editor data models",
                                   FileName = "ProgramDefinition.dcb",
                                   FilePath = "Data",
                                   FileExtension = ".dcb"
                               };
            baseFile.ProfileList.Add(new Data.Model.Settings.Profile(d)
                                         {
                                             GenObject = d.CreateObject("BaseFile", "Profile"),
                                             Name = "GenProfileModel",
                                             Title = "",
                                             FileName = "GenProfileModel.prf",
                                             FilePath = "Data"
                                         });
            settings.BaseFileList.Add(baseFile);
            settings.BaseFileList.Add(new BaseFile(d)
                                          {
                                              GenObject = d.CreateObject("GenSettings", "BaseFile"),
                                              Name = "GeneratorEditor",
                                              Title = "Defines generator editor settings data",
                                              FileName = "GeneratorEditor.dcb",
                                              FilePath = "Data",
                                              FileExtension = ".dcb"
                                          });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "Minimal",
                                               FileName = "Minimal.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "Basic",
                                               FileName = "Basic.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "Definition",
                                               FileName = "Definition.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "ProgramDefinition",
                                               FileName = "ProgramDefinition.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "GeneratorEditor",
                                               FileName = "GeneratorEditor.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "Definition"
                                           });
            settings.FileGroupList.Add(new FileGroup(d)
                                           {
                                               GenObject = d.CreateObject("GenSettings", "FileGroup"),
                                               Name = "GeneratorDefinitionModel",
                                               FileName = "GeneratorDefinitionModel.dcb",
                                               FilePath = "Data",
                                               BaseFileName = "ProgramDefinition",
                                               Profile = "GenProfileModel"
                                           });
            d.First(1);
            return model;
        }
    }
}