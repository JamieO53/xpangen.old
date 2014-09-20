// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenLookupParams : GenFragmentParams
    {
        public GenLookupParams(GenDataDef genDataDef, GenContainerFragmentBase parentContainer, string condition, bool isPrimary = true)
            : base(genDataDef, parentContainer, FragmentType.Lookup, isPrimary: isPrimary)
        {
            var sa = condition.Split('=');
            Var1 = GenDataDef.GetId(sa[0]);
            Var2 = GenDataDef.GetId(sa[1]);
        }

        public GenDataId Var1 { get; private set; }
        public GenDataId Var2 { get; private set; }
    }
}