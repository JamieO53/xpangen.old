// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;

namespace org.xpangen.Generator.Data
{
    public class GenDataDef
    {
        public GenDataDefClassList Classes { get; private set; }
        public int CurrentClassId { get; set; }
        public string Definition { get; set; }

        public GenDataDefReferenceCache Cache { get; private set; }

        public GenDataDef()
        {
            Classes = new GenDataDefClassList();
            AddClass("");
            CurrentClassId = -1;
            Cache = new GenDataDefReferenceCache(this);
        }

        public int AddClass(string parent, string name)
        {
            if (Classes.Contains(name))
                return Classes.IndexOf(name);
            AddSubClass(parent, name);
            return Classes.IndexOf(name);
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
            var propertyName = "";
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
                        id.PropertyId = Classes[id.ClassId].Properties.Add(propertyName);
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
            id.PropertyName = Classes[id.ClassId].Properties[id.PropertyId];
            return id;
        }

        public int IndexOfSubClass(int classId, int subClassId)
        {
            return Classes[classId].SubClasses.IndexOf(subClassId);
        }

        public string CreateProfile()
        {
            var def = new StringBuilder();
            def.Append("Definition=");
            def.AppendLine(Definition);
            var profile = new StringBuilder();

            ClassProfile(0, def, profile);
            return def + ".\r\n" + profile;
        }

