// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
        /// <param name="isPrimary">Is this fragmnent in the primary body?</param>
        public GenFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, FragmentType fragmentType, bool isPrimary = true)
        {
            GenDataDef = genDataDef;
            ParentSegment = parentSegment;
            ParentContainer = parentContainer;
            IsPrimary = isPrimary;
            SetFragmentType(fragmentType);
            //Assert(Fragment != null, "Fragment expected");
        }

        public GenFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, bool isPrimary = true)
        {
            GenDataDef = genDataDef;
            ParentSegment = parentSegment;
            ParentContainer = parentContainer;
            IsPrimary = isPrimary;
        }

        public GenFragmentParams SetFragmentType(FragmentType fragmentType)
        {
            FragmentType = fragmentType;
            if (ParentContainer == null || ParentContainer.Fragment == null || FragmentExists) return this;
            var container = (ContainerFragment)ParentContainer.Fragment;
            Assert(container != null, "Parent container fragment is not a container fragment");
            var fragmentBody = IsPrimary ? container.CheckBody() : container.CheckSecondaryBody();
            CheckFragment(fragmentType, fragmentBody);
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
        private bool IsPrimary { get; set; }

        public FragmentType FragmentType { get; protected set; }

        private void CheckFragment(FragmentType fragmentType, FragmentBody fragmentBody)
        {
            if (FragmentExists) return;
            
            var fragmentName = fragmentBody.FragmentName(fragmentType);
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    Fragment = fragmentBody.AddProfile();
                    break;
                case FragmentType.Text:
                    Fragment = fragmentBody.AddText(fragmentName);
                    break;
                case FragmentType.Placeholder:
                    Fragment = fragmentBody.AddPlaceholder(fragmentName);
                    break;
                case FragmentType.Segment:
                    var segmentParams = ((GenSegmentParams) this);
                    Fragment = fragmentBody.AddSegment(segmentParams.ClassName, segmentParams.Cardinality.ToString());
                    break;
                case FragmentType.Block:
                    Fragment = fragmentBody.AddBlock();
                    break;
                case FragmentType.Lookup:
                    Fragment = fragmentBody.AddLookup();
                    break;
                case FragmentType.Condition:
                    Fragment = fragmentBody.AddCondition();
                    break;
                case FragmentType.Function:
                    Fragment = fragmentBody.AddFunction();
                    break;
                case FragmentType.TextBlock:
                    Fragment = fragmentBody.AddTextBlock();
                    break;
                    //case FragmentType.Null:
                    //    Fragment = container.Body().AddFragment();
                    //    break;
                default:
                    throw new ArgumentOutOfRangeException("fragmentType");
            }
        }
    }
}