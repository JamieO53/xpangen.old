// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// The root class of all profile fragments
    /// </summary>
    public class Fragment : GenNamedApplicationBase
    {
        public Fragment()
        {
        }

        public Fragment(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
            Properties.Add("FragmentType");
        }

        /// <summary>
        /// The type of the fragment
        /// </summary>
        public string FragmentType
        {
            get { return AsString("FragmentType"); }
            set
            {
                if (FragmentType == value) return;
                SetString("FragmentType", value);
                if (!DelayedSave) SaveFields();
            }
        }


        protected override void GenObjectSetNotification()
        {
        }
    }
}
