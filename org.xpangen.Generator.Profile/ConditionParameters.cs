using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class ConditionParameters
    {
        public GenDataId Var1 { get; set; }
        public GenDataId Var2 { get; set; }
        public GenComparison GenComparison { get; set; }
        public bool UseLit { get; set; }
        public string Lit { get; set; }
    }
}
