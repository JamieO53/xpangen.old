// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics.Contracts;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentParams
    {
        private Fragment _fragment;

        /// <summary>
        /// Parameters for creating a GenFragment
        /// </summary>
        /// <param name="genDataDef">The definition of the data being generated.</param>
        /// <param name="parentContainer">The container fragment conataining this fragment.</param>
        /// <param name="fragmentType">The type of fragment.</param>
        /// <param name="isPrimary">Is this fragmnent in the primary body?</param>
        public GenFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentContainer, FragmentType fragmentType, bool isPrimary = true)
        {
            Contract.Requires(parentContainer != null);
            Contract.Ensures(fragmentType == FragmentType.Profile || Fragment != null);
            GenDataDef = genDataDef;
            ParentContainer = parentContainer;
            Container = ParentContainer == null ? null : (ContainerFragment)ParentContainer.Fragment;
            IsPrimary = isPrimary;
            SetFragmentType(fragmentType);
        }

        /// <summary>
        /// Parameters for creating a GenFragment
        /// </summary>
        /// <param name="genDataDef">The definition of the data being generated.</param>
        /// <param name="parentContainer">The container fragment conataining this fragment.</param>
        /// <param name="isPrimary">Is this fragmnent in the primary body?</param>
        public GenFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentContainer, bool isPrimary = true)
        {
            Contract.Requires(parentContainer != null && parentContainer != null);
            GenDataDef = genDataDef;
            ParentContainer = parentContainer;
            Container = ParentContainer == null ? null : (ContainerFragment)ParentContainer.Fragment;
            IsPrimary = isPrimary;
        }

        protected GenFragmentParams(GenDataDef genDataDef, Fragment fragment, FragmentType fragmentType)
        {
            Contract.Requires(fragment != null && fragment.GetType().Name == fragmentType.ToString());
            Contract.Ensures(Fragment != null && Fragment.GetType().Name == FragmentType.ToString());
            _fragment = fragment;
            GenDataDef = genDataDef;
            FragmentType = fragmentType;
        }

        public GenFragmentParams SetFragmentType(FragmentType fragmentType)
        {
            Contract.Requires(Container != null || Fragment != null);
            Contract.Ensures(Fragment != null && Fragment.GetType().Name == FragmentType.ToString());
            FragmentType = fragmentType;
            if (FragmentExists) return this;
            var fragmentBody = IsPrimary ? Container.CheckBody() : Container.CheckSecondaryBody();
            CheckFragment(fragmentType, fragmentBody);
            return this;
        }

        public ContainerFragment Container { get; set; }

        public Fragment Fragment
        {
            get
            {
                Contract.Requires(FragmentExists, "Fragment expected");
                return _fragment;
            }
            private set { _fragment = value; }
        }

        public bool FragmentExists
        {
            get { return _fragment != null; }
        }

        public GenDataDef GenDataDef { get; private set; }

        public GenContainerFragmentBase ParentContainer { get; private set; }
        private bool IsPrimary { get; set; }

        public FragmentType FragmentType { get; private set; }

        private void CheckFragment(FragmentType fragmentType, FragmentBody fragmentBody)
        {
            Contract.Ensures(Fragment != null);
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
                case FragmentType.Annotation:
                    Fragment = fragmentBody.AddAnnotation();
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
                default:
                    throw new ArgumentOutOfRangeException("fragmentType");
            }
        }
    }
}