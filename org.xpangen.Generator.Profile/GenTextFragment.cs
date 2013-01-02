using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenTextFragment : GenFragment
    {
        public GenTextFragment(GenDataDef genDataDef, GenContainerFragmentBase parentSegment) : base(genDataDef, parentSegment)
        {
            FragmentType = FragmentType.Text;
        }

        public string Text { get; set; }

        public override string ProfileLabel()
        {
            return "Text";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            return Text;
        }

        public override string Expand(GenData genData)
        {
            return Text;
        }
    }
}
