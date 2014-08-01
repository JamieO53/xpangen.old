using System;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentParams : GenBase
    {
        private Fragment _fragment;

        /// <summary>
        /// Parameters for creating a GenFragment
        /// </summary>
        /// <param name="genDataDef">The definition of the data being generated.</param>
        /// <param name="parentSegment">The class segment this fragment belongs to.</param>
        /// <param name="parentContainer">The container fragment conataining this fragment.</param>
        /// <param name="fragmentType">The type of fragment.</param>
        public GenFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, FragmentType fragmentType)
        {
            GenDataDef = genDataDef;
            ParentSegment = parentSegment;
            ParentContainer = parentContainer;
            SetFragmentType(fragmentType);
            Assert(Fragment != null, "Fragment expected");
        }

        public GenFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer)
        {
            GenDataDef = genDataDef;
            ParentSegment = parentSegment;
            ParentContainer = parentContainer;
            ClassID = ParentSegment == null ? 0 : ParentSegment.ClassId;
        }

        public GenFragmentParams SetFragmentType(FragmentType fragmentType)
        {
            FragmentType = fragmentType;
            CheckFragment(fragmentType);
            return this;
        }

        public Fragment Fragment
        {
            get
            {
                Assert(FragmentExists, "Fragment expected");
                return _fragment;
            }
            protected set { _fragment = value; }
        }

        private bool FragmentExists
        {
            get { return _fragment != null; }
        }

        public GenDataDef GenDataDef { get; private set; }

        public GenContainerFragmentBase ParentSegment { get; private set; }

        public GenContainerFragmentBase ParentContainer { get; private set; }

        public FragmentType FragmentType { get; protected set; }
        public int ClassID { get; private set; }

        private void CheckFragment(FragmentType fragmentType)
        {
            if (ParentContainer == null || ParentContainer.Fragment == null || FragmentExists) return;
            var container = (ContainerFragment) ParentContainer.Fragment;
            Assert(container != null, "Parent container fragment is not a container fragment");
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    Fragment = container.Body().AddProfile();
                    break;
                case FragmentType.Text:
                    Fragment = container.Body().AddText(GetFragmentName(container, fragmentType));
                    break;
                case FragmentType.Placeholder:
                    Fragment = container.Body().AddPlaceholder(GetFragmentName(container, fragmentType));
                    break;
                //case FragmentType.Body:
                //    break;
                case FragmentType.Segment:
                    Fragment = container.Body().AddSegment();
                    break;
                case FragmentType.Block:
                    Fragment = container.Body().AddBlock();
                    break;
                case FragmentType.Lookup:
                    Fragment = container.Body().AddLookup();
                    break;
                case FragmentType.Condition:
                    Fragment = container.Body().AddCondition();
                    break;
                case FragmentType.Function:
                    Fragment = container.Body().AddFunction();
                    break;
                case FragmentType.TextBlock:
                    Fragment = container.Body().AddTextBlock();
                    break;
                //case FragmentType.Null:
                //    Fragment = container.Body().AddFragment();
                //    break;
                default:
                    throw new ArgumentOutOfRangeException("fragmentType");
            }
        }

        private string GetFragmentName(ContainerFragment container, FragmentType fragmentType)
        {
            return fragmentType.ToString() + container.Body().FragmentList.Count;
        }
    }
}