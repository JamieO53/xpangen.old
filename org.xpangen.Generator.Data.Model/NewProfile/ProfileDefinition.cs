// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.NewProfile
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
            f.Classes[1].InstanceProperties.Add("Name");
            f.Classes[1].InstanceProperties.Add("Title");
            f.Classes[2].InstanceProperties.Add("Name");
            f.Classes[2].InstanceProperties.Add("Path");
            f.Classes[3].InstanceProperties.Add("Name");
            f.Classes[4].InstanceProperties.Add("Name");
            f.Classes[5].InstanceProperties.Add("Name");
            f.Classes[5].InstanceProperties.Add("FragmentType");
            f.Classes[6].InstanceProperties.Add("Name");
            f.Classes[7].InstanceProperties.Add("Name");
            f.Classes[7].InstanceProperties.Add("TextValue");
            f.Classes[8].InstanceProperties.Add("Name");
            f.Classes[8].InstanceProperties.Add("Class");
            f.Classes[8].InstanceProperties.Add("Property");
            f.Classes[9].InstanceProperties.Add("Name");
            f.Classes[10].InstanceProperties.Add("Name");
            f.Classes[10].InstanceProperties.Add("Class");
            f.Classes[10].InstanceProperties.Add("Cardinality");
            f.Classes[11].InstanceProperties.Add("Name");
            f.Classes[12].InstanceProperties.Add("Name");
            f.Classes[12].InstanceProperties.Add("NoMatch");
            f.Classes[12].InstanceProperties.Add("Class1");
            f.Classes[12].InstanceProperties.Add("Property1");
            f.Classes[12].InstanceProperties.Add("Class2");
            f.Classes[12].InstanceProperties.Add("Property2");
            f.Classes[13].InstanceProperties.Add("Name");
            f.Classes[13].InstanceProperties.Add("Class1");
            f.Classes[13].InstanceProperties.Add("Property1");
            f.Classes[13].InstanceProperties.Add("Class2");
            f.Classes[13].InstanceProperties.Add("Property2");
            f.Classes[13].InstanceProperties.Add("Lit");
            f.Classes[13].InstanceProperties.Add("UseLit");
            f.Classes[14].InstanceProperties.Add("Name");
            f.Classes[14].InstanceProperties.Add("FunctionName");
            f.Classes[15].InstanceProperties.Add("Name");
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
