// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data.Definition;

namespace org.xpangen.Generator.Data
{
    public class GenDataDef
    {
        public GenDataDefClassList Classes { get; private set; }
        private Definition.Definition Definition { get; set; }
        public int CurrentClassId { get; set; }
        public string DefinitionName { get; set; }

        public GenDataDefReferenceCache Cache { get; private set; }

        public GenDataDef()
        {
            Classes = new GenDataDefClassList();
            AddClass("");
            CurrentClassId = -1;
            Cache = new GenDataDefReferenceCache(this);
            Definition = new Definition.Definition(DefinitionCreator.CreateEmpty());
        }

        public int AddClass(string parent, string name)
        {
            if (Classes.Contains(name))
                return GetClassId(name);
            var c = CreateDefinitionClass(name);
            CreateDefinitionSubClass(name, c);
            AddSubClass(parent, name);
            return GetClassId(name);
        }

        private static GenObject CreateDefinitionSubClass(string name, GenObject c)
        {
            return DefinitionCreator.CreateDefinitionSubClass(c, name, "", "");
        }

        private GenObject CreateDefinitionClass(string name)
        {
            return DefinitionCreator.CreateDefinitionClass(Definition.GenDataBase.Root, name, "", "",
                                                           Definition.GenDataBase);
        }

        private Class AddDefinitionClass(string className)
        {
            var c = new Class(Definition.GenDataBase) { GenObject = CreateDefinitionClass(className) };
            Definition.ClassList.Add(c);
            return c;
        }

        private void AddDefinitionSubClass(string subClassName, Class c)
        {
            var s = new SubClass(Definition.GenDataBase)
            {
                GenObject = CreateDefinitionSubClass(subClassName, (GenObject)c.GenObject)
            };
            c.SubClassList.Add(s);
        }

        public GenDataId GetId(string className, string propertyName)
        {
            return GetId(className + "." + propertyName);
        }

        public GenDataId GetId(string name, bool createIfMissing = false)
        {
            var id = new GenDataId {ClassId = -1, PropertyId = -1, ClassName = "", PropertyName = ""};
            var sa = name.Split(new[] {'.'}, 2);
            var className = "";
            string propertyName;
            if (sa.GetUpperBound(0) == 0)
            {
                id.ClassId = CurrentClassId;
                propertyName = sa[0];
            }
            else
            {
                className = sa[0];
                id.ClassId = GetClassId(className);
                propertyName = sa[1];
            }
            if (id.ClassId != -1)
            {
                var c = Classes[id.ClassId];
                id.PropertyId = c.Properties.IndexOf(propertyName);
                if (id.PropertyId == -1)
                    if (createIfMissing)
                    {
                        id.PropertyId = Classes[id.ClassId].Properties.Add(propertyName);
                    }
                    else if (propertyName.Equals("First", StringComparison.InvariantCultureIgnoreCase))
                    {
                        id.PropertyId = c.Properties.Add("First");
                        c.SetPseudo(id.PropertyId);
                    }
                    else if (propertyName.Equals("Reference", StringComparison.InvariantCultureIgnoreCase))
                    {
                        id.PropertyId = c.Properties.Add("Reference");
                        c.SetPseudo(id.PropertyId);
                    }
            }

            if (id.ClassId == -1 || id.PropertyId == -1)
            {
                id.ClassName = className;
                id.PropertyName = propertyName;
            }
            else
            {
                id.ClassName = Classes[id.ClassId].Name;
                id.PropertyName = Classes[id.ClassId].Properties[id.PropertyId];
            }
            return id;
        }

        /// <summary>
        /// Get the ClassId of the named class.
        /// </summary>
        /// <param name="name">The name of the sought class.</param>
        /// <returns>The classId if valid, otherwise -1.</returns>
        public int GetClassId(string name)
        {
            return Classes.IndexOf(name);
        }

        public GenDataDefClass GetClassDef(int classId)
        {
            if (classId == -1) return null;
            return Classes[classId];
        }

        public GenDataDefClass GetClassDef(string name)
        {
            var classId = GetClassId(name);
            if (classId == -1) return null;
            return Classes[classId];
        }

        public static GenDataDef CreateMinimal()
        {
            var def = new GenDataDef();
            PopulateMinimal(def);
            return def;
        }

        private static void PopulateMinimal(GenDataDef def)
        {
            def.DefinitionName = "Minimal";
            def.AddClass("", "Class");
            def.AddClass("Class", "SubClass");
            def.AddClass("Class", "Property");
            def.AddClassInstanceProperty(def.GetClassId("Class"), "Name");
            def.AddClassInstanceProperty(def.GetClassId("Class"), "Inheritance");
            def.AddClassInstanceProperty(def.GetClassId("SubClass"), "Name");
            def.AddClassInstanceProperty(def.GetClassId("SubClass"), "Reference");
            def.AddClassInstanceProperty(def.GetClassId("SubClass"), "Relationship");
            def.AddClassInstanceProperty(def.GetClassId("Property"), "Name");
        }

