// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// A fragment that forms part of the body of a container fragment
    /// </summary>
    public class FragmentBody : GenNamedApplicationBase
    {
        public FragmentBody()
        {
        }

        public FragmentBody(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The name of the body fragment
        /// </summary>
        public override string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Fragment> FragmentList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            FragmentList = new GenNamedApplicationList<Fragment>(this);
        }
        public Null AddNull(string name)
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Null"
            };

            var item = new Null(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Null"),
                               Name = name
                           };
            FragmentList.Add(item);
            return item;
        }
        public Text AddText(string name, string textValue = "")
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Text"
            };

            var item = new Text(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Text"),
                               Name = name,
                               TextValue = textValue
                           };
            FragmentList.Add(item);
            return item;
        }
        public Placeholder AddPlaceholder(string name, string @class = "", string property = "")
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Placeholder"
            };

            var item = new Placeholder(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Placeholder"),
                               Name = name,
                               Class = @class,
                               Property = property
                           };
            FragmentList.Add(item);
            return item;
        }
        public Body AddBody(string name)
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Body"
            };

            var item = new Body(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Body"),
                               Name = name
                           };
            FragmentList.Add(item);
            return item;
        }
        public Segment AddSegment(string name, string @class = "", string cardinality = "")
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Segment"
            };

            var item = new Segment(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Segment"),
                               Name = name,
                               Class = @class,
                               Cardinality = cardinality
                           };
            FragmentList.Add(item);
            return item;
        }
        public Block AddBlock(string name)
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Block"
            };

            var item = new Block(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Block"),
                               Name = name
                           };
            FragmentList.Add(item);
            return item;
        }
        public Lookup AddLookup(string name, string noMatch = "", string class1 = "", string property1 = "", string class2 = "", string property2 = "")
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Lookup"
            };

            var item = new Lookup(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Lookup"),
                               Name = name,
                               NoMatch = noMatch,
                               Class1 = class1,
                               Property1 = property1,
                               Class2 = class2,
                               Property2 = property2
                           };
            FragmentList.Add(item);
            return item;
        }
        public Condition AddCondition(string name, string class1 = "", string property1 = "", string class2 = "", string property2 = "", string lit = "", string useLit = "")
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Condition"
            };

            var item = new Condition(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Condition"),
                               Name = name,
                               Class1 = class1,
                               Property1 = property1,
                               Class2 = class2,
                               Property2 = property2,
                               Lit = lit,
                               UseLit = useLit
                           };
            FragmentList.Add(item);
            return item;
        }
        public Function AddFunction(string name, string functionName = "")
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "Function"
            };

            var item = new Function(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Function"),
                               Name = name,
                               FunctionName = functionName
                           };
            FragmentList.Add(item);
            return item;
        }
        public TextBlock AddTextBlock(string name)
        {
            new Fragment(GenData)
            {
                GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                Name = name,
                FragmentType = "TextBlock"
            };

            var item = new TextBlock(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "TextBlock"),
                               Name = name
                           };
            FragmentList.Add(item);
            return item;
        }
        public Profile AddProfile(string name)
        {
            new Fragment(GenData)
                {
                    GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                    Name = name,
                    FragmentType = "Profile"
                };

            var item = new Profile(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "Profile"),
                               Name = name
                           };
            FragmentList.Add(item);
            return item;
        }
    }
}
