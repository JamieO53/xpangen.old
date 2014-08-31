// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public abstract class GenFragment : GenBase
    {
        private static GenNullFragment NullFragment
        {
            get { return _nullFragment ?? (_nullFragment = new GenNullFragment()); }
        }

        private FragmentType _fragmentType;
        private Fragment _fragment;
        private static GenNullFragment _nullFragment;
        public int ClassId { get; private set; }

        /// <summary>
        /// The fragment object holding the fragment's data
        /// </summary>
        public Fragment Fragment
        {
            get { return _fragment; }
            protected set
            {
                _fragment = value;
                CheckFragmentType(value);
            }
        }

        public GenObject GenObject { get; set; }
        private void CheckFragmentType(Fragment fragment)
        {
            if (fragment != null) Enum.TryParse(fragment.GetType().Name, out _fragmentType);
        }

        /// <summary>
        ///     The fragment type.
        /// </summary>
        public FragmentType FragmentType
        {
            get { return _fragmentType; }
            private set { _fragmentType = value; }
        }

        public GenDataDef GenDataDef { get; private set; }
        /// <summary>
        ///     Is this a text fragment?
        /// </summary>
        public bool IsTextFragment
        {
            get
            {
                return FragmentType == FragmentType.Text ||
                       FragmentType == FragmentType.Placeholder ||
                       FragmentType == FragmentType.Null;
            }
        }

        public GenContainerFragmentBase ParentSegment { get; set; }
        public GenContainerFragmentBase ParentContainer { get; set; }

        /// <summary>
        ///     Create a new <see cref="GenFragment" /> object.
        /// </summary>
        /// <param name="genFragmentParams">Data need to create the fragment object</param>
        protected GenFragment(GenFragmentParams genFragmentParams)
        {
            GenDataDef = genFragmentParams.GenDataDef;
            ParentSegment = genFragmentParams.ParentSegment;
            ParentContainer = genFragmentParams.ParentContainer;
            FragmentType = genFragmentParams.FragmentType;
            Fragment = genFragmentParams.Fragment;
            ClassId = genFragmentParams.ClassId;
            Assert(Fragment != null, "The fragment was not set up");
        }

        /// <summary>
        ///     A label identifying the fragment for browsing.
        /// </summary>
        /// <returns>The fragment label.</returns>
        public abstract string ProfileLabel();

        /// <summary>
        ///     Profile text equivalent to the fragment.
        /// </summary>
        /// <param name="syntaxDictionary">The dictionary defining the syntax of the profile text.</param>
        /// <returns>The fragment's profile text.</returns>
        public string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary = null)
        {
            var dictionary = syntaxDictionary ?? ProfileFragmentSyntaxDictionary.ActiveProfileFragmentSyntaxDictionary;
            var t = new GenProfileTextExpander(dictionary);
            return t.GetText(Fragment);
        }

        public static GenFragment Create(GenDataDef genDataDef, Fragment fragment)
        {
            FragmentType fragmentType;
            if (!Enum.TryParse(fragment.GetType().Name, out fragmentType))
                throw new ArgumentException("Fragment type not known", "fragment");
            switch (fragmentType)
            {
                case FragmentType.Profile:
                    return new GenProfileFragment(new GenProfileParams(genDataDef, (Profile.Profile)fragment));
                case FragmentType.Null:
                    return NullFragment;
                case FragmentType.Text:
                    return new GenTextFragment(new GenTextFragmentParams(genDataDef, (Text)fragment));
                case FragmentType.Placeholder:
                    return new GenPlaceholderFragment(new GenPlaceholderFragmentParams(genDataDef, (Placeholder)fragment));
                case FragmentType.Segment:
                    return new GenSegment(new GenSegmentParams(genDataDef, (Segment)fragment));
                case FragmentType.Block:
                    return new GenBlock(new GenFragmentParams(genDataDef, fragment));
                case FragmentType.Lookup:
                    return new GenLookup(new GenLookupParams(genDataDef, (Lookup)fragment));
                case FragmentType.Condition:
                    return new GenCondition(new GenConditionParams(genDataDef, (Condition)fragment));
                case FragmentType.Function:
                    return new GenFunction(new GenFragmentParams(genDataDef, fragment));
                case FragmentType.TextBlock:
                    return new GenTextBlock(new GenFragmentParams(genDataDef, fragment));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}