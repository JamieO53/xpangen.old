// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// Profile data content definition
    /// </summary>
    public class ProfileDefinition : GenApplicationBase
    {
        public ProfileDefinition(): this(new GenData(GetDefinition()))
        {
        }

        public ProfileDefinition(GenData genData)
        {
            GenData = genData;
            base.GenObject = genData.Root;
		}

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.Definition = "ProfileDefinition";
            f.AddSubClass("", "ProfileRoot");
            f.AddSubClass("ProfileRoot", "Fragment");
            f.AddSubClass("ProfileRoot", "Definition");
            f.AddSubClass("Definition", "Class");
            f.AddSubClass("Fragment", "BodyFragment");
            f.Classes[1].Properties.Add("Name");
            f.Classes[1].Properties.Add("Title");
            f.Classes[2].Properties.Add("Name");
            f.Classes[2].Properties.Add("Path");
            f.Classes[3].Properties.Add("Name");
            f.Classes[3].Properties.Add("FragmentType");
            f.Classes[4].Properties.Add("Name");
            return f;
        }

        public GenNamedApplicationList<ProfileRoot> ProfileRootList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ProfileRootList = new GenNamedApplicationList<ProfileRoot>(this);
        }

        public ProfileRoot AddProfileRoot(string name, string title = "")
        {
            var item = new ProfileRoot(GenData)
                           {
                               GenObject = GenData.CreateObject("", "ProfileRoot"),
                               Name = name,
                               Title = title
                           };
            ProfileRootList.Add(item);
            return item;
        }
    }
}
