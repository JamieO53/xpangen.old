// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.NewProfile
{
    /// <summary>
    /// Fragment that contains other fragments
    /// </summary>
    public class ContainerFragment : Fragment
    {
        public ContainerFragment()
        {
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


        protected override void GenObjectSetNotification()
        {
        }
    }
}
