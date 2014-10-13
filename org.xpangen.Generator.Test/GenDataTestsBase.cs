// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Definition;
using org.xpangen.Generator.Data.Model.Settings;
using org.xpangen.Generator.Parameter;
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
Grandchild[Reference='Grandchild']
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
Child[Reference='Child']
";

        protected string GetTestDataFileName(string testName)
        {
            return "TestData\\" + testName + ".dcb";
        }

        protected static void ValidateMinimalData(GenDataBase d)
        {
            Assert.AreEqual("Minimal", d.GenDataDef.DefinitionName);
            Assert.AreEqual(4, d.GenDataDef.Classes.Count);

            //Assert.IsFalse(d.Eol(RootClassId));
            //Assert.IsFalse(d.Eol(ClassClassId));
            //Assert.IsFalse(d.Eol(SubClassClassId));
            //Assert.IsFalse(d.Eol(PropertyClassId));

            //Assert.AreEqual(RootClassId, d.Context[RootClassId].ClassId);
            //Assert.AreEqual(ClassClassId, d.Context[ClassClassId].ClassId);
            //Assert.AreEqual(SubClassClassId, d.Context[SubClassClassId].ClassId);
            //Assert.AreEqual(PropertyClassId, d.Context[PropertyClassId].ClassId);

            //Assert.IsTrue(d.Context[RootClassId].IsFirst());
            //Assert.IsTrue(d.Context[RootClassId].IsLast());
            //Assert.AreEqual(1, d.Context[RootClassId].GenObject.SubClass.Count);

            // Class class tests
            //d.First(ClassClassId);
            var c = GetFirstObject(d);
            Assert.IsNotNull(c);
            Assert.AreNotEqual(0, c.GetSubClass("SubClass").Count);
            Assert.AreNotEqual(0, c.GetSubClass("Property").Count);
            //Assert.IsTrue(d.Context[ClassClassId].IsFirst());
            //Assert.IsTrue(d.Context[SubClassClassId].IsFirst());
            //Assert.IsTrue(d.Context[PropertyClassId].IsFirst());

            var ca = new GenAttributes(d.GenDataDef, ClassClassId);
            var sa = new GenAttributes(d.GenDataDef, SubClassClassId);
            var pa = new GenAttributes(d.GenDataDef, PropertyClassId);
            ca.GenObject = c;
            Assert.AreEqual("Class", ca.AsString("Name"));
            Assert.AreEqual(2, c.SubClass.Count);
            Assert.AreEqual(2, c.GetSubClass("SubClass").Count);
            Assert.AreEqual(2, c.GetSubClass("Property").Count);
            pa.GenObject = c.GetSubClass("Property")[0];
            Assert.AreEqual("Name", pa.AsString("Name"));

            // SubClass class tests - SubClass
            var s = GetFirstObjectOfSubClass(c, "SubClass");
            Assert.IsNotNull(s);
            sa.GenObject = s;
            Assert.AreEqual("SubClass", sa.AsString("Name"));
            Assert.AreEqual(0, s.SubClass.Count);

            // SubClass class tests - Property
            var p = GetNextObjectInSubClass(s);
            Assert.AreEqual(p.GetSubClass("SubClass").Count - 1, p.GetSubClass("SubClass").IndexOf(p));
            sa.GenObject = p;
            Assert.AreEqual("Property", sa.AsString("Name"));
            Assert.AreEqual(0, p.SubClass.Count);

            // SubClass class tests
            s = GetNextObjectInSubClass(c);
            Assert.IsNotNull(s);
            ca.GenObject = s;
            Assert.AreEqual("SubClass", ca.AsString("Name"));
            Assert.AreEqual(0, s.GetSubClass("SubClass").Count);
            Assert.AreEqual(3, s.GetSubClass("Property").Count);
            pa.GenObject = s.GetSubClass("Property")[0];
            Assert.AreEqual("Name", pa.AsString("Name"));
            pa.GenObject = GetNextObjectInSubClass((GenObject) pa.GenObject);
            Assert.AreEqual("Reference", pa.AsString("Name"));
            pa.GenObject = GetNextObjectInSubClass((GenObject) pa.GenObject);
            Assert.AreEqual("Relationship", pa.AsString("Name"));

            // Property class tests
            p = GetNextObjectInSubClass(s);
            Assert.IsNotNull(p);
            ca.GenObject = p;
            Assert.AreEqual("Property", ca.AsString("Name"));
            Assert.AreEqual(0, p.GetSubClass("SubClass").Count);
            Assert.AreEqual(1, p.GetSubClass("Property").Count);
            pa.GenObject = p.GetSubClass("Property")[0];
            Assert.AreEqual("Name", pa.AsString("Name"));
        }

        protected static GenObject CreateClass(GenDataBase d, string name)
        {
            var c = CreateGenObject(d.Root, "Class", name);
            CreateProperty("Name", c);
            return c;
        }

        protected static GenObject CreateGenObject(GenObject parent, string className, string name = null)
        {
            var o = parent.CreateGenObject(className);
            if (name != null)
            {
                if (o.Attributes.Count == 0)
                    o.Attributes.Add(name);
                else
                    o.Attributes[0] = name;
            }
            return o;
        }

        protected static void CreateProperty(string name, GenObject parent)
        {
            CreateGenObject(parent, "Property", name);
        }

        protected static void VerifyAsDef(GenDataDef f)
        {
            Assert.AreEqual(0, f.GetClassId(""));
            Assert.AreEqual(ClassClassId, f.GetClassId("Class"));
            Assert.AreEqual(SubClassClassId, f.GetClassId("SubClass"));
            Assert.AreEqual(PropertyClassId, f.GetClassId("Property"));
            Assert.AreEqual(1, f.GetClassSubClasses(0).Count);
            Assert.AreEqual(2, f.GetClassSubClasses(ClassClassId).Count);
            Assert.AreEqual(0, f.GetClassSubClasses(SubClassClassId).Count);
            Assert.AreEqual(0, f.GetClassSubClasses(PropertyClassId).Count);
            Assert.AreEqual(ClassClassId, f.GetClassSubClasses(0)[0].SubClass.ClassId);
            Assert.AreEqual(SubClassClassId, f.GetClassSubClasses(ClassClassId)[0].SubClass.ClassId);
            Assert.AreEqual(PropertyClassId, f.GetClassSubClasses(ClassClassId)[1].SubClass.ClassId);
        }

        protected static void VerifyDataCreation(GenDataBase d)
        {
            var c = GetFirstObject(d);
            var s = GetFirstObjectOfSubClass(c, "SubClass");
            
            var id = d.GenDataDef.GetId("Class.Name");
            Assert.AreEqual("Class", c.GetValue(id));

            id = d.GenDataDef.GetId("SubClass.Name");
            Assert.AreEqual("SubClass", s.GetValue(id));
        }

        protected static void SetUpData(GenDataBase genDataBase)
        {
            var c = CreateGenObject(genDataBase.Root, "Class", "Class");
            CreateGenObject(c, "Property", "Name");
            var sc = CreateGenObject(genDataBase.Root, "Class", "SubClass");
            CreateGenObject(sc, "Property", "Name");
            CreateGenObject(sc, "Property", "Reference");
            var p = CreateGenObject(genDataBase.Root, "Class", "Property");
            CreateGenObject(p, "Property", "Name");
            CreateGenObject(c, "SubClass", "SubClass");
            CreateGenObject(c, "SubClass", "Property");
        }

        protected static void CreateGenDataSaveText(string fileName, string text = GenDataSaveText)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, text);
        }

        protected static GenDataBase SetUpLookupContextData()
        {
            var f = new GenDataDef();
            var parentId = f.AddClass("", "Parent");
            f.AddClassInstanceProperty(parentId, "Name");
            var childId = f.AddClass("Parent", "Child");
            f.AddClassInstanceProperty(childId, "Name");
            f.AddClassInstanceProperty(childId, "Lookup");
            var lookupId = f.AddClass("Parent", "Lookup");
            f.AddClassInstanceProperty(lookupId, "Name");

            var a = new GenAttributes(f, 1);
            var d = new GenDataBase(f);
            var p = CreateGenObject(d.Root, "Parent", "Parent");
            a.GenObject = CreateGenObject(p, "Child", "Child1");
            a.SetString("Lookup", "Valid");
            a.SaveFields();

            a.GenObject = CreateGenObject(p, "Child", "Child2");
            a.SetString("Lookup", "Invalid");
            a.SaveFields();

            a.GenObject = CreateGenObject(p, "Lookup", "Valid");
            
            return d;
        }

        protected static GenDataBase SetUpLookupData()
        {
            var f = GenDataDef.CreateMinimal();

            var d = SetUpLookupData(f);
            return d;
        }

        private static GenDataBase SetUpLookupData(GenDataDef f)
        {
            var d = new GenDataBase(f);
            var c = CreateGenObject(d.Root, "Class", "Class");
            CreateGenObject(c, "Property", "Name");
            var sc = CreateGenObject(d.Root, "Class", "SubClass");
            CreateGenObject(sc, "Property", "Name");
            var p = CreateGenObject(d.Root, "Class", "Property");
            CreateGenObject(p, "Property", "Name");
            CreateGenObject(c, "SubClass", "SubClass");
            CreateGenObject(c, "SubClass", "Property");
            return d;
        }

        protected static GenDataBase SetUpParentChildReferenceData(string parentClassName, string childClassName, 
            string childDefName, string childDataName, GenDataBase dataChildBase)
        {
            var def = SetUpParentChildReferenceDef(parentClassName, childClassName, childDefName, dataChildBase.GenDataDef);
            var data = new GenDataBase(def) { DataName = parentClassName };
            SetUpParentOtherChildReferenceData(parentClassName, childClassName, childDataName, data,
                dataChildBase);
            return data;
        }

        private static void SetUpParentOtherChildReferenceData(string parentClassName, string childClassName, 
            string childDataName, GenDataBase dataParent, GenDataBase dataChild)
        {
            var parent = CreateGenObject(dataParent.Root, parentClassName, parentClassName);
            SetUpParentReference(childClassName + "Def", childClassName, childDataName, dataParent,
                dataChild, parent);
        }

        protected static GenDataDef SetUpParentChildReferenceDef(string parentClassName, string childClassName,
                                                               string childDefName, GenDataDef defChild)
        {
            var def = new GenDataDef {DefinitionName = parentClassName};
            def.Cache.Internal(childDefName, defChild);
            def.AddSubClass("", parentClassName);
            def.AddClassInstanceProperty(1, "Name");
            def.AddSubClass(parentClassName, childClassName, childDefName);
            return def;
        }

        protected static GenDataBase SetUpParentChildData(string parentClassName, string childClassName, string childDataName)
        {
            var def = SetUpParentChildDef(parentClassName, childClassName);
            var data = new GenDataBase(def) { DataName = parentClassName};
            var parent = CreateGenObject(data.Root, parentClassName, parentClassName);
            CreateGenObject(parent, childClassName, childDataName);
            return data;
        }

        protected static GenDataDef SetUpParentChildDef(string parentClassName, string childClassName)
        {
            var def = new GenDataDef {DefinitionName = parentClassName};
            def.AddSubClass("", parentClassName);
            def.AddClassInstanceProperty(1, "Name");
            SetUpChildDef(parentClassName, childClassName, def);
            return def;
        }

        protected static void SetUpChildDef(string parentClassName, string childClassName, GenDataDef def)
        {
            def.AddSubClass(parentClassName, childClassName);
            var childClassId = def.GetClassId(childClassName);
            def.AddClassInstanceProperty(childClassId, "Name");
        }

        private static void SetUpParentReference(string childDefName, string childClassName, 
            string childName, GenDataBase dataParent, GenDataBase dataChild, GenObject parent)
        {
            dataParent.CheckReference(childDefName, dataChild.GenDataDef.AsGenDataBase());
            dataParent.CheckReference(childName, dataChild);
            SetSubClassReference(parent, childClassName, childName);
        }

        /// <summary>
        /// Get the first object in the data.
        /// </summary>
        /// <param name="d">The data containing the object.</param>
        /// <returns>The first classId 1 object if it exists, otherwise null.</returns>
        protected static GenObject GetFirstObject(GenDataBase d)
        {
            if (d.Root == null) return null;
            if (d.Root.SubClass.Count == 0) return null;
            if (d.Root.SubClass[0].Count == 0) return null;
            return d.Root.SubClass[0][0];
        }

        protected static GenObject GetFirstObjectOfSubClass(GenObject genObject, string subClassName)
        {
            Contract.Requires(genObject != null, "The object cannot be null");
            Contract.Requires(genObject.Definition != null, "The object definition is required");
            var index = genObject.Definition.IndexOfSubClass(subClassName);
            if (index == -1) throw new ArgumentException("Class is not a subclass", subClassName);
            var subClass = genObject.GetSubClass(subClassName);
            return subClass.Count == 0 ? null : subClass[0];
        }


        protected static GenObject GetNextObjectInSubClass(GenObject genObject)
        {
            var i = genObject.ParentSubClass.IndexOf(genObject);
            if (i > genObject.ParentSubClass.Count) return null;
            return genObject.ParentSubClass[i + 1];
        }

        protected static GenObject GetLastObjectInSubClass(GenObject genObject)
        {
            if (genObject.ParentSubClass.Count == 0) return null;
            return genObject.ParentSubClass[genObject.ParentSubClass.Count - 1];
        }

        /// <summary>
        /// Set the subclass reference.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="subClassName">The childe class name.</param>
        /// <param name="reference">The reference path.</param>
        private static void SetSubClassReference(GenObject parent, string subClassName, string reference)
        {
            var subClassIndex = parent.Definition.SubClasses.IndexOf(subClassName);
            var sub = parent.SubClass[subClassIndex] as SubClassReference;
            if (sub != null)
                sub.Reference = reference;
        }

        protected static void MoveItem(GenDataBase d, ListMove move, int itemIndex, string order, string action)
        {
            var c = GetFirstObject(d);
            var sc = c.GetSubClass("SubClass");
            sc.Move(move, itemIndex);
            CheckOrder(d, order, action);
        }

        protected static void CheckOrder(GenDataBase d1, string order, string action)
        {
            var d = d1;
            var id = d.GenDataDef.GetId("SubClass.Name");
            var s0 = GenObject.GetContext(d.Root, "SubClass");
            var s1 = GetNextObjectInSubClass(s0);
            var s2 = GetNextObjectInSubClass(s1);
            Assert.AreEqual("SubClass" + order[0], s0.GetValue(id), action + " first item");
            Assert.AreEqual("SubClass" + order[1], s1.GetValue(id), action + " second item");
            Assert.AreEqual("SubClass" + order[2], s2.GetValue(id), action + " third item");
        }

        protected static void CompareGenDataDef(GenDataDef expected, GenDataDef actual, string path)
        {
            Assert.AreEqual(expected.Classes.Count, actual.Classes.Count, path);
            for (var i = 0; i < expected.Classes.Count; i++)
            {
                var expClass = expected.GetClassDef(i);
                var actClass = actual.GetClassDef(i);
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
            foreach (GenDataDefReferenceCacheItem expectedReference in expectedReferences)
            {
                CompareGenDataDef(expectedReference.GenDataDef, actual.Cache[expectedReference.Path],
                    expectedReference.Path);
            }
        }

        protected static GeneratorEditor PopulateGenSettings()
        {
            var f = GenDataBase.DataLoader.LoadData("data/GeneratorEditor").AsDef();
            var d = new GenDataBase(f);
            var model = new GeneratorEditor(d) {GenObject = d.Root};
            model.SaveFields();
            var settings = new GenSettings(d) {GenObject = CreateGenObject(d.Root, "GenSettings"), HomeDir = "."};
            model.GenSettingsList.Add(settings);
            settings.AddBaseFile("Minimal", "Minimal.dcb", "Data", "The simplest definition required by the generator",
                ".dcb");
            settings.AddBaseFile("Definition", "Definition.dcb", "Data", "The definition required by the editor", ".dcb");
            var baseFile = settings.AddBaseFile("ProgramDefinition", "ProgramDefinition.dcb", "Data",
                "Defines generator editor data models", ".dcb");
            baseFile.AddProfile("GenProfileModel", "GenProfileModel.prf", "Data");
            settings.AddBaseFile("GeneratorEditor", "GeneratorEditor.dcb", "Data",
                "Defines generator editor settings data", ".dcb");
            settings.AddFileGroup("Minimal", "Minimal.dcb", "Data", "Definition");
            settings.AddFileGroup("Basic", "Basic.dcb", "Data", "Definition");
            settings.AddFileGroup("Definition", "Definition.dcb", "Data", "Definition");
            settings.AddFileGroup("ProgramDefinition", "ProgramDefinition.dcb", "Data", "Definition");
            settings.AddFileGroup("GeneratorEditor", "GeneratorEditor.dcb", "Data", "Definition");
            settings.AddFileGroup("GeneratorDefinitionModel", "GeneratorDefinitionModel.dcb", "Data",
                "ProgramDefinition", "GenProfileModel");
            return model;
        }

        protected static void CompareGenData(GenDataBase expected, GenDataBase actual)
        {
            Assert.AreEqual(expected.DataName, actual.DataName);
            Assert.AreEqual(expected.GenDataDef.Classes.Count, actual.GenDataDef.Classes.Count);
            CompareContext(expected.Root.SubClass[0], actual.Root.SubClass[0]);
        }

        private static void CompareContext(ISubClassBase expectedList, ISubClassBase actualList)
        {
            var expectedId = expectedList.ClassId;
            var actualId = actualList.ClassId;
            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedList.Count, actualList.Count, "Class " + expectedId + " objects");
            Assert.AreEqual(expectedList.ClassId, actualList.ClassId);
            Assert.AreEqual(expectedList.Definition.SubClass.RefClassId, actualList.Definition.SubClass.RefClassId);
            Assert.AreEqual(expectedList.Reference, expectedList.Reference);
            Assert.AreEqual(expectedList.Definition.SubClass.ToString(), actualList.Definition.SubClass.ToString());
            for (var i = 0; i < expectedList.Count; i++)
            {
                var expectedObject = expectedList[i];
                var actualObject = actualList[i];
                Assert.AreEqual(expectedObject.ClassId, actualObject.ClassId);
                var expectedAttributes = new GenAttributes(expectedObject.GenDataBase.GenDataDef, expectedObject.ClassId);
                var actualAttributes = new GenAttributes(actualObject.GenDataBase.GenDataDef, actualObject.ClassId);
                expectedAttributes.GenObject = expectedObject;
                actualAttributes.GenObject = actualObject;
                Assert.GreaterOrEqual(expectedObject.Attributes.Count, actualObject.Attributes.Count);
                foreach (var property in actualObject.Definition.Properties)
                    Assert.AreEqual(expectedAttributes.AsString(property),
                        actualAttributes.AsString(property),
                        property + " " + expectedAttributes.AsString("Name") + " vs " +
                        actualAttributes.AsString("Name"));

                Assert.AreEqual(expectedObject.SubClass.Count, actualObject.SubClass.Count);
                for (var j = 0; j < actualObject.SubClass.Count; j++)
                {
                    var expectedSubClassName = expectedObject.SubClass[j].Definition.SubClass.Name;
                    var actualSubClassName = actualObject.SubClass[j].Definition.SubClass.Name;
                    Assert.AreEqual(expectedSubClassName, actualSubClassName);
                    var expectedSubClassDef = expectedObject.GetSubClass(expectedSubClassName).Definition.SubClass;
                    var actualSubClassDef = actualObject.GetSubClass(actualSubClassName).Definition.SubClass;
                    Assert.AreEqual(expectedSubClassDef.ToString(), actualSubClassDef.ToString());
                    Assert.AreEqual(expectedSubClassDef.ClassId, actualSubClassDef.ClassId);
                    Assert.AreEqual(expectedSubClassDef.IsInherited, actualSubClassDef.IsInherited);
                    Assert.AreEqual(expectedSubClassDef.IsAbstract, actualSubClassDef.IsAbstract);
                    Assert.AreEqual(expectedObject.SubClass[j].ClassId, actualObject.SubClass[j].ClassId);
                    Assert.AreEqual(expectedObject.SubClass[j].Reference, actualObject.SubClass[j].Reference);
                    CompareContext(expectedObject.SubClass[j], actualObject.SubClass[j]);
                }

                Assert.AreEqual(expectedObject.Definition.Inheritors.Count, actualObject.Definition.Inheritors.Count);
                for (var j = 0; j < expectedObject.Definition.Inheritors.Count; j++)
                {
                    var expectedDefInheritor = expectedObject.Definition.Inheritors[j];
                    var actualDefInheritor = actualObject.Definition.Inheritors[j];
                    Assert.AreEqual(expectedDefInheritor.ClassId, actualDefInheritor.ClassId);
                    Assert.Less(expectedId, expectedDefInheritor.ClassId);
                }
            }
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
            var profileDataDef = profile.Profile.GenDataBase.GenDataDef;
            VerifyObjectClass(profileDataDef, "Profile", profile.Profile.GenObject);
            var profileDefinition = profile.Profile.ProfileDefinition();
            VerifyObjectClass(profileDataDef, "ProfileRoot", profileDefinition.ProfileRootList[0].GenObject);
            var fragmentBodies = profileDefinition.ProfileRootList[0].FragmentBodyList;
            Assert.AreEqual("Root0", fragmentBodies[0].Name, "Root body name");
            Assert.AreEqual(1, fragmentBodies[0].FragmentList.Count);
            Assert.AreEqual(profile.Profile, fragmentBodies[0].FragmentList[0], "Profile's container");
            Assert.AreEqual("Empty1", fragmentBodies[1].Name, "Empty body name");
            Assert.AreEqual(0, fragmentBodies[1].FragmentList.Count);
            Assert.AreEqual("Profile2", fragmentBodies[2].Name, "Profile body name");
            Assert.AreEqual("Profile2", profile.Profile.Primary, "The profile's body name");
            Assert.AreEqual("Empty1", profile.Profile.Secondary, "The profile's secondary body is empty");
            ValidateProfileFragmentBodyList(profileDataDef, fragmentBodies);
            ValidateProfileContainerData(profile, genDataDef, "", profileDataDef);
        }

        private static void VerifyObjectClass(GenDataDef profileDataDef, string className, IGenObject genObject)
        {
            Assert.AreEqual(profileDataDef.Classes.IndexOf(className), genObject.ClassId, className + " class ID");
            Assert.AreEqual(className, genObject.ClassName, "Object Class name");
        }

        private static void ValidateProfileFragmentBodyList(GenDataDef profileDataDef, GenNamedApplicationList<FragmentBody> fragmentBodies)
        {
            foreach (var body in fragmentBodies)
            {
                VerifyObjectClass(profileDataDef, "FragmentBody", body.GenObject);
                Assert.AreEqual(1, fragmentBodies.Count(body0 => body0.Name == body.Name),
                    "Fragment body names must be unique: " + body.Name);
            }
        }

        private static void ValidateProfileContainerData(GenContainerFragmentBase container, GenDataDef genDataDef, string parentClassName, GenDataDef profileDataDef)
        {
            Assert.IsInstanceOf(typeof (ContainerFragment), container.Fragment,
                "Container must have a container fragment class");
            var containerFragment = (ContainerFragment) container.Fragment;
            Assert.AreEqual(container.Body.Count, containerFragment.Body().FragmentList.Count, "Container body count");
            Assert.AreEqual(container.Body.SecondaryCount, containerFragment.SecondaryBody().FragmentList.Count,
                "Container secondary body count");
            var containerFragmentTypeName = containerFragment.GetType().Name;
            if (containerFragment.Primary != "Empty1")
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
            ValidateProfileContainerPartData(container.Body.Fragment, containerFragment.Body().FragmentList,
                genDataDef, parentClassName, profileDataDef);
            ValidateProfileContainerPartData(container.Body.SecondaryFragment,
                containerFragment.SecondaryBody().FragmentList, genDataDef, parentClassName, profileDataDef);
        }

        private static void ValidateProfileContainerPartData(IList<GenFragment> genFragments, List<Fragment> fragmentList, GenDataDef genDataDef, string parentClassName, GenDataDef profileDataDef)
        {
            foreach (var genFragment in genFragments)
            {
                var fragment = genFragment.Fragment;
                Assert.AreEqual(genFragments.IndexOf(genFragment), fragmentList.IndexOf(fragment),
                    "Fragments in the same order");
                Assert.AreEqual(genFragment.FragmentType.ToString(), fragment.GetType().Name, "Fragment types");
                Assert.Contains(fragment, fragmentList, "Fragment not in correct list");
                ValidateFragmentData(genDataDef, parentClassName, genFragment, profileDataDef);
            }
        }

        protected static void ValidateFragmentData(GenDataDef genDataDef, string parentClassName, GenFragment genFragment, GenDataDef profileDataDef)
        {
            var fragment = genFragment.Fragment;
            VerifyObjectClass(profileDataDef, genFragment.FragmentType.ToString(), fragment.GenObject);
            var containerFragment = fragment as ContainerFragment;
            if (containerFragment != null)
            {
                var genContainerFragment = (GenContainerFragmentBase) genFragment;
                var className = parentClassName;
                ValidateProfileSegmentData(genDataDef, containerFragment, genContainerFragment, ref className);
                ValidateProfileTextBlockData(containerFragment, genContainerFragment);
                ValidateProfileContainerData(genContainerFragment, genDataDef, className, profileDataDef);
            }
        }

        private static void ValidateProfileSegmentData(GenDataDef genDataDef, ContainerFragment containerFragment,
            GenContainerFragmentBase genContainerFragment, ref string className)
        {
            var segment = containerFragment as Segment;
            if (segment == null) return;
            
            var genSegment = (GenSegment) genContainerFragment;
            className = segment.Class;
            Assert.AreEqual(genDataDef.GetClassName(genSegment.ClassId), className, "Segment class");
            var cardinality = ((Segment)genSegment.Fragment).GenCardinality;
            Assert.AreEqual(cardinality.ToString(), segment.Cardinality);
            if (cardinality == GenCardinality.AllDlm ||
                cardinality == GenCardinality.BackDlm)
            {
                Assert.AreNotEqual("Empty1", segment.Secondary);
                Assert.AreNotEqual(0, segment.SecondaryBody().FragmentList.Count);
            }
        }

        private static void ValidateProfileTextBlockData(ContainerFragment containerFragment, 
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

        protected const string VirtualDefinition = @"Definition=Definition
Class=Class
Field={Name,Title,Inheritance}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference,Relationship}
Class=Property
Field={Name,Title,DataType,Default,LookupType,LookupDependence,LookupTable}
.
Class=Container[]
SubClass=Abstract[]
Property=Name[,DataType=Identifier]
Class=Abstract[Title='Abstract class',Inheritance=Abstract]
SubClass=Virtual1[,Relationship=Extends]
SubClass=Virtual2[,Relationship=Extends]
SubClass=Child[]
Property=Name[,DataType=Identifier]
Class=Virtual1[]
Property=V1Field[,DataType=String]
Class=Virtual2[]
Property=V2Field[,DataType=String]
Class=Child[]
Property=Name[,DataType=Identifier]
";
        protected const string VirtualDefinitionProfile = @"Definition=VirtualDefinition
