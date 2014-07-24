// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Text;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenSegBody : GenBase
    {
        private readonly List<GenFragment> _fragment;

        public GenSegBody(GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer)
        {
            _fragment = new List<GenFragment>();
            ParentSegment = parentSegment;
            ParentContainer = parentContainer;
        }

        public IList<GenFragment> Fragment
        {
            get { return _fragment; }
        }

        public int Count
        {
            get { return _fragment.Count; }
        }

        public string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var s = new StringBuilder();
            foreach (var fragment in Fragment)
                s.Append(fragment.ProfileText(syntaxDictionary));
            return s.ToString();
        }

        public string Expand(GenData genData)
        {
            var s = new StringBuilder();
            foreach (var fragment in Fragment)
                s.Append(fragment.Expand(genData));
            return s.ToString();
        }

        public void Add(GenFragment fragment)
        {
            _fragment.Add(fragment);
            fragment.ParentSegment = ParentSegment;
            fragment.ParentContainer = ParentSegment;
        }

        public GenContainerFragmentBase ParentSegment { get; set; }
        public GenContainerFragmentBase ParentContainer { get; set; }

        public int IndexOf(GenFragment fragment)
        {
            return _fragment.IndexOf(fragment);
        }

        public bool Generate(GenFragment prefix, GenData genData, GenWriter writer)
        {
            var generated = false;
            foreach (var fragment in Fragment)
            {
                generated |= fragment.Generate(prefix, genData, writer);
            }
            return generated;
        }
    }
}