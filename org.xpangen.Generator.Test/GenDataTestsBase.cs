// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Settings;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Profile;
using Text = org.xpangen.Generator.Profile.Profile.Text;

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
            Assert.AreEqual("Minimal", d.GenDataDef.DefinitionName);
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
            Assert.AreEqual("Class", d.Context.GetValue(id));

            id = d.GenDataDef.GetId("SubClass.Name");
            Assert.AreEqual("SubClass", d.Context.GetValue(id));

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
            f.Classes[parentId].AddInstanceProperty("Name");
            var childId = f.AddClass("Parent", "Child");
            f.Classes[childId].AddInstanceProperty("Name");
            f.Classes[childId].AddInstanceProperty("Lookup");
            var lookupId = f.AddClass("Parent", "Lookup");
            f.Classes[lookupId].AddInstanceProperty("Name");

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
            def.DefinitionName = parentClassName;
            def.Cache.Internal(childDefName, defChild);
            def.AddSubClass("", parentClassName);
            def.Classes[1].AddInstanceProperty("Name");
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
            def.DefinitionName = parentClassName;
            def.AddSubClass("", parentClassName);
            def.Classes[1].AddInstanceProperty("Name");
            def.AddSubClass(parentClassName, childClassName);
            def.Classes[2].AddInstanceProperty("Name");
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
            Assert.AreEqual("SubClass" + order[0], d.Context.GetValue(id), action + " first item");
            d.Next(SubClassClassId);
            Assert.AreEqual("SubClass" + order[1], d.Context.GetValue(id), action + " second item");
            d.Next(SubClassClassId);
            Assert.AreEqual("SubClass" + order[2], d.Context.GetValue(id), action + " third item");
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

        protected static void CompareGenData(GenData expected, GenData actual)
        {
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.DataName, actual.DataName);
            Assert.AreEqual(expected.Context.Count, actual.Context.Count);
            CompareContext(0, 0, expected, actual);
        }

        private static void CompareContext(int expectedId, int actualId, GenData expected, GenData actual)
        {
            var expectedContext = expected.Context[expectedId];
            var actualContext = actual.Context[actualId];
            Assert.AreEqual(expectedContext.ToString(), actualContext.ToString());
            Assert.AreEqual(expectedContext.Count, actualContext.Count, "Class " + expectedId + " objects");
            Assert.AreEqual(expectedContext.ClassId, actualContext.ClassId);
            Assert.AreEqual(expectedContext.RefClassId, actualContext.RefClassId);
            Assert.AreEqual(expectedContext.Reference, expectedContext.Reference);
            Assert.AreEqual(expectedContext.DefClass.ToString(), actualContext.DefClass.ToString());
            expected.First(expectedId); actual.First(actualId);
            while (!expected.Eol(expectedId) && !actual.Eol(actualId))
            {
                if (expectedContext.DefSubClass != null || actualContext.DefSubClass != null)
                {
                    Assert.IsNotNull(expectedContext.DefSubClass);
                    Assert.IsNotNull(actualContext.DefSubClass);
                }
                if (expectedContext.DefSubClass != null && actualContext.DefSubClass != null)
                    Assert.AreEqual(expectedContext.DefSubClass.ToString(),
                                    actualContext.DefSubClass.ToString());
                var expectedObject = expectedContext.GenObject;
                var actualObject = actualContext.GenObject;
                Assert.AreEqual(expectedObject.ClassId, actualObject.ClassId);
                var expectedAttributes = new GenAttributes(expected.GenDataDef, expectedObject.ClassId);
                var actualAttributes = new GenAttributes(actual.GenDataDef, actualObject.ClassId);
                expectedAttributes.GenObject = expectedObject;
                actualAttributes.GenObject = actualObject;
                Assert.GreaterOrEqual(expectedObject.Attributes.Count, actualObject.Attributes.Count);
                for (var i = 0; i < actualObject.Definition.Properties.Count; i++)
                    Assert.AreEqual(expectedAttributes.AsString(actualObject.Definition.Properties[i]),
                                    actualAttributes.AsString(actualObject.Definition.Properties[i]),
                                    actualObject.Definition.Properties[i] + 
                                    " " + expectedAttributes.AsString("Name") +
                                    " vs " + actualAttributes.AsString("Name"));

                Assert.AreEqual(expectedObject.SubClass.Count, actualObject.SubClass.Count);
                for (var i = 0; i < actualObject.SubClass.Count; i++)
                {
                    var expectedSubClassDef = expected.Context[expectedId].DefClass.SubClasses[i].SubClass;
                    var actualSubClassDef = actual.Context[expectedId].DefClass.SubClasses[i].SubClass;
                    Assert.AreEqual(expectedSubClassDef.ToString(), actualSubClassDef.ToString());
                    Assert.AreEqual(expectedSubClassDef.ClassId, actualSubClassDef.ClassId);
                    Assert.AreEqual(expectedSubClassDef.IsInherited, actualSubClassDef.IsInherited);
                    Assert.AreEqual(expectedSubClassDef.IsAbstract, actualSubClassDef.IsAbstract);
                    Assert.AreEqual(expectedObject.SubClass[i].ClassId, actualObject.SubClass[i].ClassId);
                    Assert.AreEqual(expectedObject.SubClass[i].Reference, actualObject.SubClass[i].Reference);
                    CompareContext(expectedSubClassDef.ClassId,
                                   actualSubClassDef.ClassId, expected, actual);
                }

                Assert.AreEqual(expectedContext.DefClass.Inheritors.Count, actualContext.DefClass.Inheritors.Count);
                for (var i = 0; i < actualContext.DefClass.Inheritors.Count; i++)
                {
                    var expectedDefInheritor = expectedContext.DefClass.Inheritors[i];
                    var actualDefInheritor = actualContext.DefClass.Inheritors[i];
                    Assert.AreEqual(expectedDefInheritor.ClassId, actualDefInheritor.ClassId);
                    Assert.Less(expectedId, expectedDefInheritor.ClassId);
                }
                expected.Next(expectedId); actual.Next(actualId);
            }
            Assert.AreEqual(expected.Eol(expectedId), actual.Eol(actualId));
        }

        protected static void CompareGenDataBase(GenDataBase expected, GenDataBase actual)
        {
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.DataName, actual.DataName);
            CompareObject(expected.Root, actual.Root);
        }

        private static void CompareObject(GenObject expected, GenObject actual)
        {
            Assert.AreEqual(expected.ClassId, actual.ClassId);
            Assert.AreEqual(expected.Attributes.Count, actual.Attributes.Count);
            for (var i = 0; i < expected.Attributes.Count; i++)
                Assert.AreEqual(expected.Attributes[i], actual.Attributes[i]);
            Assert.AreEqual(expected.SubClass.Count, actual.SubClass.Count);
            for (var i = 0; i < expected.SubClass.Count; i++)
            {
                Assert.AreEqual(expected.SubClass[i].Count, actual.SubClass[i].Count);
                for (var j = 0; j < expected.SubClass.Count; j++)
                    CompareObject(expected.SubClass[i][j], actual.SubClass[i][j]);
            }
        }

        protected static void ValidateProfileData(GenProfileFragment profile, GenDataDef genDataDef)
        {
            var profileDefinition = profile.Profile.ProfileDefinition();
            var fragmentBodies = profileDefinition.ProfileRootList[0].FragmentBodyList;
            Assert.AreEqual("Root0", fragmentBodies[0].Name, "Root body name");
            Assert.AreEqual(1, fragmentBodies[0].FragmentList.Count);
            Assert.AreEqual(profile.Profile, fragmentBodies[0].FragmentList[0], "Profile's container");
            Assert.AreEqual("Empty1", fragmentBodies[1].Name, "Empty body name");
            Assert.AreEqual(0, fragmentBodies[1].FragmentList.Count);
            Assert.AreEqual("Profile2", fragmentBodies[2].Name, "Profile body name");
            Assert.AreEqual("Profile2", profile.Profile.Primary, "The profile's body name");
            Assert.AreEqual("Empty1", profile.Profile.Secondary, "The profile's secondary body is empty");
            ValidateProfileFragmentBodyList(fragmentBodies);
            ValidateProfileContainerData(profile, genDataDef, "");
        }

        protected static void ValidateProfileFragmentBodyList(GenNamedApplicationList<FragmentBody> fragmentBodies)
        {
            foreach (var body in fragmentBodies)
                Assert.AreEqual(1, fragmentBodies.Count(body0 => body0.Name == body.Name),
                    "Fragment body names must be unique: " + body.Name);
        }

        protected static void ValidateProfileContainerData(GenContainerFragmentBase container, GenDataDef genDataDef,
            string parentClassName)
        {
            Assert.IsInstanceOf(typeof (ContainerFragment), container.Fragment,
                "Container must have a container fragment class");
            var containerFragment = (ContainerFragment) container.Fragment;
            Assert.AreEqual(container.Body.Count, containerFragment.Body().FragmentList.Count, "Container body count");
            Assert.AreEqual(container.Body.SecondaryCount, containerFragment.SecondaryBody().FragmentList.Count,
                "Container secondary body count");
            var containerFragmentTypeName = containerFragment.GetType().Name;
            Assert.AreEqual(containerFragment.Primary.Substring(0, containerFragmentTypeName.Length),
                containerFragmentTypeName);
            if (container.Body.Count == 0)
                Assert.AreEqual("Empty1", containerFragment.Primary, "Empty primary container name");
            if (container.Body.SecondaryCount == 0)
                Assert.AreEqual("Empty1", containerFragment.Secondary, "Empty secondary container name");
            if (containerFragment.Secondary != "Empty1")
                Assert.AreEqual(containerFragment.Secondary.Substring(0, containerFragmentTypeName.Length),
                    containerFragmentTypeName);
            Assert.AreEqual(container, container.Body.ParentContainer);
            ValidateProfileContainerPartData(container.Body.Fragment, containerFragment.Body().FragmentList, genDataDef,
                parentClassName);
            ValidateProfileContainerPartData(container.Body.SecondaryFragment,
                containerFragment.SecondaryBody().FragmentList, genDataDef,
                parentClassName);
        }

        protected static void ValidateProfileContainerPartData(IList<GenFragment> genFragments, GenNamedApplicationList<Fragment> fragmentList, GenDataDef genDataDef, string parentClassName)
        {
            foreach (var genFragment in genFragments)
            {
                var fragment = genFragment.Fragment;
                Assert.AreEqual(genFragments.IndexOf(genFragment), fragmentList.IndexOf(fragment),
                    "Fragments in the same order");
                Assert.AreEqual(genFragment.FragmentType.ToString(), fragment.GetType().Name, "Fragment types");
                Assert.Contains(fragment, fragmentList, "Fragment not in correct list");
                var containerFragment = fragment as ContainerFragment;
                if (containerFragment != null)
                {
                    var genContainerFragment = (GenContainerFragmentBase) genFragment;
                    var className = parentClassName;
                    ValidateProfileSegmentData(genDataDef, containerFragment, genContainerFragment, ref className);
                    ValidateProfileTextBlockData(containerFragment, genContainerFragment);
                    ValidateProfileContainerData(genContainerFragment, genDataDef, className);
                }
            }
        }

        protected static void ValidateProfileSegmentData(GenDataDef genDataDef, ContainerFragment containerFragment,
            GenContainerFragmentBase genContainerFragment, ref string className)
        {
            var segment = containerFragment as Segment;
            if (segment == null) return;
            
            var genSegment = (GenSegment) genContainerFragment;
            className = segment.Class;
            Assert.AreEqual(genDataDef.Classes[genSegment.ClassId].Name, className, "Segment class");
            GenCardinality cardinality;
            Enum.TryParse(((Segment)genSegment.Fragment).Cardinality, out cardinality);
            Assert.AreEqual(cardinality.ToString(), segment.Cardinality);
            if (cardinality == GenCardinality.AllDlm ||
                cardinality == GenCardinality.BackDlm)
            {
                Assert.AreNotEqual("Empty1", segment.Secondary);
                Assert.AreNotEqual(0, segment.SecondaryBody().FragmentList.Count);
            }
        }

        protected static void ValidateProfileTextBlockData(ContainerFragment containerFragment, 
            GenContainerFragmentBase genContainerFragment)
        {
            var textBlock = containerFragment as TextBlock;
            if (textBlock == null) return;

            var genTextBlock = (GenTextBlock)genContainerFragment;
            var s = "";
            foreach (var fragment in textBlock.Body().FragmentList)
            {
                var text = fragment as Text;
                var placeholder = fragment as Placeholder;
                if (text != null)
                    s += text.TextValue;
                else if (placeholder != null)
                    s += String.Format("`{0}.{1}`", placeholder.Class, placeholder.Property);
            }
            Assert.AreEqual(
                genTextBlock.ProfileText(ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary), s,
                "Text block profile text");
        }
    }
}