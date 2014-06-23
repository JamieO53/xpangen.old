using System;
using System.Text;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    public static class GenDataDefProfile
    {
        private static void ClassProfile(GenDataDef genDataDef, int classId, StringBuilder profile)
        {
            if (classId != 0)
            {
                if (genDataDef.Classes[classId].Inheritors.Count > 0)
                    profile.Append("`[" + genDataDef.Classes[classId].Name +
                                   (genDataDef.Classes[classId].IsInherited ? "^:" : ":"));
                else
                {
                    profile.Append("`[" + genDataDef.Classes[classId].Name +
                                   (genDataDef.Classes[classId].IsInherited ? "^:" : ":") +
                                   genDataDef.Classes[classId].Name);

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

                        var j = 0;
                        if (
                            String.Compare(genDataDef.Classes[classId].Properties[0], "Name",
                                           StringComparison.OrdinalIgnoreCase) == 0)
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
                                profile.Append("`?" + genDataDef.Classes[classId].Name + "." +
                                               genDataDef.Classes[classId].Properties[i] + ":" +
                                               sep + genDataDef.Classes[classId].Properties[i] +
                                               "`?" + genDataDef.Classes[classId].Name + "." +
                                               genDataDef.Classes[classId].Properties[i] + "<>True:" +
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

                if (genDataDef.Classes[classId].Inheritors.Count > 0)
                    for (var i = 0; i < genDataDef.Classes[classId].Inheritors.Count; i++)
                        ClassProfile(genDataDef, genDataDef.Classes[classId].Inheritors[i].ClassId, profile);
            }


            if (!genDataDef.Classes[classId].IsAbstract)
                SubClassProfiles(genDataDef, classId, profile);

            if (genDataDef.Classes[classId].IsInherited)
                SubClassProfiles(genDataDef, genDataDef.Classes[classId].Parent.ClassId, profile);

            if (classId != 0)
                profile.Append("`]");
        }

        private static void SubClassProfiles(GenDataDef genDataDef, int classId, StringBuilder profile)
        {
            for (var i = 0; i < genDataDef.Classes[classId].SubClasses.Count; i++)
            {
                if (string.IsNullOrEmpty(genDataDef.Classes[classId].SubClasses[i].SubClass.Reference))
                    ClassProfile(genDataDef, genDataDef.Classes[classId].SubClasses[i].SubClass.ClassId, profile);
                else
                    profile.Append("`[" + genDataDef.Classes[classId].SubClasses[i].SubClass.Name + "@:`]");
            }
        }

        public static string CreateProfile(GenDataDef genDataDef)
        {
            var def = new StringBuilder();
            def.Append("Definition=");
            def.AppendLine(genDataDef.Definition);
            GenParameters.ClassDefinition(genDataDef, 0, def);
            var profile = new StringBuilder();

            ClassProfile(genDataDef, 0, profile);
            return def + ".\r\n" + profile;
        }
    }
}
