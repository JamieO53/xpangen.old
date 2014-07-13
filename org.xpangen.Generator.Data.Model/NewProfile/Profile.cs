// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.NewProfile
{
    /// <summary>
    /// The container fragment for the profile
    /// </summary>
    public class Profile : GenNamedApplicationBase
    {
        public Profile()
        {
        }

        public Profile(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
        }

        /// <summary>
        /// The name of the profile fragment
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

        public GenNamedApplicationList<FragmentBody> FragmentBodyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            FragmentBodyList = new GenNamedApplicationList<FragmentBody>(this);
        }

        public FragmentBody AddFragmentBody(string name)
        {
            var item = new FragmentBody(GenData)
                           {
                               GenObject = GenData.CreateObject("Profile", "FragmentBody"),
                               Name = name
                           };
            FragmentBodyList.Add(item);
            return item;
        }

    }
}
