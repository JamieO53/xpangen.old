// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data.Definition;

namespace org.xpangen.Generator.Data
{
    public class GenDataDef : GenBase
    {
        public GenDataDefClassList Classes { get; private set; }
        public Definition.Definition Definition { get; private set; }
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
                return Classes.IndexOf(name);
            var c = CreateDefinitionClass(name);
            CreateDefinitionSubClass(name, c);
            AddSubClass(parent, name);
            return Classes.IndexOf(name);
        }

        private static GenObject CreateDefinitionSubClass(string name, GenObject c)
        {
            return DefinitionCreator.CreateDefinitionSubClass(c, name, "", "");
        }

        private GenObject CreateDefinitionClass(string name)
        {
            return DefinitionCreator.CreateDefinitionClass(Definition.GenData.Root, name, "", "",
                                                           Definition.GenData.GenDataBase);
        }

        private Class AddDefinitionClass(string className)
        {
            var c = new Class(Definition.GenData) { GenObject = CreateDefinitionClass(className) };
            Definition.ClassList.Add(c);
            return c;
        }

        private SubClass AddDefinitionSubClass(string subClassName, Class c)
        {
            var s = new SubClass(Definition.GenData)
            {
                GenObject = CreateDefinitionSubClass(subClassName, (GenObject)c.GenObject)
            };
            c.SubClassList.Add(s);
            return s;
        }

        public GenDataId GetId(string name)
        {
            var id = GetId(name, false);
            return id;
        }

        public GenDataId GetId(string name, bool createIfMissing)
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
                id.ClassId = Classes.IndexOf(className);
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

            if (id.ClassId == -1 && id.PropertyId == -1)
                throw new Exception("<<<<Unknown Class: " + name + ">>>>");
            if (id.PropertyId == -1 && className != "")
                throw new Exception("<<<<Unknown Class/Property: " + name + ">>>>");

