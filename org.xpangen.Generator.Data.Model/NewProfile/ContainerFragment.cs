// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// Fragment that contains other fragments
    /// </summary>
    public class ContainerFragment : Fragment
    {
        public ContainerFragment()
        {
        }

        public ContainerFragment(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
        }

        /// <summary>
        /// Generated name of the fragment
        /// </summary>
        public override string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                if (!DelayedSave) SaveFields();
            }
        }


        protected override void GenObjectSetNotification()
        {
        }
    }
}
