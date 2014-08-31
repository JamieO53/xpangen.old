// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenPlaceholderFragment : GenFragment
    {
        public GenPlaceholderFragment(GenPlaceholderFragmentParams genPlaceholderFragmentParams) :
            base(genPlaceholderFragmentParams.SetFragmentType(FragmentType.Placeholder))
        {
            Id = genPlaceholderFragmentParams.Id;
        }

        private GenDataId Id {
            get
            {
                return GenDataDef.GetId(Placeholder.Class + "." + Placeholder.Property);
            } 
            set {
                Placeholder.Class = value.ClassName;
                Placeholder.Property = value.PropertyName;
            } 
        }

        private Placeholder Placeholder { get { return (Placeholder) Fragment; } }
        
        public override string ProfileLabel()
        {
            return Id.Identifier;
        }
    }
}