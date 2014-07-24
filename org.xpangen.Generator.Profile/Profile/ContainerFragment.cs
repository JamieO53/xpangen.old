// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// Fragment that contains other fragments
    /// </summary>
    public class ContainerFragment : Fragment
    {
        public ContainerFragment()
        {
            SubClasses.Add("Profile");
            SubClasses.Add("Segment");
            SubClasses.Add("Block");
            SubClasses.Add("Lookup");
            SubClasses.Add("Condition");
            SubClasses.Add("Function");
            SubClasses.Add("TextBlock");
            Properties.Add("Name");
        }

        public ContainerFragment(GenData genData) : this()
        {
            GenData = genData;
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

        public GenNamedApplicationList<Profile> ProfileList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            ProfileList = new GenNamedApplicationList<Profile>(this, 7, 0);
        }

        public Profile AddProfile(string name)
        {
            var item = new Profile(GenData)
                           {
                               GenObject = GenData.CreateObject("ContainerFragment", "Profile"),
                               Name = name
                           };
            ProfileList.Add(item);
            return item;
        }

    }
}
