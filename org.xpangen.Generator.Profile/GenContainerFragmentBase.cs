using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenContainerFragmentBase : GenFragment
    {
        protected GenContainerFragmentBase(GenDataDef genDataDef, GenContainerFragmentBase parentSegment)
            : base(genDataDef, parentSegment)
        {
            GenDataDef = genDataDef;
            ParentSegement = parentSegment;
            Body = new GenSegBody(genDataDef, parentSegment) {ParentSegement = parentSegment};
        }

        public GenSegBody Body { get; private set; }

        public int ClassId
        {
            get
            {
                return ParentSegement is GenSegment
                           ? ((GenSegment) ParentSegement).ClassId
                           : (ParentSegement is GenLookup ? ((GenLookup) ParentSegement).ClassId : -1);
            }
        }
    }
}