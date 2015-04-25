// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// The container fragment for the profile
    /// </summary>
    public class Profile : ContainerFragment
    {
        public Profile()
        {
            Properties.Add("Name");
        }

        public Profile(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }


    }
}
