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
            Classes.Add("FragmentBody");
            Classes.Add("Fragment");
            Classes.Add("Text");
            Classes.Add("Placeholder");
            Classes.Add("ContainerFragment");
            Classes.Add("Profile");
            Classes.Add("Segment");
            Classes.Add("Block");
            Classes.Add("Lookup");
            Classes.Add("Condition");
            Classes.Add("Function");
            Classes.Add("TextBlock");
            SubClasses.Add("FragmentBody");
            base.GenObject = genData.Root;
        }

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.DefinitionName = "ProfileDefinition";
            f.AddSubClass("", "FragmentBody");
            f.AddSubClass("FragmentBody", "Fragment");
            f.AddSubClass("Fragment", "Null");
            f.AddSubClass("Fragment", "Text");
            f.AddSubClass("Fragment", "Placeholder");
            f.AddSubClass("Fragment", "ContainerFragment");
            f.AddSubClass("ContainerFragment", "Profile");
            f.AddSubClass("ContainerFragment", "Segment");
            f.AddSubClass("ContainerFragment", "Block");
            f.AddSubClass("ContainerFragment", "Lookup");
            f.AddSubClass("ContainerFragment", "Condition");
            f.AddSubClass("ContainerFragment", "Function");
            f.AddSubClass("ContainerFragment", "TextBlock");
            f.Classes[1].AddInstanceProperty("Name");
            f.Classes[2].AddInstanceProperty("Name");
            f.Classes[3].AddInstanceProperty("Name");
            f.Classes[3].AddInstanceProperty("TextValue");
            f.Classes[4].AddInstanceProperty("Name");
            f.Classes[4].AddInstanceProperty("Class");
            f.Classes[4].AddInstanceProperty("Property");
            f.Classes[5].AddInstanceProperty("Name");
            f.Classes[5].AddInstanceProperty("Primary");
            f.Classes[5].AddInstanceProperty("Secondary");
            f.Classes[6].AddInstanceProperty("Name");
            f.Classes[7].AddInstanceProperty("Name");
            f.Classes[7].AddInstanceProperty("Class");
            f.Classes[7].AddInstanceProperty("Cardinality");
            f.Classes[8].AddInstanceProperty("Name");
            f.Classes[9].AddInstanceProperty("Name");
            f.Classes[9].AddInstanceProperty("NoMatch");
            f.Classes[9].AddInstanceProperty("Class1");
            f.Classes[9].AddInstanceProperty("Property1");
            f.Classes[9].AddInstanceProperty("Class2");
            f.Classes[9].AddInstanceProperty("Property2");
            f.Classes[10].AddInstanceProperty("Name");
            f.Classes[10].AddInstanceProperty("Class1");
            f.Classes[10].AddInstanceProperty("Property1");
            f.Classes[10].AddInstanceProperty("Comparison");
            f.Classes[10].AddInstanceProperty("Class2");
            f.Classes[10].AddInstanceProperty("Property2");
            f.Classes[10].AddInstanceProperty("Lit");
            f.Classes[10].AddInstanceProperty("UseLit");
            f.Classes[11].AddInstanceProperty("Name");
            f.Classes[11].AddInstanceProperty("FunctionName");
            f.Classes[12].AddInstanceProperty("Name");
            return f;
        }

        public GenNamedApplicationList<FragmentBody> FragmentBodyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            FragmentBodyList = new GenNamedApplicationList<FragmentBody>(this, 1, 0);
        }

        public FragmentBody AddFragmentBody(string name)
        {
            var item = new FragmentBody(GenData)
                           {
                               GenObject = GenData.CreateObject("", "FragmentBody"),
                               Name = name
                           };
            FragmentBodyList.Add(item);
            return item;
        }
    }
}
