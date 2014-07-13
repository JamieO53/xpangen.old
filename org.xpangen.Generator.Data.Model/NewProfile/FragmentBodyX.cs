// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.NewProfile
{
    public static class FragmentBodyX
    {
        public static Segment AddSegment(this FragmentBody body, string @class, string cardinality)
        {
            var name = CreateContainerFragmentBody(body);
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
            var name = CreateContainerFragmentBody(body);
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
            var name = CreateContainerFragmentBody(body);
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
            var name = CreateContainerFragmentBody(body);
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
            var name = CreateContainerFragmentBody(body);
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
            var name = CreateContainerFragmentBody(body);
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
        
        private static string CreateContainerFragmentBody(FragmentBody body)
        {
            var profile = Profile(body);
            var name = "Segment" + profile.FragmentBodyList.Count;
            profile.AddFragmentBody(name);
            return name;
        }

        private static Profile Profile(FragmentBody body)
        {
            return (Profile) body.Parent;
        }
    }
}