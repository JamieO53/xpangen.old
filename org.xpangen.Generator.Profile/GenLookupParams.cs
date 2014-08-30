// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenLookupParams : GenFragmentParams
    {
        private readonly string _condition;

        public GenLookupParams(GenDataDef genDataDef, GenContainerFragmentBase parentSegment,
            GenContainerFragmentBase parentContainer, string condition, bool isPrimary = true)
            : base(genDataDef, parentSegment, parentContainer, FragmentType.Lookup, isPrimary)
        {
            _condition = condition;
            var sa = condition.Split('=');
            Var1 = GenDataDef.GetId(sa[0]);
            Var2 = GenDataDef.GetId(sa[1]);
        }

        public GenLookupParams(GenDataDef genDataDef, Lookup lookup) : base(genDataDef, null, null, FragmentType.Lookup)
        {
            Var1 = GenDataDef.GetId(lookup.Class1, lookup.Property1);
            Var2 = GenDataDef.GetId(lookup.Class2, lookup.Property2);
            Fragment = lookup;
        }

        public string Condition
        {
            get { return _condition; }
        }

        public GenDataId Var1 { get; private set; }
        public GenDataId Var2 { get; private set; }
    }
}