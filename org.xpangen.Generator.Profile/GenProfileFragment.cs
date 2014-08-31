// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenProfileFragment : GenContainerFragmentBase
    {
        public GenProfileFragment(GenDataDef genDataDef)
            : this(new GenProfileParams(genDataDef))
        {
        }

        public GenProfileFragment(GenProfileParams genProfileParams) : base(genProfileParams)
        {
            ClassId = 0;
            Body.ParentSegment = this;
        }

        public Profile.Profile Profile
        {
            get { return (Profile.Profile)Fragment; }
        }
        public override string ProfileLabel()
        {
            return "Profile";
        }
    }
}