Class=Container
Field=Name
SubClass=Abstract
Class=Abstract[Virtual1,Virtual2]
Field=Name
SubClass=Child
Class=Virtual1
Field=V1Field
Class=Virtual2
Field=V2Field
Class=Child
Field=Name
.
`[Container:Container=`Container.Name`
`[Abstract:`[Virtual1^:Virtual1=`Virtual1.Name`[`?Virtual1.V1Field:V1Field`?Virtual1.V1Field<>True:=`@StringOrName:`{`Virtual1.V1Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`[Virtual2^:Virtual2=`Virtual2.Name`[`?Virtual2.V2Field:V2Field`?Virtual2.V2Field<>True:=`@StringOrName:`{`Virtual2.V2Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`]`]";
        protected const string VirtualDefinitionData = @"Definition=VirtualDefinition
Class=Container
Field=Name
SubClass=Abstract
Class=Abstract[Virtual1,Virtual2]
Field=Name
SubClass=Child
Class=Virtual1
Field=V1Field
Class=Virtual2
Field=V2Field
Class=Child
Field=Name
.
Container=Container
Virtual1=V1Instance1[V1Field='Value 1']
Child=V1I1Child1
Child=V1I1Child2
Virtual2=V2Instance1[V2Field='Value 1']
Child=V2I1Child1
Child=V2I1Child2
Virtual1=V1Instance2[V1Field='Value 2']
Child=V1I2Child1
Child=V1I2Child2
Virtual2=V2Instance2[V2Field='Value 2']
Child=V2I2Child1
Child=V2I2Child2
";
        protected const string InheritanceProfile = @"`[Container:Container=`Container.Name`
