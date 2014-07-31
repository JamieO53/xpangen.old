using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenSegmentParams : GenFragmentParams
    {
        private string _className;
        private GenCardinality _cardinality;

        public GenSegmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, string className, GenCardinality cardinality)
            : base(genDataDef, parentSegment, parentContainer)
        {
            _className = className;
            _cardinality = cardinality;
        }

        public string ClassName
        {
            get { return _className; }
        }

        public GenCardinality Cardinality
        {
            get { return _cardinality; }
        }
    }
}