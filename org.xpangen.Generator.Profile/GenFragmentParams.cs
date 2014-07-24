using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentParams
    {
        private readonly GenDataDef _genDataDef;
        private readonly GenContainerFragmentBase _parentSegment;
        private readonly GenContainerFragmentBase _parentContainer;
        private FragmentType _fragmentType;

        /// <summary>
        /// Parameters for creating a GenFragment
        /// </summary>
        /// <param name="genDataDef">The definition of the data being generated.</param>
        /// <param name="parentSegment">The class segment this fragment belongs to.</param>
        /// <param name="parentContainer"></param>
        /// <param name="fragmentType">The type of fragment.</param>
        public GenFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, FragmentType fragmentType)
        {
            _genDataDef = genDataDef;
            _parentSegment = parentSegment;
            _parentContainer = parentContainer;
            _fragmentType = fragmentType;
        }

        public GenFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer)
        {
            _genDataDef = genDataDef;
            _parentSegment = parentSegment;
            _parentContainer = parentContainer;
        }

        public GenFragmentParams SetFragmentType(FragmentType fragmentType)
        {
            _fragmentType = fragmentType;
            return this;
        }
        
        public GenDataDef GenDataDef
        {
            get { return _genDataDef; }
        }

        public GenContainerFragmentBase ParentSegment
        {
            get { return _parentSegment; }
        }

        public GenContainerFragmentBase ParentContainer
        {
            get { return _parentContainer; }
        }

        public FragmentType FragmentType
        {
            get { return _fragmentType; }
        }
    }
}