        public GenDataBase AsGenDataBase()
        {
            var f = CreateMinimal();
            var d = new GenDataBase(f) {DataName = DefinitionName};
            var a = new GenAttributes(f, 1);
            for (var i = 1; i < Classes.Count; i++)
            {
                if (!string.IsNullOrEmpty(Classes[i].ReferenceDefinition)) continue;
                var @class = d.Root.CreateGenObject("Class");
                a.GenObject = @class;
                var c = Classes[i];
                a.SetString("Name", c.Name);
                a.SaveFields();
                foreach (var inheritor in c.Inheritors)
                {
                    a.GenObject = @class.CreateGenObject("SubClass");
                    var inheritorId = inheritor.ClassId;
                    a.SetString("Name", Classes[inheritorId].Name);
                    a.SetString("Relationship", "Extends");
                    a.SaveFields();
                    a.GenObject = ((GenObject) a.GenObject).Parent;
                    a.SetString("Inheritance", "Abstract");
                    a.SaveFields();
                }
                foreach (var subClass in c.SubClasses)
                {
                    a.GenObject = @class.CreateGenObject("SubClass");
                    var sc = subClass.SubClass;
                    a.SetString("Name", sc.Name);
                    a.SetString("Reference", sc.Reference);
                    a.SaveFields();
                }
                foreach (var property in c.InstanceProperties)
                {
                    a.GenObject = @class.CreateGenObject("Property");
                    a.SetString("Name", property);
                    a.SaveFields();
                }
            }
            return d;
        }

        public void AddSubClass(string className, string subClassName)
        {
            var i = AddClass(className);
            var j = AddClass(subClassName);
            var sc = Classes[i].SubClasses;
            var k = sc.IndexOf(subClassName);
            if (k == -1)
                sc.Add(new GenDataDefSubClass {SubClass = Classes[j], Reference = ""});
            Classes[j].Parent = Classes[i];
            var c = Definition.ClassList.Find(className) ?? AddDefinitionClass(className);
            if (!c.SubClassList.Contains(subClassName))
                AddDefinitionSubClass(subClassName, c);
        }

        public void AddSubClass(string className, string subClassName, string reference)
        {
            AddSubClass(className, subClassName);
            if (string.IsNullOrEmpty(reference)) return;

            var i = GetClassId(className);
            var j = Classes[i].SubClasses.IndexOf(subClassName);
            var sc = Classes[i].SubClasses[j];
            sc.SubClass.CreateInstanceProperties();
            ParseReference(reference, sc);
            if (!Cache.Contains(sc.ReferenceDefinition))
                Cache[sc.ReferenceDefinition] = sc.ReferenceDefinition.Equals("minimal",
                                                                              StringComparison
                                                                                  .InvariantCultureIgnoreCase)
                                                    ? CreateMinimal()
                                                    : GenDataBase.DataLoader.LoadData(sc.ReferenceDefinition).AsDef();
            var rf = Cache[sc.ReferenceDefinition];
            for (var k = 1; k < rf.Classes.Count; k++)
            {
                var item = rf.Classes[k];
                if (!Classes.Contains(rf.Classes[k].Name))
                {
                    var newClass = new GenDataDefClass
                                       {
                                           Name = item.Name,
                                           Parent = item.Parent,
                                           ClassId = Classes.Count,
                                           Definition = item.Definition,
                                           IsReference = true,
                                           IsInherited = item.IsInherited,
                                           RefClassId = item.ClassId,
                                           RefDef = rf,
                                           Reference = sc.Reference,
                                           ReferenceDefinition = sc.ReferenceDefinition
                                       };
                    foreach (var instanceProperty in item.InstanceProperties)
                        newClass.AddInstanceProperty(instanceProperty);
                    Classes.Add(newClass);
                }
                else
                {
                    var oldItem = Classes[GetClassId(item.Name)];
                    oldItem.IsReference = true;
                    oldItem.RefClassId = item.ClassId;
                    oldItem.RefDef = rf;
                    oldItem.Reference = sc.Reference;
                    oldItem.ReferenceDefinition = sc.ReferenceDefinition;
                    oldItem.IsInherited = item.IsInherited;
                    foreach (var instanceProperty in item.InstanceProperties)
                        oldItem.AddInstanceProperty(instanceProperty);
                }
            }
            for (var k = 1; k < rf.Classes.Count; k++)
            {
                var item = rf.Classes[k];
                if (item.SubClasses.Count != 0)
                {
                    var refItem = Classes[GetClassId(item.Name)];
                    for (var l = 0; l < item.SubClasses.Count; l++)
                    {
                        var sub = item.SubClasses[l];
                        var classId = GetClassId(sub.SubClass.Name);

                        var found = false;
                        for (var m = 0; m < refItem.SubClasses.Count; m++)
                        {
                            if (refItem.SubClasses[m].SubClass.ClassId != classId) continue;
                            found = true;
                            break;
                        }

                        if (found) continue;

                        var newSub = new GenDataDefSubClass
                                         {
                                             Reference = sc.Reference,
                                             ReferenceDefinition = sc.ReferenceDefinition,
                                             SubClass = Classes[classId]
                                         };
                        refItem.SubClasses.Add(newSub);
                        newSub.SubClass.Parent = refItem;
                    }
                }
                if (item.Inheritors.Count != 0)
                {
                    var refItem = Classes[GetClassId(item.Name)];
                    for (var l = 0; l < item.Inheritors.Count; l++)
                    {
                        var inheritor = item.Inheritors[l];
                        var classId = GetClassId(inheritor.Name);

                        var found = false;
                        for (var m = 0; m < refItem.Inheritors.Count; m++)
                        {
                            if (refItem.Inheritors[m].ClassId != classId) continue;
                            found = true;
                            break;
                        }

                        if (found) continue;

                        refItem.Inheritors.Add(Classes[classId]);
                        Classes[classId].Parent = refItem;
                    }
                }
            }
        }

