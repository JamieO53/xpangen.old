// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// The root node of the profile
    /// </summary>
    public class ProfileRoot : GenNamedApplicationBase
    {
        public ProfileRoot()
        {
            SubClasses.Add("FragmentBody");
            Properties.Add("Name");
            Properties.Add("Title");
        }

        public ProfileRoot(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        internal string CreateContainerFragmentBody(string prefix)
        {
            var name = prefix + FragmentBodyList.Count;
            AddFragmentBody(name);
            return name;
        }

        /// <summary>
        /// The description of the profile root for the data editor hint
        /// </summary>
        public string Title
        {
            get { return AsString("Title"); }
            set
            {
                if (Title == value) return;
                SetString("Title", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<FragmentBody> FragmentBodyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            FragmentBodyList = new GenNamedApplicationList<FragmentBody>(this, 2, 0);
        }

        public FragmentBody AddFragmentBody(string name)
        {
            var item = new FragmentBody(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("FragmentBody"),
                               Name = name
                           };
            FragmentBodyList.Add(item);
            return item;
        }

    }
}
