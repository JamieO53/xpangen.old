// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// The root node of the profile
    /// </summary>
    public class ProfileRoot : GenNamedApplicationBase
    {
        public ProfileRoot()
        {
        }

        public ProfileRoot(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The name of the profile root property
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

        public GenNamedApplicationList<Fragment> FragmentList { get; private set; }
        public GenNamedApplicationList<Definition> DefinitionList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            FragmentList = new GenNamedApplicationList<Fragment>(this);
            DefinitionList = new GenNamedApplicationList<Definition>(this);
        }

        public Fragment AddFragment(string name, string fragmentType = "")
        {
            var item = new Fragment(GenData)
                           {
                               GenObject = GenData.CreateObject("ProfileRoot", "Fragment"),
                               Name = name,
                               FragmentType = fragmentType
                           };
            FragmentList.Add(item);
            return item;
        }


        public Definition AddDefinition(string name, string path = "")
        {
            var item = new Definition(GenData)
                           {
                               GenObject = GenData.CreateObject("ProfileRoot", "Definition"),
                               Name = name,
                               Path = path
                           };
            DefinitionList.Add(item);
            return item;
        }
    }
}
