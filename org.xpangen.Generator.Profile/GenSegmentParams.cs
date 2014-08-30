// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenSegmentParams : GenFragmentParams
    {
        private readonly string _className;
        private readonly GenCardinality _cardinality;

        public GenSegmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment, GenContainerFragmentBase parentContainer, string className, GenCardinality cardinality, bool isPrimary = true)
            : base(genDataDef, parentSegment, parentContainer, isPrimary)
        {
            _className = className;
            _cardinality = cardinality;
        }

        public GenSegmentParams(GenDataDef genDataDef, Segment segment) : base(genDataDef, null, null, FragmentType.Segment)
        {
            Fragment = segment;
            _className = segment.Class;
            if (!Enum.TryParse(segment.Cardinality, out _cardinality))
                throw new ArgumentException("Invalid cardinality: " + segment.Cardinality);
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