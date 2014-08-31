﻿// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Text;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenSegBody : GenBase
    {
        private readonly List<GenFragment> _fragment;
        private IList<GenFragment> _secondaryFragment;

        public GenSegBody(GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer)
        {
            _fragment = new List<GenFragment>();
            _secondaryFragment = new List<GenFragment>();
            ParentSegment = parentSegment;
            ParentContainer = parentContainer;
        }

        public GenObject GenObject { get; set; }
        public IList<GenFragment> Fragment
        {
            get { return _fragment; }
        }

        public IList<GenFragment> SecondaryFragment
        {
            get { return _secondaryFragment; }
        }

        public int Count
        {
            get { return _fragment.Count; }
        }

        public int SecondaryCount
        {
            get { return _secondaryFragment.Count; }
        }

        public void Add(GenFragment fragment)
        {
            _fragment.Add(fragment);
            fragment.ParentSegment = ParentSegment;
            fragment.ParentContainer = ParentContainer;
        }

        public void AddSecondary(GenFragment secondaryFragment)
        {
            _secondaryFragment.Add(secondaryFragment);
            secondaryFragment.ParentSegment = ParentSegment;
            secondaryFragment.ParentContainer = ParentContainer;
        }

        public GenContainerFragmentBase ParentSegment { private get; set; }
        public GenContainerFragmentBase ParentContainer { get; private set; }

        public string SecondaryProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var s = new StringBuilder();
            foreach (var fragment in SecondaryFragment)
                s.Append(fragment.ProfileText(syntaxDictionary));
            return s.ToString();
        }
    }
}