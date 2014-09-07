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
                              GenObject = body.GenData.CreateObject("FragmentBody", "Profile"),
                              Name = name,
                              Primary = name,
                              Secondary = "Empty1"
                          };
            body.FragmentList.Add(profile);
            SetContainerLinks(body, profile);
            return profile;
        }

        public static Segment AddSegment(this FragmentBody body, string @class = "", string cardinality = "")
        {
            var name = CreateContainerFragmentBody(body, "Segment");
            var segment = new Segment(body.GenData)
                              {
                                  GenObject = body.GenData.CreateObject("FragmentBody", "Segment"),
                                  Name = name,
                                  Primary = name,
                                  Secondary = "Empty1",
                                  Class = @class,
                                  Cardinality = cardinality
                              };
            body.FragmentList.Add(segment);
            SetContainerLinks(body, segment);
            return segment;
        }

        public static Block AddBlock(this FragmentBody body)
        {
            var name = CreateContainerFragmentBody(body, "Block");
            var block = new Block(body.GenData)
                            {
                                GenObject = body.GenData.CreateObject("FragmentBody", "Block"),
                                Name = name,
                                Primary = name,
                                Secondary = "Empty1"
                            };
            body.FragmentList.Add(block);
            SetContainerLinks(body, block);
            return block;
        }

        public static Condition AddCondition(this FragmentBody body, string class1 = "", string property1 = "", 
            string comparison = "", string class2 = "", string property2 = "", string lit = "", string useLit = "")
        {
            var name = CreateContainerFragmentBody(body, "Condition");
            var condition = new Condition(body.GenData)
                                {
                                    GenObject = body.GenData.CreateObject("FragmentBody", "Condition"),
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
            SetContainerLinks(body, condition);
            return condition;
        }

        public static Function AddFunction(this FragmentBody body, string functionName = "")
        {
            var name = "Function" + body.FragmentList.Count;
            var function = new Function(body.GenData)
                               {
                                   GenObject = body.GenData.CreateObject("FragmentBody", "Function"),
                                   Name = name,
                                   Primary = "Empty1",
                                   Secondary = "Empty1",
                                   FunctionName = functionName
                               };
            body.FragmentList.Add(function);
            SetContainerLinks(body, function);
            return function;
        }

        public static Lookup AddLookup(this FragmentBody body, string noMatch = "", string class1 = "", 
            string property1 = "", string class2 = "", string property2 = "")
        {
            var name = CreateContainerFragmentBody(body, "Lookup");
            var lookup = new Lookup(body.GenData)
                             {
                                 GenObject = body.GenData.CreateObject("FragmentBody", "Lookup"),
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
            SetContainerLinks(body, lookup);
            return lookup;
        }

        public static TextBlock AddTextBlock(this FragmentBody body)
        {
            var name = CreateContainerFragmentBody(body, "TextBlock");
            var textBlock = new TextBlock(body.GenData)
                                {
                                    GenObject = body.GenData.CreateObject("FragmentBody", "TextBlock"),
                                    Name = name,
                                    Primary = name,
                                    Secondary = "Empty1"
                                };
            body.FragmentList.Add(textBlock);
            SetContainerLinks(body, textBlock);
            return textBlock;
        }

        private static void SetContainerLinks(FragmentBody body, ContainerFragment containerFragment)
        {
            containerFragment.Links.Add("ContainerBody", body);
            containerFragment.Links.Add("GenObject", null);
            var bodies = containerFragment.ProfileDefinition().ProfileRoot().FragmentBodyList;
            containerFragment.Links.Add("PrimaryBody", bodies.Find(containerFragment.Primary));
            containerFragment.Links.Add("SecondaryBody", bodies.Find(containerFragment.Secondary));
            if (containerFragment.Primary != "Empty1")
                containerFragment.Body().Links.Add("Parent", containerFragment);
        }

        public static FragmentBody Body(this ContainerFragment containerFragment)
        {
            //return containerFragment.ProfileDefinition().ProfileRoot().FragmentBodyList.Find(containerFragment.Primary);
            return (FragmentBody) containerFragment.Links["PrimaryBody"];
        }

        public static FragmentBody SecondaryBody(this ContainerFragment containerFragment)
        {
            //return containerFragment.ProfileDefinition().ProfileRoot().FragmentBodyList.Find(containerFragment.Secondary);
            return (FragmentBody) containerFragment.Links["SecondaryBody"];
        }

        public static FragmentBody CheckBody(this ContainerFragment containerFragment)
        {
            var root = containerFragment.ProfileDefinition().ProfileRoot();
            if (containerFragment.Primary == "Empty1")
            {
                containerFragment.Primary = CreateContainerFragmentBody(root, containerFragment.GetType().Name);
                containerFragment.Links["PrimaryBody"] =
                    containerFragment.ProfileDefinition().ProfileRoot().FragmentBodyList.Find(containerFragment.Primary);
                containerFragment.Body().Links.Add("Parent", containerFragment);
            }
            return root.FragmentBodyList.Find(containerFragment.Primary);
        }

        public static FragmentBody CheckSecondaryBody(this ContainerFragment containerFragment)
        {
            var root = containerFragment.ProfileDefinition().ProfileRoot();
            if (containerFragment.Secondary == "Empty1")
            {
                containerFragment.Secondary = CreateContainerFragmentBody(root, containerFragment.GetType().Name);
                containerFragment.Links["SecondaryBody"] =
                    containerFragment.ProfileDefinition().ProfileRoot().FragmentBodyList.Find(containerFragment.Secondary);
                containerFragment.SecondaryBody().Links.Add("Parent", containerFragment);
            }
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

        public static string FragmentName(this FragmentBody body, FragmentType fragmentType)
        {
            return fragmentType.ToString() + body.FragmentList.Count;
        }
        
        public static void Setup(this ProfileDefinition def)
        {
            var root = def.AddProfileRoot("");
            var rootBody = root.AddFragmentBody("Root0");
            root.AddFragmentBody("Empty1");
            var profile = rootBody.AddProfile();
            profile.Primary = "Profile2";
            profile.Secondary = "Empty1";
        }
    }
}