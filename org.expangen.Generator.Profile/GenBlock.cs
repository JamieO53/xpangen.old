using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenBlock : GenFragment
    {
        public GenSegBody Body { get; private set; }

        public GenBlock(GenDataDef genDataDef, GenContainerFragmentBase parentSegment)
            : base(genDataDef, parentSegment)
        {
            FragmentType = FragmentType.Block;
            Body = new GenSegBody(genDataDef, parentSegment);
        }

        public override string ProfileLabel()
        {
            return "Block";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 Body.ProfileText(syntaxDictionary)
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            return Body.Expand(genData);
        }
    }
}