`[Virtual1:Virtual1=`Virtual1.Name`[`?Virtual1.V1Field:V1Field`?Virtual1.V1Field<>True:=`@StringOrName:`{`Virtual1.V1Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`[Virtual2:Virtual2=`Virtual2.Name`[`?Virtual2.V2Field:V2Field`?Virtual2.V2Field<>True:=`@StringOrName:`{`Virtual2.V2Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`]";
        protected const string NestedInheritanceProfile = @"`[Parent:`[Container:Container=`Container.Name`
`[Virtual1:Virtual1=`Virtual1.Name`[`?Virtual1.V1Field:V1Field`?Virtual1.V1Field<>True:=`@StringOrName:`{`Virtual1.V1Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`[Virtual2:Virtual2=`Virtual2.Name`[`?Virtual2.V2Field:V2Field`?Virtual2.V2Field<>True:=`@StringOrName:`{`Virtual2.V2Field``]`]`]`]]
`[Child:Child=`Child.Name`
`]`]`]`]";
        protected const string InheritanceProfileResult = @"Container=Container
Virtual1=V1Instance1[V1Field='Value 1']
Child=V1I1Child1
Child=V1I1Child2
Virtual1=V1Instance2[V1Field='Value 2']
Child=V1I2Child1
Child=V1I2Child2
Virtual2=V2Instance1[V2Field='Value 1']
Child=V2I1Child1
Child=V2I1Child2
Virtual2=V2Instance2[V2Field='Value 2']
Child=V2I2Child1
Child=V2I2Child2
";
        protected const string VirtualDefinitionFile = "TestData\\VirtualDefinition.dcb";
        protected const string VirtualDataFile = "TestData\\VirtualData.dcb";
        protected const string VirtualParentDefinitionFile = "TestData\\VirtualParentDefinition.dcb";
        protected const string VirtualParentDataFile = "TestData\\VirtualParentData.dcb";
        protected const string VirtualParentDefinition = @"Definition=Definition
Class=Class
Field={Name,Title,Inheritance}
SubClass={SubClass,Property}
Class=SubClass
Field={Name,Reference,Relationship}
Class=Property
Field={Name,Title,DataType,Default,LookupType,LookupDependence,LookupTable}
.
Class=Parent[]
SubClass=Container[Reference='TestData/VirtualDefinition']
Property=Name[,DataType=String]
";
        protected const string VirtualParentData = @"Definition=VirtualParentDefinition
Class=Parent
Field=Name
SubClass=Container[Reference='TestData/VirtualDefinition']
.
Parent=Parent
Container[Reference='TestData\VirtualData']
";

        protected static GenDataBase LoadVirtualParentData()
        {
            SetUpParametersFile(VirtualDefinitionFile, VirtualDefinition);
            SetUpParametersFile(VirtualDataFile, VirtualDefinitionData);
            SetUpParametersFile(VirtualParentDefinitionFile, VirtualParentDefinition);
            SetUpParametersFile(VirtualParentDataFile, VirtualParentData);
            var data = GenDataBase.DataLoader.LoadData(VirtualParentDataFile);
            return data;
        }

        protected static void SetUpParametersFile(string fileName, string text)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            File.WriteAllText(fileName, text);
        }

        protected static void SaveVirtualAndParentData()
        {
            if (!Directory.Exists("TestData")) Directory.CreateDirectory("TestData");

            var df = SetUpVirtualDefinition();
            if (File.Exists(VirtualDefinitionFile)) File.Delete(VirtualDefinitionFile);
            GenParameters.SaveToFile(df.GenDataBase, VirtualDefinitionFile);
            var d = PopulateInheritanceData(VirtualDataFile);
            if (File.Exists(VirtualDataFile)) File.Delete(VirtualDataFile);
            GenParameters.SaveToFile(d, VirtualDataFile);

            var pdf = SetUpParentOfVirtualDefinition();
            if (File.Exists(VirtualParentDefinitionFile)) File.Delete(VirtualParentDefinitionFile);
            GenParameters.SaveToFile(pdf.GenDataBase, VirtualParentDefinitionFile);
            var pd = SetUpParentOfVirtualData();
            if (File.Exists(VirtualParentDataFile)) File.Delete(VirtualParentDataFile);
            GenParameters.SaveToFile(pd, VirtualParentDataFile);
        }

        protected static GenDataBase PopulateInheritanceData(string dataName)
        {
            var f = SetUpVirtualDefinition().GenDataBase.AsDef();
            var d = new GenDataBase(f) {DataName = Path.GetFileNameWithoutExtension(dataName)};
            var container = CreateGenObject(d.Root, "Container", "Container");
            var virtual1 = CreateGenObject(container, "Virtual1", "V1Instance1");
            Assert.AreEqual(3, virtual1.ClassId);
            var @abstract = new GenAttributes(f, 3) {GenObject = virtual1};
            @abstract.SetString("V1Field", "Value 1");
            @abstract.SaveFields();
            CreateGenObject(virtual1, "Child", "V1I1Child1");
            CreateGenObject(virtual1, "Child", "V1I1Child2");
            var virtual2 = CreateGenObject(container, "Virtual2", "V2Instance1");
            Assert.AreEqual(4, virtual2.ClassId);
            @abstract.GenObject = virtual2;
            //@abstract.SetString("Name", "V2Instance1");
            @abstract.SetString("V2Field", "Value 1");
            @abstract.SaveFields();
            CreateGenObject(virtual2, "Child", "V2I1Child1");
            CreateGenObject(virtual2, "Child", "V2I1Child2");
            virtual1 = CreateGenObject(container, "Virtual1", "V1Instance2");
            Assert.AreEqual(3, virtual1.ClassId);
            @abstract.GenObject = virtual1;
            //@abstract.SetString("Name", "V1Instance2");
            @abstract.SetString("V1Field", "Value 2");
            @abstract.SaveFields();
            CreateGenObject(virtual1, "Child", "V1I2Child1");
            CreateGenObject(virtual1, "Child", "V1I2Child2");
            virtual2 = CreateGenObject(container, "Virtual2", "V2Instance2");
            Assert.AreEqual(4, virtual2.ClassId);
            @abstract.GenObject = virtual2;
            //@abstract.SetString("Name", "V2Instance2");
            @abstract.SetString("V2Field", "Value 2");
            @abstract.SaveFields();
            CreateGenObject(virtual2, "Child", "V2I2Child1");
            CreateGenObject(virtual2, "Child", "V2I2Child2");
            return d;
        }

        protected static Definition SetUpVirtualDefinition()
        {
            var df = new Definition {GenDataBase = {DataName = "VirtualDefinition"}};
            var c = df.AddClass("Container");
            c.AddProperty("Name", dataType: "Identifier");
            c.AddSubClass("Abstract");
            var a = df.AddClass("Abstract", "Abstract class", "Abstract");
            a.AddProperty("Name", dataType: "Identifier");
            a.AddSubClass("Virtual1").Relationship = "Extends";
            a.AddSubClass("Virtual2").Relationship = "Extends";
            a.AddSubClass("Child");
            var v1 = df.AddClass("Virtual1");
            v1.AddProperty("V1Field");
            var v2 = df.AddClass("Virtual2");
            v2.AddProperty("V2Field");
            var ch = df.AddClass("Child");
            ch.AddProperty("Name", dataType: "Identifier");
            return df;
        }

        private static Definition SetUpParentOfVirtualDefinition()
        {
            Assert.IsTrue(File.Exists(VirtualDefinitionFile));
            Assert.IsTrue(File.Exists(VirtualDataFile));
            var df = new Definition {GenDataBase = {DataName = "VirtualParentDefintion"}};
            var c = df.AddClass("Parent");
            c.AddProperty("Name");
            c.AddSubClass("Container", "TestData/VirtualDefinition");
            return df;
        }

        protected static GenDataBase SetUpParentOfVirtualData()
        {
            Assert.IsTrue(File.Exists(VirtualDefinitionFile));
            Assert.IsTrue(File.Exists(VirtualParentDefinitionFile));
            Assert.IsTrue(File.Exists(VirtualDataFile));
            var f = GenDataBase.DataLoader.LoadData(VirtualParentDefinitionFile).AsDef();
            var d = new GenDataBase(f) { DataName = "VirtualParentData" };
            var container = new GenAttributes(f, 1) {GenObject = CreateGenObject(d.Root, "Parent", "Parent")};
            container.SetString("Name", "Parent");
            container.SaveFields();
            GetFirstObject(d).SubClass[0].Reference = "TestData\\VirtualData";
            return d;
        }
    }
}