            id.ClassName = Classes[id.ClassId].Name;
            //Assert(id.ClassId >= 0 && id.ClassId < Classes.Count, "Invalid ClassId in GetId: " + id);
            //Assert(id.PropertyId >= 0 && id.PropertyId < Classes[id.ClassId].Properties.Count, "Invalid PropertyId in GetId: " + id);
            id.PropertyName = Classes[id.ClassId].Properties[id.PropertyId];
            return id;
        }

        public int IndexOfSubClass(int classId, int subClassId)
        {
            var idx = Classes[classId].SubClasses.IndexOf(subClassId);
            if (idx == -1 && Classes[classId].IsInherited)
                idx = Classes[classId].SubClasses.Count + IndexOfSubClass(Classes[classId].Parent.ClassId, subClassId);
            return idx;
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
            def.Classes[def.Classes.IndexOf("Class")].AddInstanceProperty("Name");
            def.Classes[def.Classes.IndexOf("Class")].AddInstanceProperty("Inheritance");
            def.Classes[def.Classes.IndexOf("SubClass")].AddInstanceProperty("Name");
            def.Classes[def.Classes.IndexOf("SubClass")].AddInstanceProperty("Reference");
            def.Classes[def.Classes.IndexOf("SubClass")].AddInstanceProperty("Relationship");
            def.Classes[def.Classes.IndexOf("Property")].AddInstanceProperty("Name");
        }

        public static GenDataDef CreateDefinition()
        {
            var def = new GenDataDef();
            PopulateDefinition(def);
            return def;
        }

        private static void PopulateDefinition(GenDataDef def)
        {
            def.DefinitionName = "Definition";
            def.AddSubClass("", "Class");
            def.AddSubClass("Class", "SubClass");
            def.AddSubClass("Class", "Property");
            def.Classes[1].AddInstanceProperty("Name");
            def.Classes[1].AddInstanceProperty("Title");
            def.Classes[1].AddInstanceProperty("Inheritance");
            def.Classes[2].AddInstanceProperty("Name");
            def.Classes[2].AddInstanceProperty("Reference");
            def.Classes[2].AddInstanceProperty("Relationship");
            def.Classes[3].AddInstanceProperty("Name");
            def.Classes[3].AddInstanceProperty("Title");
            def.Classes[3].AddInstanceProperty("DataType");
            def.Classes[3].AddInstanceProperty("Default");
            def.Classes[3].AddInstanceProperty("LookupType");
            def.Classes[3].AddInstanceProperty("LookupDependence");
            def.Classes[3].AddInstanceProperty("LookupTable");
        }

        public GenData AsGenData()
        {
            var f = CreateMinimal();
            var d = new GenData(f) {DataName = DefinitionName};
            var a = new GenAttributes(f, 1);
            for (var i = 1; i < Classes.Count; i++)
            {
                if (!string.IsNullOrEmpty(Classes[i].ReferenceDefinition)) continue;
                a.GenObject = d.CreateObject("", "Class");
                var c = Classes[i];
                a.SetString("Name", c.Name);
                a.SaveFields();
                for (var j = 0; j < c.Inheritors.Count; j++)
                {
                    a.GenObject = d.CreateObject("Class", "SubClass");
                    var inheritorId = c.Inheritors[j].ClassId;
                    a.SetString("Name", Classes[inheritorId].Name);
                    a.SetString("Relationship", "Extends");
                    a.SaveFields();
                    a.GenObject = ((GenObject) a.GenObject).Parent;
                    a.SetString("Inheritance", "Abstract");
                    a.SaveFields();
                }
                for (var j = 0; j < c.SubClasses.Count; j++)
                {
                    a.GenObject = d.CreateObject("Class", "SubClass");
                    var subClass = c.SubClasses[j].SubClass;
                    a.SetString("Name", subClass.Name);
                    a.SetString("Reference", c.SubClasses[j].Reference);
                    a.SaveFields();
                }
                for (var j = 0; j < c.InstanceProperties.Count; j++)
                {
                    a.GenObject = d.CreateObject("Class", "Property");
                    a.SetString("Name", c.InstanceProperties[j]);
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
            if (c.SubClassList.Find(subClassName) == null)
                AddDefinitionSubClass(subClassName, c);
        }

        public void AddSubClass(string className, string subClassName, string reference)
        {
            AddSubClass(className, subClassName);
            if (string.IsNullOrEmpty(reference)) return;

            var i = Classes.IndexOf(className);
            var j = Classes[i].SubClasses.IndexOf(subClassName);
            var sc = Classes[i].SubClasses[j];
            sc.SubClass.CreateInstanceProperties();
            ParseReference(reference, sc);
            if (!Cache.Contains(sc.ReferenceDefinition))
                Cache[sc.ReferenceDefinition] = sc.ReferenceDefinition.Equals("minimal",
                                                                              StringComparison
                                                                                  .InvariantCultureIgnoreCase)
                                                    ? CreateMinimal()
                                                    : GenData.DataLoader.LoadData(sc.ReferenceDefinition).AsDef();
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
                                           IsReference = true,
                                           IsInherited = item.IsInherited,
                                           RefClassId = item.ClassId,
                                           RefDef = rf,
                                           Reference = sc.Reference,
                                           ReferenceDefinition = sc.ReferenceDefinition
                                       };
                    for (var l = 0; l < item.InstanceProperties.Count; l++)
                        newClass.AddInstanceProperty(item.InstanceProperties[l]);
                    Classes.Add(newClass);
                }
                else
                {
                    var oldItem = Classes[Classes.IndexOf(item.Name)];
                    oldItem.IsReference = true;
                    oldItem.RefClassId = item.ClassId;
                    oldItem.RefDef = rf;
                    oldItem.Reference = sc.Reference;
                    oldItem.ReferenceDefinition = sc.ReferenceDefinition;
                    oldItem.IsInherited = item.IsInherited;
                    for (var l = 0; l < item.InstanceProperties.Count; l++)
                        oldItem.AddInstanceProperty(item.InstanceProperties[l]);
                }
            }
            for (var k = 1; k < rf.Classes.Count; k++)
            {
                var item = rf.Classes[k];
                if (item.SubClasses.Count != 0)
                {
                    var refItem = Classes[Classes.IndexOf(item.Name)];
                    for (var l = 0; l < item.SubClasses.Count; l++)
                    {
                        var sub = item.SubClasses[l];
                        var classId = Classes.IndexOf(sub.SubClass.Name);

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
                    var refItem = Classes[Classes.IndexOf(item.Name)];
                    for (var l = 0; l < item.Inheritors.Count; l++)
                    {
                        var inheritor = item.Inheritors[l];
                        var classId = Classes.IndexOf(inheritor.Name);

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
            var i = Classes.IndexOf(className);
            var j = Classes.IndexOf(inheritorName);
            GenDataDefClass inheritor;
            if (j == -1)
            {
                inheritor = new GenDataDefClass {Name = inheritorName, ClassId = Classes.Count};
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
            var i = Classes.IndexOf(className);
            if (i == -1)
            {
                Classes.Add(new GenDataDefClass {Name = className, ClassId = Classes.Count, RefClassId = Classes.Count});
                i = Classes.IndexOf(className);
            }
            if (i == 0) Classes[0].CreateInstanceProperties();
            return i;
        }

        public string GetIdentifier(GenDataId genDataId)
        {
            return genDataId.Identifier;
        }

        public override string ToString()
        {
            return "GenDataDef:" + DefinitionName;
        }
    }
}