// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenProfileParams : GenFragmentParams
    {
        public GenProfileParams(GenDataDef genDataDef, Profile.Profile profile = null) : base(genDataDef, null, null, FragmentType.Profile)
        {
            if (profile == null)
            {
                ProfileDefinition = new ProfileDefinition();
                ProfileDefinition.Setup();
                Fragment = ProfileDefinition.Profile();
                FragmentType = FragmentType.Profile;
            }
            else
            {
                ProfileDefinition = profile.ProfileDefinition();
                Fragment = profile;
                FragmentType = FragmentType.Profile;
            }
            FragmentType = FragmentType.Profile;
        }

        private ProfileDefinition ProfileDefinition { get; set; }
    }
}