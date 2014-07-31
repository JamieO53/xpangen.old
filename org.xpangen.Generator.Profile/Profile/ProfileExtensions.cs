// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Profile.Profile
{
    public static class ProfileExtensions
    {
        public static Profile AddProfile(this FragmentBody body)
        {
            var name = CreateContainerFragmentBody(body, "Profile");
            var profile = new Profile(body.GenData)
                          {
                              GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                              Name = name,
                              Primary = name,
                              Secondary = "Empty1"
                          };
            body.FragmentList.Add(profile);
            return profile;
        }
        public static Segment AddSegment(this FragmentBody body, string @class = "", string cardinality = "")
        {
            var name = CreateContainerFragmentBody(body, "Segment");
            var segment = new Segment(body.GenData)
                              {
                                  GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                  Name = name,
                                  Primary = name,
                                  Secondary = "Empty1",
                                  Class = @class,
                                  Cardinality = cardinality
                              };
            body.FragmentList.Add(segment);
            return segment;
        }

        public static Block AddBlock(this FragmentBody body)
        {
            var name = CreateContainerFragmentBody(body, "Block");
            var block = new Block(body.GenData)
                            {
                                GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                Name = name,
                                Primary = name,
                                Secondary = "Empty1"
                            };
            body.FragmentList.Add(block);
            return block;
        }

        public static Condition AddCondition(this FragmentBody body, string class1 = "", string property1 = "", 
            string comparison = "", string class2 = "", string property2 = "", string lit = "", string useLit = "")
        {
            var name = CreateContainerFragmentBody(body, "Condition");
            var condition = new Condition(body.GenData)
                                {
                                    GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                    Name = name,
                                    Primary = name,
                                    Secondary = "Empty1",
                                    Class1 = class1,
                                    Property1 = property1,
                                    Comparison = comparison,
                                    Class2 = class2,
                                    Property2 = property2,
                                    Lit = lit,
                                    UseLit = useLit
                                };
            body.FragmentList.Add(condition);
            return condition;
        }

        public static Function AddFunction(this FragmentBody body, string functionName = "")
        {
            var name = CreateContainerFragmentBody(body, "Function");
            var function = new Function(body.GenData)
                               {
                                   GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                   Name = name,
                                   Primary = name,
                                   Secondary = "Empty1",
                                   FunctionName = functionName
                               };
            body.FragmentList.Add(function);
            return function;
        }

        public static Lookup AddLookup(this FragmentBody body, string noMatch = "", string class1 = "", 
            string property1 = "", string class2 = "", string property2 = "")
        {
            var name = CreateContainerFragmentBody(body, "Lookup");
            var lookup = new Lookup(body.GenData)
                             {
                                 GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                 Name = name,
                                 Primary = name,
                                 Secondary = "Empty1",
                                 NoMatch = noMatch,
                                 Class1 = class1,
                                 Property1 = property1,
                                 Class2 = class2,
                                 Property2 = property2
                             };
            body.FragmentList.Add(lookup);
            return lookup;
        }

        public static TextBlock AddTextBlock(this FragmentBody body)
        {
            var name = CreateContainerFragmentBody(body, "TextBlock");
            var textBlock = new TextBlock(body.GenData)
                                {
                                    GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                    Name = name,
                                    Primary = name,
                                    Secondary = "Empty1"
                                };
            body.FragmentList.Add(textBlock);
            return textBlock;
        }

        public static FragmentBody Body(this ContainerFragment containerFragment)
        {
            return containerFragment.ProfileDefinition().ProfileRoot().FragmentBodyList.Find(containerFragment.Primary);
        }

        public static FragmentBody SecondaryBody(this ContainerFragment containerFragment)
        {
            return containerFragment.ProfileDefinition().ProfileRoot().FragmentBodyList.Find(containerFragment.Secondary);
        }

        public static FragmentBody CheckBody(this ContainerFragment containerFragment)
        {
            var root = containerFragment.ProfileDefinition().ProfileRoot();
            if (containerFragment.Primary == "Empty1")
                containerFragment.Primary = CreateContainerFragmentBody(root, containerFragment.GetType().Name);
            return root.FragmentBodyList.Find(containerFragment.Primary);
        }

        public static FragmentBody CheckSecondaryBody(this ContainerFragment containerFragment)
        {
            var root = containerFragment.ProfileDefinition().ProfileRoot();
            if (containerFragment.Secondary == "Empty1")
                containerFragment.Secondary = CreateContainerFragmentBody(root, containerFragment.GetType().Name);
            return root.FragmentBodyList.Find(containerFragment.Secondary);
        }

        private static string CreateContainerFragmentBody(FragmentBody body, string prefix)
        {
            var root = body.ProfileDefinition().ProfileRoot();
            return CreateContainerFragmentBody(root, prefix);
        }

        private static string CreateContainerFragmentBody(ProfileRoot root, string prefix)
        {
            var name = prefix + root.FragmentBodyList.Count;
            root.AddFragmentBody(name);
            return name;
        }

        public static ProfileRoot ProfileRoot(this ProfileDefinition def)
        {
            return def.ProfileRootList[0];
        }

        public static Profile Profile(this ProfileDefinition def)
        {
            return (Profile) ProfileFragmentBody(def).FragmentList[0];
        }

        private static FragmentBody ProfileFragmentBody(ProfileDefinition def)
        {
            return def.ProfileRoot().FragmentBodyList[0];
        }

        public static Profile Profile(this Fragment fragment)
        {
            return FragmentBody(fragment).Profile();
        }

        public static Profile Profile(this FragmentBody body)
        {
            return ProfileDefinition(body).Profile();
        }

        public static ProfileDefinition ProfileDefinition(this FragmentBody body)
        {
            return (ProfileDefinition) body.Parent.Parent;
        }

        public static ProfileDefinition ProfileDefinition(this Fragment fragment)
        {
            return fragment.FragmentBody().ProfileDefinition();
        }

        public static FragmentBody FragmentBody(this Fragment fragment)
        {
            return (FragmentBody)fragment.Parent;
        }

        public static void Setup(this ProfileDefinition def)
        {
            var root = def.AddProfileRoot("");
            var rootBody = root.AddFragmentBody("Root0");
            var emptyBody = root.AddFragmentBody("Empty1");
            var profile = rootBody.AddProfile();
            var profileBody = root.AddFragmentBody("Profile2");
            profile.Primary = "Profile2";
            profile.Secondary = "Empty1";
        }
    }
}