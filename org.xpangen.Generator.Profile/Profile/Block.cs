// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// The Block fragment data
    /// </summary>
    public class Block : ContainerFragment
    {
        public Block()
        {
            Properties.Add("Name");
        }

        public Block(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }


    }
}
