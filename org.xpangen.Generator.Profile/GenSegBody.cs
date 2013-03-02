// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Text;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenSegBody : GenFragment
    {
        private readonly List<GenFragment> _fragment;

        public GenSegBody(GenDataDef genDataDef, GenContainerFragmentBase parentSegment) : base(genDataDef, parentSegment)
        {
            _fragment = new List<GenFragment>();
            FragmentType = FragmentType.Body;
        }

        public IList<GenFragment> Fragment
        {
            get { return _fragment; }
        }

        public int Count
        {
            get { return _fragment.Count; }
        }

        public override string ProfileLabel()
        {
            return "Body";
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var s = new StringBuilder();
            foreach (var fragment in Fragment)
                s.Append(fragment.ProfileText(syntaxDictionary));
            return s.ToString();
        }

        public override string Expand(GenData genData)
        {
            var s = new StringBuilder();
            foreach (var fragment in Fragment)
                s.Append(fragment.Expand(genData));
            return s.ToString();
        }

        public void Add(GenFragment fragment)
        {
            _fragment.Add(fragment);
            fragment.ParentSegement = ParentSegement;
        }

        public int IndexOf(GenFragment fragment)
        {
            return _fragment.IndexOf(fragment);
        }
    }
}
