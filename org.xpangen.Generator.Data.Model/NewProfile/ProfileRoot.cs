// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

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
            Properties.Add("Name");
            Properties.Add("Title");
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

        public GenNamedApplicationList<Definition> DefinitionList { get; private set; }
        public GenNamedApplicationList<Profile> ProfileList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            DefinitionList = new GenNamedApplicationList<Definition>(this);
            base.GenObjectSetNotification();
            ProfileList = new GenNamedApplicationList<Profile>(this);
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


        public Profile AddProfile(string name)
        {
            var item = new Profile(GenData)
                           {
                               GenObject = GenData.CreateObject("ProfileRoot", "Profile"),
                               Name = name
                           };
            ProfileList.Add(item);
            return item;
        }

    }
}
