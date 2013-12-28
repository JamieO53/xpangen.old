// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Text;

namespace org.xpangen.Generator.Data
{
    public class GenDataDef
    {
        public NameList Classes { get; private set; }
        public List<NameList> Properties { get; private set; }
        public List<IndexList> SubClasses { get; private set; }
        public IndexList Parents { get; private set; }
        public int CurrentClassId { get; set; }

        public GenDataDef()
        {
            Classes = new NameList();
            Properties = new List<NameList>();
            SubClasses = new List<IndexList>();
            Parents = new IndexList();
            AddClass("", "");
        }

        public int AddClass(string parent, string name)
        {
            if (Classes.Contains(name))
                return Classes.IndexOf(name);
            var classId = Classes.Count;
            var parentClassId = Classes.IndexOf(parent);
            Parents.Add(parentClassId);
            Classes.Add(name);
            Properties.Add(new NameList());
            SubClasses.Add(new IndexList());
            if (parentClassId != -1)
                SubClasses[parentClassId].Add(classId);
            return classId;
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
                id.PropertyId = Properties[id.ClassId].IndexOf(propertyName);
            }
            else
            {
                className = sa[0];
                id.ClassId = Classes.IndexOf(className);
                propertyName = sa[1];
                if (id.ClassId != -1)
                    id.PropertyId = Properties[id.ClassId].IndexOf(propertyName);
                else
                {
                    if (String.Compare(propertyName, "First", StringComparison.OrdinalIgnoreCase) == 0)
                        id.PropertyId = Properties[id.ClassId].Add("First");
                }
            }
            if (id.ClassId == -1 && id.PropertyId == -1)
                throw new Exception("<<<<Unknown Class: " + name + ">>>>");
            if (id.PropertyId == -1 && className != "")
            {
                if (!createIfMissing)
                    throw new Exception("<<<<Unknown Class/Property: " + name + ">>>>");
                id.PropertyId = Properties[id.ClassId].Add(propertyName);
            }
            return id;
        }

        public int IndexOfSubClass(int classId, int subClassId)
        {
            return SubClasses[classId].IndexOf(subClassId);
        }

        public string CreateProfile()
        {
            var def = new StringBuilder();
            var profile = new StringBuilder();

            ClassProfile(0, def, profile);
            return def + ".\r\n" + profile;
        }

        private void ClassProfile(int classId, StringBuilder def, StringBuilder profile)
        {
            if (classId != 0)
            {
                def.AppendLine("Class=" + Classes[classId]);
                profile.Append("`[" + Classes[classId] + ":" + Classes[classId]);

                if (Properties[classId].Count > 0)
                {
                    if (Properties[classId].Count == 1)
                        def.AppendLine("Field=" + Properties[classId][0]);
                    else
                    {
                        def.Append("Field={" + Properties[classId][0]);
                        for (var i = 1; i < Properties[classId].Count; i++)
                            def.Append("," + Properties[classId][i]);
                        def.AppendLine("}");
                    }

                    var j = 0;
                    if (String.Compare(Properties[classId][0], "Name", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        profile.Append("=`" + Classes[classId] + "Name`");
                        j = 1;
                    }
                    if (Properties[classId].Count > j)
                    {
                        profile.Append("[");
                        var sep = "";
                        for (var i = j; i < Properties[classId].Count; i++)
                        {
                            profile.Append("`?" + Properties[classId][i] + ":" +
                                           sep + Properties[classId][i] +
                                           "`?" + Properties[classId][i] + "<>'True':" +
                                           "=`@StringOrName:`{`" + Classes[classId] +
                                           '.' + Properties[classId][i] +
                                           "``]`]`]`]");
                            sep = ",";
                        }
                        profile.Append("]");
                    }
                    profile.AppendLine();
                }
                
                if (SubClasses[classId].Count == 1)
                    def.AppendLine("SubClass=" + Classes[SubClasses[classId][0]]);
                else if (SubClasses[classId].Count > 1)
                {
                    def.Append("SubClass={" + Classes[SubClasses[classId][0]]);
                    for (var i = 1; i < SubClasses[classId].Count; i++)
                        def.Append("," + Classes[SubClasses[classId][i]]);
                    def.AppendLine("}");
                }
            }

            for (var i = 0; i < SubClasses[classId].Count; i++)
                ClassProfile(SubClasses[classId][i], def, profile);

            if (classId != 0)
                profile.Append("`]");
        }

        public static GenDataDef CreateMinimal()
        {
            var def = new GenDataDef();
            def.AddClass("", "Class");
            def.AddClass("Class", "SubClass");
            def.AddClass("Class", "Property");
            def.AddClass("SubClass", "FieldFilter");
            def.Properties[def.Classes.IndexOf("Class")].Add("Name");
            def.Properties[def.Classes.IndexOf("SubClass")].Add("Name");
            def.Properties[def.Classes.IndexOf("SubClass")].Add("Reference");
            def.Properties[def.Classes.IndexOf("Property")].Add("Name");
            def.Properties[def.Classes.IndexOf("FieldFilter")].Add("Name");
            def.Properties[def.Classes.IndexOf("FieldFilter")].Add("Operand");
            //def.AddSubClass("", "Class");
            //def.AddSubClass("Class", "SubClass");
            //def.AddSubClass("Class", "Property");
            //def.AddSubClass("SubClass", "FieldFilter");
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
                a.SetString("Name", Classes[i]);
                a.SaveFields();
                for (var j = 0; j < SubClasses[i].Count; j++)
                {
                    a.GenObject = d.CreateObject("Class", "SubClass");
                    a.SetString("Name", Classes[SubClasses[i][j]]);
                    a.SaveFields();
                }
                for (var j = 0; j < Properties[i].Count; j++)
                {
                    a.GenObject = d.CreateObject("Class", "Property");
                    a.SetString("Name", Properties[i][j]);
                    a.SaveFields();
                }
            }
            return d;
        }

        public void AddSubClass(string className, string subClassName)
        {
            var i = AddClass(className);
            var j = AddClass(subClassName);
            var sc = SubClasses[i];
            var k = sc.IndexOf(j);
            if (k == -1)
                sc.Add(j);
            for (var l = Parents.Count; l <= j; l++)
                Parents.Add(-1);
            Parents[j] = i;
        }

        public int AddClass(string className)
        {
            var i = Classes.IndexOf(className);
            if (i == -1)
            {
                i = Classes.Add(className);
                Properties.Add(new NameList());
                SubClasses.Add(new IndexList());
            }

            return i;
        }

        public string GetIdentifier(GenDataId genDataId)
        {
            if (genDataId.ClassId >= Classes.Count || genDataId.PropertyId >= Properties[genDataId.ClassId].Count)
                return "";
            return Classes[genDataId.ClassId] + "." + Properties[genDataId.ClassId][genDataId.PropertyId];
        }
    }
}
