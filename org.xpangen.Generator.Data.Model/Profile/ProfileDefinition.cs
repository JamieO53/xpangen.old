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
            f.AddSubClass("ProfileRoot", "Definition");
            f.AddSubClass("ProfileRoot", "FragmentBody");
            f.AddSubClass("Definition", "Class");
            f.AddSubClass("FragmentBody", "Fragment");
            f.AddSubClass("Fragment", "Null");
            f.AddSubClass("Fragment", "Text");
            f.AddSubClass("Fragment", "Placeholder");
            f.AddSubClass("Fragment", "Body");
            f.AddSubClass("Fragment", "Segment");
            f.AddSubClass("Fragment", "Block");
            f.AddSubClass("Fragment", "Lookup");
            f.AddSubClass("Fragment", "Condition");
            f.AddSubClass("Fragment", "Function");
            f.AddSubClass("Fragment", "TextBlock");
            f.AddSubClass("Fragment", "Profile");
            f.Classes[1].Properties.Add("Name");
            f.Classes[1].Properties.Add("Title");
            f.Classes[2].Properties.Add("Name");
            f.Classes[2].Properties.Add("Path");
            f.Classes[3].Properties.Add("Name");
            f.Classes[4].Properties.Add("Name");
            f.Classes[4].Properties.Add("FragmentType");
            f.Classes[4].Properties.Add("Body");
            f.Classes[5].Properties.Add("Name");
            f.Classes[6].Properties.Add("Name");
            f.Classes[6].Properties.Add("TextValue");
            f.Classes[7].Properties.Add("Name");
            f.Classes[7].Properties.Add("Class");
            f.Classes[7].Properties.Add("Property");
            f.Classes[8].Properties.Add("Name");
            f.Classes[9].Properties.Add("Name");
            f.Classes[10].Properties.Add("Name");
            f.Classes[10].Properties.Add("Class");
            f.Classes[10].Properties.Add("Cardinality");
            f.Classes[11].Properties.Add("Name");
            f.Classes[12].Properties.Add("Name");
            f.Classes[12].Properties.Add("NoMatch");
            f.Classes[12].Properties.Add("Class1");
            f.Classes[12].Properties.Add("Property1");
            f.Classes[12].Properties.Add("Class2");
            f.Classes[12].Properties.Add("Property2");
            f.Classes[13].Properties.Add("Name");
            f.Classes[13].Properties.Add("Class1");
            f.Classes[13].Properties.Add("Property1");
            f.Classes[13].Properties.Add("Class2");
            f.Classes[13].Properties.Add("Property2");
            f.Classes[13].Properties.Add("Lit");
            f.Classes[13].Properties.Add("UseLit");
            f.Classes[14].Properties.Add("Name");
            f.Classes[14].Properties.Add("FunctionName");
            f.Classes[15].Properties.Add("Name");
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
