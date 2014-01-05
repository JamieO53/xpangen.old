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

        public GenDataDef()
        {
            Classes = new GenDataDefClassList();
            AddClass("");
            CurrentClassId = -1;
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
            var id = new GenDataId {ClassId = -1, PropertyId = -1};
            var sa = name.Split(new[] {'.'}, 2);
            string className = "";
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
                        id.PropertyId = Classes[id.ClassId].Properties.Add(propertyName);
                    else if (propertyName.Equals("First", StringComparison.InvariantCultureIgnoreCase))
                        id.PropertyId = c.Properties.Add("First");
            }

            if (id.ClassId == -1 && id.PropertyId == -1)
                throw new Exception("<<<<Unknown Class: " + name + ">>>>");
            if (id.PropertyId == -1 && className != "")
                throw new Exception("<<<<Unknown Class/Property: " + name + ">>>>");
            
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
                    def.AppendLine("SubClass=" + Classes[classId].SubClasses[0].SubClass.Name);
                else if (Classes[classId].SubClasses.Count > 1)
                {
                    def.Append("SubClass={" + Classes[classId].SubClasses[0].SubClass.Name);
                    for (var i = 1; i < Classes[classId].SubClasses.Count; i++)
                        def.Append("," + Classes[classId].SubClasses[i].SubClass.Name);
                    def.AppendLine("}");
                }
            }

            for (var i = 0; i < Classes[classId].SubClasses.Count; i++)
                ClassProfile(Classes[classId].SubClasses[i].SubClass.ClassId, def, profile);

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
            def.AddClass("SubClass", "FieldFilter");
            def.Classes[def.Classes.IndexOf("Class")].Properties.Add("Name");
            def.Classes[def.Classes.IndexOf("SubClass")].Properties.Add("Name");
            def.Classes[def.Classes.IndexOf("SubClass")].Properties.Add("Reference");
            def.Classes[def.Classes.IndexOf("Property")].Properties.Add("Name");
            def.Classes[def.Classes.IndexOf("FieldFilter")].Properties.Add("Name");
            def.Classes[def.Classes.IndexOf("FieldFilter")].Properties.Add("Operand");
            return def;
        }

        public GenData AsGenData()
        {
            var f = CreateMinimal();
            var d = new GenData(f);
            var a = new GenAttributes(f);
            for (var i = 1; i < Classes.Count; i++)
            {
                a.GenObject = d.CreateObject("", "Class");
                var c = Classes[i];
                a.SetString("Name", c.Name);
                a.SaveFields();
                for (var j = 0; j < c.SubClasses.Count; j++)
                {
                    a.GenObject = d.CreateObject("Class", "SubClass");
                    a.SetString("Name", c.SubClasses[j].SubClass.Name);
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
            var i = Classes.IndexOf(className);
            var j = Classes[i].SubClasses.IndexOf(subClassName);
            var sc = Classes[i].SubClasses[j];
            sc.FieldFilters = new GenDataDefFieldFilterList();
            ParseReference(reference, sc);
        }

        private void ParseReference(string reference, GenDataDefSubClass sc)
        {
            sc.Reference = reference;
            var ra = reference.Split(':');
            sc.ReferenceDefinition = ra[0];
            if (ra.GetUpperBound(0) > 0)
            {
                var fa = ra[1].Split(';');
                for (var i = 0; i <= fa.GetLowerBound(0); i++)
                {
                    var ca = fa[i].Split('=');
                    var ta = ca[0].Split('.');
                    var sa = ca[1].Split('.');
                    var tid = GetId(ca[0]);
                    var sid = GetId(ca[1]);
                    var ff = new GenDataDefFieldFilter
                                 {
                                     Target =
                                         new GenDataDefId
                                             {
                                                 ClassId = tid.ClassId,
                                                 PropertyId = tid.PropertyId,
                                                 ClassName = ta[0],
                                                 PropertyName = ta[1]
                                             },
                                     Source =
                                         new GenDataDefId
                                             {
                                                 ClassId = sid.ClassId,
                                                 PropertyId = sid.PropertyId,
                                                 ClassName = sa[0],
                                                 PropertyName = sa[1]
                                             }
                                 };
                    sc.FieldFilters.Add(ff);
                }
            }
        }

        public int AddClass(string className)
        {
            var i = Classes.IndexOf(className);
            if (i == -1)
            {
                Classes.Add(new GenDataDefClass{Name = className, ClassId = Classes.Count});
                i = Classes.IndexOf(className);
            }

            return i;
        }

        public string GetIdentifier(GenDataId genDataId)
        {
            if (genDataId.ClassId >= Classes.Count || genDataId.PropertyId >= Classes[genDataId.ClassId].Properties.Count)
                return "";
            return Classes[genDataId.ClassId].Name + "." + Classes[genDataId.ClassId].Properties[genDataId.PropertyId];
        }
    }
}
