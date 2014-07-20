// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile.Profile
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
            Classes.Add("ProfileRoot");
            Classes.Add("Definition");
            Classes.Add("Profile");
            Classes.Add("FragmentBody");
            Classes.Add("Fragment");
            Classes.Add("Null");
            Classes.Add("Text");
            Classes.Add("Placeholder");
            Classes.Add("ContainerFragment");
            Classes.Add("Segment");
            Classes.Add("Block");
            Classes.Add("Lookup");
            Classes.Add("Condition");
            Classes.Add("Function");
            Classes.Add("TextBlock");
            SubClasses.Add("ProfileRoot");
            base.GenObject = genData.Root;
        }

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.DefinitionName = "ProfileDefinition";
            f.AddSubClass("", "ProfileRoot");
            f.AddSubClass("ProfileRoot", "Definition");
            f.AddSubClass("ProfileRoot", "Profile");
            f.AddSubClass("Definition", "Class");
            f.AddSubClass("Profile", "FragmentBody");
            f.AddSubClass("FragmentBody", "Fragment");
            f.AddSubClass("Fragment", "Null");
            f.AddSubClass("Fragment", "Text");
            f.AddSubClass("Fragment", "Placeholder");
            f.AddSubClass("Fragment", "ContainerFragment");
            f.AddSubClass("ContainerFragment", "Segment");
            f.AddSubClass("ContainerFragment", "Block");
            f.AddSubClass("ContainerFragment", "Lookup");
            f.AddSubClass("ContainerFragment", "Condition");
            f.AddSubClass("ContainerFragment", "Function");
            f.AddSubClass("ContainerFragment", "TextBlock");
            f.Classes[1].AddInstanceProperty("Name");
            f.Classes[1].AddInstanceProperty("Title");
            f.Classes[2].AddInstanceProperty("Name");
            f.Classes[2].AddInstanceProperty("Path");
            f.Classes[3].AddInstanceProperty("Name");
            f.Classes[4].AddInstanceProperty("Name");
            f.Classes[5].AddInstanceProperty("Name");
            f.Classes[6].AddInstanceProperty("Name");
            f.Classes[7].AddInstanceProperty("Name");
            f.Classes[7].AddInstanceProperty("TextValue");
            f.Classes[8].AddInstanceProperty("Name");
            f.Classes[8].AddInstanceProperty("Class");
            f.Classes[8].AddInstanceProperty("Property");
            f.Classes[9].AddInstanceProperty("Name");
            f.Classes[10].AddInstanceProperty("Name");
            f.Classes[10].AddInstanceProperty("Class");
            f.Classes[10].AddInstanceProperty("Cardinality");
            f.Classes[11].AddInstanceProperty("Name");
            f.Classes[12].AddInstanceProperty("Name");
            f.Classes[12].AddInstanceProperty("NoMatch");
            f.Classes[12].AddInstanceProperty("Class1");
            f.Classes[12].AddInstanceProperty("Property1");
            f.Classes[12].AddInstanceProperty("Class2");
            f.Classes[12].AddInstanceProperty("Property2");
            f.Classes[13].AddInstanceProperty("Name");
            f.Classes[13].AddInstanceProperty("Class1");
            f.Classes[13].AddInstanceProperty("Property1");
            f.Classes[13].AddInstanceProperty("Comparison");
            f.Classes[13].AddInstanceProperty("Class2");
            f.Classes[13].AddInstanceProperty("Property2");
            f.Classes[13].AddInstanceProperty("Lit");
            f.Classes[13].AddInstanceProperty("UseLit");
            f.Classes[14].AddInstanceProperty("Name");
            f.Classes[14].AddInstanceProperty("FunctionName");
            f.Classes[15].AddInstanceProperty("Name");
            return f;
        }

        public GenNamedApplicationList<ProfileRoot> ProfileRootList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ProfileRootList = new GenNamedApplicationList<ProfileRoot>(this, 1, 0);
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