        private void ClassProfile(int classId, StringBuilder def, StringBuilder profile)
        {
            if (classId != 0)
            {
                def.AppendLine("Class=" + Classes[classId].Name);
                profile.Append("`[" + Classes[classId].Name + ":" + Classes[classId].Name);

                if (Classes[classId].Properties.Count > 0)
                {
                    if (Classes[classId].Properties.Count == 1)
                        def.AppendLine("Field=" + Classes[classId].Properties[0]);
                    else
                    {
                        def.Append("Field={" + Classes[classId].Properties[0]);
                        for (var i = 1; i < Classes[classId].Properties.Count; i++)
                            def.Append("," + Classes[classId].Properties[i]);
                        def.AppendLine("}");
                    }

                    var j = 0;
                    if (String.Compare(Classes[classId].Properties[0], "Name", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        profile.Append("=`" + Classes[classId].Name + ".Name`");
                        j = 1;
                    }
                    if (Classes[classId].Properties.Count > j)
                    {
                        profile.Append("[");
                        var sep = "";
                        for (var i = j; i < Classes[classId].Properties.Count; i++)
                        {
                            profile.Append("`?" + Classes[classId].Name + "." + Classes[classId].Properties[i] + ":" +
                                           sep + Classes[classId].Properties[i] +
                                           "`?" + Classes[classId].Name + "." + Classes[classId].Properties[i] + "<>True:" +
                                           "=`@StringOrName:`{`" + Classes[classId].Name +
                                           '.' + Classes[classId].Properties[i] +
                                           "``]`]`]`]");
                            sep = ",";
                        }
                        profile.Append("]");
                    }
                    profile.AppendLine();
                }
                
                if (Classes[classId].SubClasses.Count == 1)
                {
                    def.Append("SubClass=" + Classes[classId].SubClasses[0].SubClass.Name);
                    if (!string.IsNullOrEmpty(Classes[classId].SubClasses[0].SubClass.Reference))
                        def.AppendLine("[Reference='" + Classes[classId].SubClasses[0].SubClass.Reference + "']");
                    else
                        def.AppendLine();
                }
                else if (Classes[classId].SubClasses.Count > 1)
                {
                    def.Append("SubClass={" + Classes[classId].SubClasses[0].SubClass.Name);
                    for (var i = 1; i < Classes[classId].SubClasses.Count; i++)
                        def.Append("," + Classes[classId].SubClasses[i].SubClass.Name);
                    def.AppendLine("}");
                }
            }

            for (var i = 0; i < Classes[classId].SubClasses.Count; i++)
                if (string.IsNullOrEmpty(Classes[classId].SubClasses[i].SubClass.Reference))
                    ClassProfile(Classes[classId].SubClasses[i].SubClass.ClassId, def, profile);
                else
                    profile.Append("`[" + Classes[classId].SubClasses[i].SubClass.Name + "@:`]");

            if (classId != 0)
                profile.Append("`]");
        }

        public static GenDataDef CreateMinimal()
        {
            var def = new GenDataDef();
            def.Definition = "Minimal";
            def.AddClass("", "Class");
            def.AddClass("Class", "SubClass");
            def.AddClass("Class", "Property");
            def.Classes[def.Classes.IndexOf("Class")].Properties.Add("Name");
            def.Classes[def.Classes.IndexOf("SubClass")].Properties.Add("Name");
            def.Classes[def.Classes.IndexOf("SubClass")].Properties.Add("Reference");
            def.Classes[def.Classes.IndexOf("Property")].Properties.Add("Name");
            return def;
        }

        public GenData AsGenData()
        {
            var f = CreateMinimal();
            var d = new GenData(f);
            var a = new GenAttributes(f);
            for (var i = 1; i < Classes.Count; i++)
            {
                if (!string.IsNullOrEmpty(Classes[i].ReferenceDefinition)) continue;
                a.GenObject = d.CreateObject("", "Class");
                var c = Classes[i];
                a.SetString("Name", c.Name);
                a.SaveFields();
                for (var j = 0; j < c.SubClasses.Count; j++)
                {
                    a.GenObject = d.CreateObject("Class", "SubClass");
                    var subClass = c.SubClasses[j].SubClass;
                    a.SetString("Name", subClass.Name);
                    a.SetString("Reference", c.SubClasses[j].Reference);
                    a.SaveFields();
                }
                for (var j = 0; j < c.Properties.Count; j++)
                {
                    a.GenObject = d.CreateObject("Class", "Property");
                    a.SetString("Name", c.Properties[j]);
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
                sc.Add(new GenDataDefSubClass { SubClass = Classes[j], Reference = ""});
            Classes[j].Parent = Classes[i];
        }


        public void AddSubClass(string className, string subClassName, string reference)
        {
            AddSubClass(className, subClassName);
            if (string.IsNullOrEmpty(reference)) return;
            
            var i = Classes.IndexOf(className);
            var j = Classes[i].SubClasses.IndexOf(subClassName);
            var sc = Classes[i].SubClasses[j];
            ParseReference(reference, sc);
            if (!Cache.Contains(sc.ReferenceDefinition))
                Cache[sc.ReferenceDefinition] = sc.ReferenceDefinition.Equals("minimal", StringComparison.InvariantCultureIgnoreCase)
                     ? CreateMinimal()
                     : GenData.DataLoader.LoadData(sc.ReferenceDefinition).AsDef();
            var rf = Cache[sc.ReferenceDefinition];
            for (var k = 1; k < rf.Classes.Count; k++)
            {
                var item = rf.Classes[k];
                if (!Classes.Contains(rf.Classes[k].Name))
                {
                    Classes.Add(new GenDataDefClass
                                    {
                                        Name = item.Name,
                                        Parent = item.Parent,
                                        ClassId = Classes.Count,
                                        IsReference = true,
                                        RefClassId = item.ClassId,
                                        RefDef = rf,
                                        Reference = sc.Reference,
                                        ReferenceDefinition = sc.ReferenceDefinition
                                    });
                }
                else
                {
                    var oldItem = Classes[Classes.IndexOf(item.Name)];
                    oldItem.IsReference = true;
                    oldItem.RefClassId = item.ClassId;
                    oldItem.RefDef = rf;
                    oldItem.Reference = sc.Reference;
                    oldItem.ReferenceDefinition = sc.ReferenceDefinition;
                }
            }
            for (var k = 1; k < rf.Classes.Count; k++)
            {
                var item = rf.Classes[k];
                if (item.SubClasses.Count == 0) continue;
                var refItem = Classes[Classes.IndexOf(item.Name)];
                for (var l = 0; l < item.SubClasses.Count; l++)
                {
                    var sub = item.SubClasses[l];
                    var classId = Classes.IndexOf(sub.SubClass.Name);
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
        }

        private static void ParseReference(string reference, GenDataDefSubClass sc)
        {
            sc.Reference = reference;
            var ra = reference.Split(':');
            sc.ReferenceDefinition = ra[0];
            //if (ra.GetUpperBound(0) > 0)
            //{
            //    var fa = ra[1].Split(';');
            //    for (var i = 0; i <= fa.GetLowerBound(0); i++)
            //    {
            //        var ca = fa[i].Split('=');
            //        var ta = ca[0].Split('.');
            //        var sa = ca[1].Split('.');
            //        var tid = GetId(ca[0]);
            //        var sid = GetId(ca[1]);
            //    }
            //}
        }

        public int AddClass(string className)
        {
            var i = Classes.IndexOf(className);
            if (i == -1)
            {
                Classes.Add(new GenDataDefClass{Name = className, ClassId = Classes.Count, RefClassId = Classes.Count });
                i = Classes.IndexOf(className);
            }

            return i;
        }

        public string GetIdentifier(GenDataId genDataId)
        {
            return genDataId.Identifier;
        }
    }
}
