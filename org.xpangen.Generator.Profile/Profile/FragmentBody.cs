// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// A fragment that forms part of the body of a container fragment
    /// </summary>
    public class FragmentBody : GenNamedApplicationBase
    {
        public FragmentBody()
        {
            SubClasses.Add("Fragment");
            Properties.Add("Name");
        }

        public FragmentBody(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        public GenNamedApplicationList<Fragment> FragmentList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            FragmentList = new GenNamedApplicationList<Fragment>(this, 3, 0);
        }

        public string FragmentName(FragmentType fragmentType)
        {
            return fragmentType.ToString() + FragmentList.Count;
        }

        internal ProfileDefinition ProfileDefinition
        {
            get { return (ProfileDefinition) Parent.Parent; }
        }

        private void SetContainerLinks(ContainerFragment containerFragment)
        {
            containerFragment.Links.Add("ContainerBody", this);
            containerFragment.Links.Add("GenObject", null);
            var bodies = containerFragment.ProfileDefinition.ProfileRoot().FragmentBodyList;
            containerFragment.Links.Add("PrimaryBody", bodies.Find(containerFragment.Primary));
            containerFragment.Links.Add("SecondaryBody", bodies.Find(containerFragment.Secondary));
            if (containerFragment.Primary != "Empty1")
                containerFragment.Body().Links.Add("Parent", containerFragment);
        }

        private string CreateContainerFragmentBody(string prefix)
        {
            var root = ProfileDefinition.ProfileRoot();
            return CreateContainerFragmentBody(root, prefix);
        }

        private static string CreateContainerFragmentBody(ProfileRoot root, string prefix)
        {
            var name = prefix + root.FragmentBodyList.Count;
            root.AddFragmentBody(name);
            return name;
        }

        public Text AddText(string name, string textValue = "")
        {
            var item = new Text(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("Text"),
                               Name = name,
                               TextValue = textValue
                           };
            FragmentList.Add(item);
            return item;
        }
        public Placeholder AddPlaceholder(string name, string @class = "", string property = "")
        {
            var item = new Placeholder(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("Placeholder"),
                               Name = name,
                               Class = @class,
                               Property = property
                           };
            FragmentList.Add(item);
            return item;
        }
        public ContainerFragment AddContainerFragment(string name, string primary = "", string secondary = "")
        {
            var item = new ContainerFragment(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("ContainerFragment"),
                               Name = name,
                               Primary = primary,
                               Secondary = secondary
                           };
            FragmentList.Add(item);
            return item;
        }
        public Profile AddProfile()
        {
            var name = CreateContainerFragmentBody("Profile");
            var profile = new Profile(GenDataBase)
            {
                GenObject = ((GenObject) GenObject).CreateGenObject("Profile"),
                Name = name,
                Primary = name,
                Secondary = "Empty1"
            };
            FragmentList.Add(profile);
            SetContainerLinks(profile);
            return profile;
        }

        public Segment AddSegment(string @class = "", string cardinality = "")
        {
            var name = CreateContainerFragmentBody("Segment");
            var segment = new Segment(GenDataBase)
            {
                GenObject = ((GenObject) GenObject).CreateGenObject("Segment"),
                Name = name,
                Primary = name,
                Secondary = "Empty1",
                Class = @class,
                Cardinality = cardinality
            };
            FragmentList.Add(segment);
            SetContainerLinks(segment);
            return segment;
        }

        public Block AddBlock()
        {
            var name = CreateContainerFragmentBody("Block");
            var block = new Block(GenDataBase)
            {
                GenObject = ((GenObject) GenObject).CreateGenObject("Block"),
                Name = name,
                Primary = name,
                Secondary = "Empty1"
            };
            FragmentList.Add(block);
            SetContainerLinks(block);
            return block;
        }

        public Annotation AddAnnotation()
        {
            var name = CreateContainerFragmentBody("Annotation");
            var annotation = new Annotation(GenDataBase)
            {
                GenObject = ((GenObject) GenObject).CreateGenObject("Annotation"),
                Name = name,
                Primary = name,
                Secondary = "Empty1"
            };
            FragmentList.Add(annotation);
            SetContainerLinks(annotation);
            return annotation;
        }

        public Condition AddCondition(string class1 = "", string property1 = "", string comparison = "", 
            string class2 = "", string property2 = "", string lit = "", string useLit = "")
        {
            var name = CreateContainerFragmentBody("Condition");
            var condition = new Condition(GenDataBase)
            {
                GenObject = ((GenObject) GenObject).CreateGenObject("Condition"),
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
            FragmentList.Add(condition);
            SetContainerLinks(condition);
            return condition;
        }

        public Function AddFunction(string functionName = "")
        {
            var name = "Function" + FragmentList.Count;
            var function = new Function(GenDataBase)
            {
                GenObject = ((GenObject) GenObject).CreateGenObject("Function"),
                Name = name,
                Primary = "Empty1",
                Secondary = "Empty1",
                FunctionName = functionName
            };
            FragmentList.Add(function);
            SetContainerLinks(function);
            return function;
        }

        public Lookup AddLookup(string noMatch = "", string class1 = "", string property1 = "", 
            string class2 = "", string property2 = "")
        {
            var name = CreateContainerFragmentBody("Lookup");
            var lookup = new Lookup(GenDataBase)
            {
                GenObject = ((GenObject) GenObject).CreateGenObject("Lookup"),
                Name = name,
                Primary = name,
                Secondary = "Empty1",
                NoMatch = noMatch,
                Class1 = class1,
                Property1 = property1,
                Class2 = class2,
                Property2 = property2
            };
            FragmentList.Add(lookup);
            SetContainerLinks(lookup);
            return lookup;
        }

        public TextBlock AddTextBlock()
        {
            var name = CreateContainerFragmentBody("TextBlock");
            var textBlock = new TextBlock(GenDataBase)
            {
                GenObject = ((GenObject) GenObject).CreateGenObject("TextBlock"),
                Name = name,
                Primary = name,
                Secondary = "Empty1"
            };
            FragmentList.Add(textBlock);
            SetContainerLinks(textBlock);
            return textBlock;
        }
    }
}
