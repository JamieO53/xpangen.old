using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenPlaceholderFragment : GenFragment
    {
        public GenPlaceholderFragment(GenDataDef genDataDef, GenContainerFragmentBase parentSegment) : base(genDataDef, parentSegment)
        {
            FragmentType = FragmentType.Placeholder;
        }

        public GenDataId Id { get; set; }

        public override string ProfileLabel()
        {
            return GenDataDef.GetIdentifier(Id);
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            return string.Format(format, new object[]
                                             {
                                                 GenDataDef.Classes[Id.ClassId],
                                                 GenDataDef.Properties[Id.ClassId][Id.PropertyId]
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            return genData.GetValue(Id);
        }
    }
}
