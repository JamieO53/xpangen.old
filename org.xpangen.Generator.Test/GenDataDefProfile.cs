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
                if (genDataDef.GetClassInheritors(classId).Count > 0)
                    profile.Append("`[" + genDataDef.GetClassName(classId) +
                                   (genDataDef.GetClassIsInherited(classId) ? "^:" : ":"));
                else
                {
                    profile.Append("`[" + genDataDef.GetClassName(classId) +
                                   (genDataDef.GetClassIsInherited(classId) ? "^:" : ":") +
                                   genDataDef.GetClassName(classId));

                    if (genDataDef.GetClassProperties(classId).Count > 0)
                    {
                        var f = new StringBuilder();
                        var sep = "";
                        var defClass = genDataDef.GetClassDef(classId);
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
                            String.Compare(genDataDef.GetClassProperties(classId)[0], "Name",
                                           StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            profile.Append("=`" + genDataDef.GetClassName(classId) + ".Name`");
                            j = 1;
                        }
                        if (genDataDef.GetClassProperties(classId).Count > j)
                        {
                            profile.Append("[");
                            sep = "";
                            for (var i = j; i < genDataDef.GetClassProperties(classId).Count; i++)
                            {
                                var property = genDataDef.GetClassProperties(classId)[i];
                                profile.Append("`?" + genDataDef.GetClassName(classId) + "." +
                                               property + ":" + sep + property +
                                               "`?" + genDataDef.GetClassName(classId) + "." +
                                               property + "<>True:" +
                                               "=`@StringOrName:`{`" + genDataDef.GetClassName(classId) +
                                               '.' + property + "``]`]`]`]");
                                sep = ",";
                            }
                            profile.Append("]");
                        }
                        profile.AppendLine();
                    }
                }

                if (genDataDef.GetClassInheritors(classId).Count > 0)
                    foreach (var inheritor in genDataDef.GetClassInheritors(classId))
                        ClassProfile(genDataDef, inheritor.ClassId, profile);
            }


            if (!genDataDef.GetClassIsAbstract(classId))
                SubClassProfiles(genDataDef, classId, profile);

            if (genDataDef.GetClassIsInherited(classId))
                SubClassProfiles(genDataDef, genDataDef.GetClassParent(classId).ClassId, profile);

            if (classId != 0)
                profile.Append("`]");
        }

        private static void SubClassProfiles(GenDataDef genDataDef, int classId, StringBuilder profile)
        {
            var subClasses = genDataDef.GetClassSubClasses(classId);
            foreach (var subClass in subClasses)
            {
                if (string.IsNullOrEmpty(subClass.SubClass.Reference))
                    ClassProfile(genDataDef, subClass.SubClass.ClassId, profile);
                else
                    profile.Append("`[" + subClass.SubClass.Name + "@:`]");
            }
        }

        public static string CreateProfile(GenDataDef genDataDef)
        {
            var def = new StringBuilder();
            def.Append("Definition=");
            def.AppendLine(genDataDef.DefinitionName);
            GenParameters.ClassDefinition(genDataDef, 0, def);
            var profile = new StringBuilder();

            ClassProfile(genDataDef, 0, profile);
            return def + ".\r\n" + profile;
        }
    }
}
