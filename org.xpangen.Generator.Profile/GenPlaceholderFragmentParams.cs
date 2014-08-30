// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenPlaceholderFragmentParams : GenFragmentParams
    {
        public GenDataId Id { get; set; }

        public GenPlaceholderFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            GenContainerFragmentBase parentContainer, GenDataId id, bool isPrimary = true)
            : base(genDataDef, parentSegment, parentContainer, FragmentType.Placeholder, isPrimary)
        {
            Id = id;
        }

        public GenPlaceholderFragmentParams(GenDataDef genDataDef, Placeholder placeholder) : base(genDataDef, null, null, FragmentType.Placeholder)
        {
            Fragment = placeholder;
            Id = genDataDef.GetId(placeholder.Class + "." + placeholder.Property);
        }
    }
}