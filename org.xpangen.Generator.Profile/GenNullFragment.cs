using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenNullFragment : GenFragment
    {
        public GenNullFragment(GenDataDef genDataDef, GenContainerFragmentBase parentSegment) : base(genDataDef, parentSegment)
        {
            FragmentType = FragmentType.Null;
        }

        public override string ProfileLabel()
        {
            return "";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            return "";
        }

        public override string Expand(GenData genData)
        {
            return "";
        }
    }
}
