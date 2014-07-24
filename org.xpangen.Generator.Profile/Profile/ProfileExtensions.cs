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
                              Name = name
                          };
            body.FragmentList.Add(profile);
            return profile;
        }
        public static Segment AddSegment(this FragmentBody body, string @class, string cardinality)
        {
            var name = CreateContainerFragmentBody(body, "Segment");
            var segment = new Segment(body.GenData)
                              {
                                  GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                  Name = name,
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
                                Name = name
                            };
            body.FragmentList.Add(block);
            return block;
        }

        public static Condition AddCondition(this FragmentBody body, string class1, string property1, string comparison,
                                             string class2, string property2, string lit, string useLit)
        {
            var name = CreateContainerFragmentBody(body, "Condition");
            var condition = new Condition(body.GenData)
                                {
                                    GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                    Name = name,
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

        public static Function AddFunction(this FragmentBody body, string functionName)
        {
            var name = CreateContainerFragmentBody(body, "Function");
            var function = new Function(body.GenData)
                               {
                                   GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                   Name = name,
                                   FunctionName = functionName
                               };
            body.FragmentList.Add(function);
            return function;
        }

        public static Lookup AddLookup(this FragmentBody body, string noMatch, string class1, string property1, string class2, string property2)
        {
            var name = CreateContainerFragmentBody(body, "Lookup");
            var lookup = new Lookup(body.GenData)
                             {
                                 GenObject = body.GenData.CreateObject("FragmentBody", "Fragment"),
                                 Name = name,
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
                                    Name = name
                                };
            body.FragmentList.Add(textBlock);
            return textBlock;
        }

        public static FragmentBody Body(this ContainerFragment fragment)
        {
            var name = fragment.Name;
            var profile = (Profile)fragment.Parent.Parent;
            return profile.FragmentBodyList.Find(name);
        }
        
        private static string CreateContainerFragmentBody(FragmentBody body, string prefix)
        {
            var profile = Profile(body);
            var name = prefix + profile.FragmentBodyList.Count;
            profile.AddFragmentBody(name);
            return name;
        }

        public static Profile Profile(this ProfileDefinition def)
        {
            return (Profile) def.ProfileRootList[0].FragmentBodyList[0].FragmentList[0];
        }

        public static Profile Profile(this FragmentBody body)
        {
            return (Profile) body.Parent;
        }

        public static FragmentBody Body(this Profile profile)
        {
            return profile.FragmentBodyList[0];
        }
    }
}