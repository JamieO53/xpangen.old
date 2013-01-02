using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;

namespace org.xpangen.Generator.Test
{
    /// <summary>
    /// Additions to GenFragment class for testing
    /// </summary>
    internal class GenFragmentTest : GenFragment
    {
        public GenFragmentTest(GenDataDef genDataDef, GenContainerFragmentBase parentSegment)
            : base(genDataDef, parentSegment)
        {
            FragmentType = FragmentType.Null; // Default for testing
        }

        public override string ProfileLabel()
        {
            return "Label";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            return "Text";
        }

        public override string Expand(GenData genData)
        {
            return "Generate";
        }
    }
}