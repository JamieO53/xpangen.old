using System;
using System.Text;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Test
{
    public static class GenDataDefProfile
    {
        private static void ClassProfile(GenDataDef genDataDef, int classId, StringBuilder def, StringBuilder profile)
        {
            if (classId != 0)
            {
                def.Append("Class=" + genDataDef.Classes[classId].Name);
                if (genDataDef.Classes[classId].Inheritors.Count > 0)
                {
                    var sep = '[';
                    foreach (var inheritor in genDataDef.Classes[classId].Inheritors)
                    {
                        def.Append(sep);
                        def.Append(inheritor.Name);
                        sep = ',';
                    }
                    def.AppendLine("]");
                    if (genDataDef.Classes[classId].Properties.Count > 0)
                    {
                        if (genDataDef.Classes[classId].Properties.Count == 1)
                            def.AppendLine("Field=" + genDataDef.Classes[classId].Properties[0]);
                        else
                        {
                            def.Append("Field={" + genDataDef.Classes[classId].Properties[0]);
                            for (var i = 1; i < genDataDef.Classes[classId].Properties.Count; i++)
                                def.Append("," + genDataDef.Classes[classId].Properties[i]);
                            def.AppendLine("}");
                        }
                    }
                }
                else
                {
                    def.AppendLine();
                    profile.Append("`[" + genDataDef.Classes[classId].Name + ":" + genDataDef.Classes[classId].Name);

                    if (genDataDef.Classes[classId].Properties.Count > 0)
                    {
                        var f = new StringBuilder();
                        var sep = "";
                        var defClass = genDataDef.Classes[classId];
                        for (var i = 0; i < defClass.Properties.Count; i++)
                        {
                            if (!defClass.IsInherited ||
                                defClass.Parent.Properties.IndexOf(defClass.Properties[i]) == -1)
                            {
                                f.Append(sep);
                                f.Append(defClass.Properties[i]);
                                sep = ",";
                            }
                        }

                        var fields = f.ToString();
                        if (fields != string.Empty)
                            if (!fields.Contains(","))
                                def.AppendLine("Field=" + fields);
                            else
                                def.AppendLine("Field={" + fields + "}");

                        var j = 0;
                        if (String.Compare(genDataDef.Classes[classId].Properties[0], "Name", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            profile.Append("=`" + genDataDef.Classes[classId].Name + ".Name`");
                            j = 1;
                        }
                        if (genDataDef.Classes[classId].Properties.Count > j)
                        {
                            profile.Append("[");
                            sep = "";
                            for (var i = j; i < genDataDef.Classes[classId].Properties.Count; i++)
                            {
                                profile.Append("`?" + genDataDef.Classes[classId].Name + "." + genDataDef.Classes[classId].Properties[i] + ":" +
                                               sep + genDataDef.Classes[classId].Properties[i] +
                                               "`?" + genDataDef.Classes[classId].Name + "." + genDataDef.Classes[classId].Properties[i] + "<>True:" +
                                               "=`@StringOrName:`{`" + genDataDef.Classes[classId].Name +
                                               '.' + genDataDef.Classes[classId].Properties[i] +
                                               "``]`]`]`]");
                                sep = ",";
                            }
                            profile.Append("]");
                        }
                        profile.AppendLine();
                    }
                }

                if (genDataDef.Classes[classId].SubClasses.Count == 1)
                {
                    def.Append("SubClass=" + genDataDef.Classes[classId].SubClasses[0].SubClass.Name);
                    if (!string.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[0].SubClass.Reference))
                        def.AppendLine("[Reference='" + genDataDef.Classes[classId].SubClasses[0].SubClass.Reference + "']");
                    else
                        def.AppendLine();
                }
                else if (genDataDef.Classes[classId].SubClasses.Count > 1)
                {
                    def.Append("SubClass={" + genDataDef.Classes[classId].SubClasses[0].SubClass.Name);
                    for (var i = 1; i < genDataDef.Classes[classId].SubClasses.Count; i++)
                        def.Append("," + genDataDef.Classes[classId].SubClasses[i].SubClass.Name);
                    def.AppendLine("}");
                }

                if (genDataDef.Classes[classId].Inheritors.Count > 0)
                {
                    profile.Append("`[" + genDataDef.Classes[classId].Name + ":");
                    for (var i = 0; i < genDataDef.Classes[classId].Inheritors.Count; i++)
                        ClassProfile(genDataDef, genDataDef.Classes[classId].Inheritors[i].ClassId, def, profile);
                    profile.Append("`]");
                }
            }

            for (var i = 0; i < genDataDef.Classes[classId].SubClasses.Count; i++)
                if (string.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[i].SubClass.Reference))
                    ClassProfile(genDataDef, genDataDef.Classes[classId].SubClasses[i].SubClass.ClassId, def, profile);
                else
                    profile.Append("`[" + genDataDef.Classes[classId].SubClasses[i].SubClass.Name + "@:`]");

            if (classId != 0)
                profile.Append("`]");
        }

        public static string CreateProfile(GenDataDef genDataDef)
        {
            var def = new StringBuilder();
            def.Append("Definition=");
            def.AppendLine(genDataDef.Definition);
            var profile = new StringBuilder();

            ClassProfile(genDataDef, 0, def, profile);
            return def + ".\r\n" + profile;
        }
    }
}
