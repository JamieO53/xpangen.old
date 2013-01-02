using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenLookup : GenContainerFragmentBase
    {
        public string Condition { get; set; }
        public bool NoMatch { get; set; }

        public new int ClassId { get; set; }
        protected GenDataId Var2 { get; set; }
        protected GenDataId Var1 { get; set; }

        public GenLookup(GenDataDef genDataDef, string condition, GenContainerFragmentBase parentSegment)
            : base(genDataDef, parentSegment)
        {
            Body.ParentSegement = this;
            FragmentType = FragmentType.Lookup;
            Condition = condition;
            var sa = condition.Split('=');
            Var1 = genDataDef.GetId(sa[0]);
            Var2 = genDataDef.GetId(sa[1]);
            ClassId = Var1.ClassId;
        }

        public override string ProfileLabel()
        {
            return (NoMatch ? "~" : "") + GenDataDef.GetIdentifier(Var1) + "=" + GenDataDef.GetIdentifier(Var2);
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString() + (NoMatch ? "2" : "1")].Format;
            return string.Format(format, new object[]
                                             {
                                                 GenDataDef.GetIdentifier(Var1),
                                                 GenDataDef.GetIdentifier(Var2),
                                                 Body.ProfileText(syntaxDictionary)
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            var result = "";
            if (NoMatch)
            {
                var context = new GenContext(genData);

                if (genData.Eol(Var2.ClassId))
                {
                    context.SaveContext();
                    genData.Reset(Var1.ClassId);
                    result = Body.Expand(genData);
                    context.RestoreContext();
                }
                else
                {
                    context.SaveContext();
                    var v = genData.GetValue(Var2);
                    SearchFor(genData, ClassId, Var1, v);
                    if (genData.Eol(ClassId))
                    {
                        context.RestoreContext();
                        result = Body.Expand(genData);
                    }
                    else
                        context.RestoreContext();
                }
            }
            else
            {
                if (!genData.Eol(Var2.ClassId))
                {
                    var context = new GenContext(genData);
                    context.SaveContext();

                    var v = genData.GetValue(Var2);
                    SearchFor(genData, ClassId, Var1, v);

                    if (!genData.Eol(ClassId))
                        result = Body.Expand(genData);
                    context.RestoreContext();
                }
            }
            return result;
        }

        private static void SearchFor(GenData genData, int classId, GenDataId id, string value)
        {
            genData.First(classId);
            while (!genData.Eol(classId) && genData.GetValue(id) != value)
            {
                genData.Next(classId);
            }
        }
    }
}
