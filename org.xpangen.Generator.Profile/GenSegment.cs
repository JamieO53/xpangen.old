// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenSegment : GenContainerFragmentBase
    {
        public GenSegment(GenSegmentParams genSegmentParams)
            : base(genSegmentParams.SetFragmentType(FragmentType.Segment))
        {
            Body.ParentSegment = this;
            ClassId = GenDataDef.Classes.IndexOf(genSegmentParams.ClassName);
            GenCardinality = genSegmentParams.Cardinality;
        }

        public GenCardinality GenCardinality 
        {
            get
            {
                GenCardinality c;
                Enum.TryParse(Segment.Cardinality, out c);
                return c;
            }
            private set { Segment.Cardinality = value.ToString(); } 
        }

        private Segment Segment
        {
            get { return (Segment) Fragment; }
        }
        public override string ProfileLabel()
        {
            return GenDataDef.Classes[ClassId].Name;
        }
    }
}