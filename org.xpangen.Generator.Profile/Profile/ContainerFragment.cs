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
            SubClasses.Add("Annotation");
            Properties.Add("Name");
            Properties.Add("Primary");
            Properties.Add("Secondary");
        }

        public ContainerFragment(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// The primary fragment body of the container fragment
        /// </summary>
        public string Primary
        {
            get { return AsString("Primary"); }
            set
            {
                if (Primary == value) return;
                SetString("Primary", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The secondary fragment body of the container fragment
        /// </summary>
        public string Secondary
        {
            get { return AsString("Secondary"); }
            set
            {
                if (Secondary == value) return;
                SetString("Secondary", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public FragmentBody Body()
        {
            return (FragmentBody) Links["PrimaryBody"];
        }

        public FragmentBody SecondaryBody()
        {
            return (FragmentBody) Links["SecondaryBody"];
        }

        public FragmentBody CheckBody()
        {
            var root = ProfileDefinition.ProfileRoot();
            if (Primary == "Empty1")
            {
                Primary = root.CreateContainerFragmentBody(GetType().Name);
                Links["PrimaryBody"] = ProfileDefinition.ProfileRoot().FragmentBodyList.Find(Primary);
                Body().Links.Add("Parent", this);
            }
            return root.FragmentBodyList.Find(Primary);
        }

        public FragmentBody CheckSecondaryBody()
        {
            var root = ProfileDefinition.ProfileRoot();
            if (Secondary == "Empty1")
            {
                Secondary = root.CreateContainerFragmentBody(GetType().Name);
                Links["SecondaryBody"] = ProfileDefinition.ProfileRoot().FragmentBodyList.Find(Secondary);
                SecondaryBody().Links.Add("Parent", this);
            }
            return root.FragmentBodyList.Find(Secondary);
        }

        protected override void GenObjectSetNotification()
        {
        }
    }
}