        public void AddInheritor(string className, string inheritorName)
        {
            var i = GetClassId(className);
            var j = GetClassId(inheritorName);
            GenDataDefClass inheritor;
            if (j == -1)
            {
                inheritor = new GenDataDefClass
                            {
                                Name = inheritorName,
                                ClassId = Classes.Count,
                                Definition = Definition.ClassList.Find(inheritorName)
                            };
                Classes.Add(inheritor);
            }
            else
                inheritor = Classes[j];
            var parent = Classes[i];
            if (!parent.Inheritors.Contains(inheritorName))
            {
                parent.Inheritors.Add(inheritor);
                inheritor.Parent = parent;
                inheritor.IsInherited = true;
                for (var k = 0; k < parent.InstanceProperties.Count; k++)
                {
                    if (inheritor.InstanceProperties.IndexOf(parent.Properties[k]) == -1)
                        inheritor.AddInstanceProperty(parent.Properties[k]);
                }
            }
        }

        private static void ParseReference(string reference, GenDataDefSubClass sc)
        {
            sc.Reference = reference;
            var ra = reference.Split(':');
            sc.ReferenceDefinition = ra[0];
        }

        public int AddClass(string className)
        {
            var i = GetClassId(className);
            if (i == -1)
            {
                Classes.Add(new GenDataDefClass
                            {
                                Name = className,
                                ClassId = Classes.Count,
                                RefClassId = Classes.Count,
                                Definition = Definition != null ? Definition.ClassList.Find(className) : null
                            });
                i = GetClassId(className);
            }
            if (i == 0) Classes[0].CreateInstanceProperties();
            return i;
        }

        public override string ToString()
        {
            return "GenDataDef:" + DefinitionName;
        }

        public string GetClassName(int classId)
        {
            return GetClassDef(classId).Name;
        }

        public GenDataDefClass GetClassParent(int classId)
        {
            return GetClassDef(classId).Parent;
        }

        public int AddClassInstanceProperty(int classId, string name)
        {
            return GetClassDef(classId).AddInstanceProperty(name);
        }

        public GenDataDefSubClassList GetClassSubClasses(int classId)
        {
            return GetClassDef(classId).SubClasses;
        }

        public NameList GetClassProperties(int classId)
        {
            return GetClassDef(classId).Properties;
        }

        public GenDataDefClassList GetClassInheritors(int classId)
        {
            return GetClassDef(classId).Inheritors;
        }

        public NameList GetClassInstanceProperties(int classId)
        {
            return GetClassDef(classId).InstanceProperties;
        }

        public bool GetClassIsInherited(int classId)
        {
            return GetClassDef(classId).IsInherited;
        }

        public bool GetClassIsAbstract(int classId)
        {
            return GetClassDef(classId).IsAbstract;
        }

        public int GetBaseClassId(string className)
        {
            var classId = GetClassId(className);
            if (classId == -1) throw new GeneratorException("Unknown record type: " + className, GenErrorType.Assertion);
            var baseClassId = classId;
            while (GetClassIsInherited(baseClassId))
                baseClassId = GetClassParent(baseClassId).ClassId;
            return baseClassId;
        }
    }
}