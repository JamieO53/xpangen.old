using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenConditionParams : GenFragmentParams
    {
        public GenDataId Var1 { get; set; }
        public GenDataId Var2 { get; set; }
        public GenComparison GenComparison { get; set; }
        public bool UseLit { get; set; }
        public string Lit { get; set; }
        public GenConditionParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, ConditionParameters conditionParameters)
            : base(genDataDef, parentSegment, parentContainer)
        {
            Var1 = conditionParameters.Var1;
            Var2 = conditionParameters.Var2;
            GenComparison = conditionParameters.GenComparison;
            UseLit = conditionParameters.UseLit;
            Lit = conditionParameters.Lit;
        }
    }
}