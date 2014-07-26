using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenLookupParams : GenFragmentParams
    {
        private string _condition;

        public GenLookupParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, string condition)
            : base(genDataDef, parentSegment, parentContainer)
        {
            _condition = condition;
        }

        public string Condition
        {
            get { return _condition; }
        }
    }
}