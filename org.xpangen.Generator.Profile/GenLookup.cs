// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenLookup : GenContainerFragmentBase
    {
        public bool NoMatch
        {
            get { return Body.SecondaryCount > 0; } 
        }

        public new int ClassId { get; private set; }

        public GenLookup(GenLookupParams genLookupParams)
            : base(genLookupParams.SetFragmentType(FragmentType.Lookup))
        {
            Body.ParentSegment = this;
            var lookup = (Lookup) Fragment;
            lookup.Class1 = genLookupParams.Var1.ClassName;
            lookup.Property1 = genLookupParams.Var1.PropertyName;
            lookup.Class2 = genLookupParams.Var2.ClassName;
            lookup.Property2 = genLookupParams.Var2.PropertyName;
            ClassId = GenDataDef.Classes.IndexOf(lookup.Class1);
        }
    }
}