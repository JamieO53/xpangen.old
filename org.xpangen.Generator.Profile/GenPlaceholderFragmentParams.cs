// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenPlaceholderFragmentParams : GenFragmentParams
    {
        public GenDataId Id { get; private set; }

        public GenPlaceholderFragmentParams(GenDataDef genDataDef, GenContainerFragmentBase parentContainer, GenDataId id, bool isPrimary = true)
            : base(genDataDef, parentContainer, FragmentType.Placeholder, isPrimary: isPrimary)
        {
            Id = id;
        }
    }
}