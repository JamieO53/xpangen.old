// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
            ((Segment) Fragment).Cardinality = genSegmentParams.Cardinality.ToString();
        }
    }
}