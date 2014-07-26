using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenPlaceholderFragmentParams: GenFragmentParams
    {
        public GenDataId Id { get; set; }

        public GenPlaceholderFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, GenDataId id) 
            : base(genDataDef, parentSegment, parentContainer)
        {
            Id = id;
        }
    }
}