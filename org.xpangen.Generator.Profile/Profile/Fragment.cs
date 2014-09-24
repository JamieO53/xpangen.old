// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// The root class of all profile fragments
    /// </summary>
    public class Fragment : GenNamedApplicationBase
    {
        public Fragment()
        {
            SubClasses.Add("Null");
            SubClasses.Add("Text");
            SubClasses.Add("Placeholder");
            SubClasses.Add("ContainerFragment");
            Properties.Add("Name");
        }

        public Fragment(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }


        protected override void GenObjectSetNotification()
        {
        }
    }
}
