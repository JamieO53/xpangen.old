// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;
namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// A fragment that forms part of the body of a container fragment
    /// </summary>
    public class BodyFragment : GenNamedApplicationBase
    {
        public BodyFragment()
        {
        }

        public BodyFragment(GenData genData)
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

        public GenNamedApplicationList<Null> NullList { get; private set; }
        public GenNamedApplicationList<Text> TextList { get; private set; }
        public GenNamedApplicationList<Placeholder> PlaceholderList { get; private set; }
        public GenNamedApplicationList<Body> BodyList { get; private set; }
        public GenNamedApplicationList<Segment> SegmentList { get; private set; }
        public GenNamedApplicationList<Block> BlockList { get; private set; }
        public GenNamedApplicationList<Lookup> LookupList { get; private set; }
        public GenNamedApplicationList<Condition> ConditionList { get; private set; }
        public GenNamedApplicationList<Function> FunctionList { get; private set; }
        public GenNamedApplicationList<TextBlock> TextBlockList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            NullList = new GenNamedApplicationList<Null>(this);
            TextList = new GenNamedApplicationList<Text>(this);
            PlaceholderList = new GenNamedApplicationList<Placeholder>(this);
            BodyList = new GenNamedApplicationList<Body>(this);
            SegmentList = new GenNamedApplicationList<Segment>(this);
            BlockList = new GenNamedApplicationList<Block>(this);
            LookupList = new GenNamedApplicationList<Lookup>(this);
            ConditionList = new GenNamedApplicationList<Condition>(this);
            FunctionList = new GenNamedApplicationList<Function>(this);
            TextBlockList = new GenNamedApplicationList<TextBlock>(this);
        }

        public Null AddNull(string name)
        {
            var item = new Null(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Null"),
                               Name = name
                           };
            NullList.Add(item);
            return item;
        }


        public Text AddText(string name, string textValue = "")
        {
            var item = new Text(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Text"),
                               Name = name,
                               TextValue = textValue
                           };
            TextList.Add(item);
            return item;
        }


        public Placeholder AddPlaceholder(string name, string @class = "", string property = "")
        {
            var item = new Placeholder(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Placeholder"),
                               Name = name,
                               Class = @class,
                               Property = property
                           };
            PlaceholderList.Add(item);
            return item;
        }


        public Body AddBody(string name)
        {
            var item = new Body(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Body"),
                               Name = name
                           };
            BodyList.Add(item);
            return item;
        }


        public Segment AddSegment(string name, string @class = "", string cardinality = "")
        {
            var item = new Segment(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Segment"),
                               Name = name,
                               Class = @class,
                               Cardinality = cardinality
                           };
            SegmentList.Add(item);
            return item;
        }


        public Block AddBlock(string name)
        {
            var item = new Block(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Block"),
                               Name = name
                           };
            BlockList.Add(item);
            return item;
        }


        public Lookup AddLookup(string name, string noMatch = "", string class1 = "", string property1 = "", string class2 = "", string property2 = "")
        {
            var item = new Lookup(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Lookup"),
                               Name = name,
                               NoMatch = noMatch,
                               Class1 = class1,
                               Property1 = property1,
                               Class2 = class2,
                               Property2 = property2
                           };
            LookupList.Add(item);
            return item;
        }


        public Condition AddCondition(string name, string class1 = "", string property1 = "", string class2 = "", string property2 = "", string lit = "", string useLit = "")
        {
            var item = new Condition(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Condition"),
                               Name = name,
                               Class1 = class1,
                               Property1 = property1,
                               Class2 = class2,
                               Property2 = property2,
                               Lit = lit,
                               UseLit = useLit
                           };
            ConditionList.Add(item);
            return item;
        }


        public Function AddFunction(string name, string functionName = "")
        {
            var item = new Function(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "Function"),
                               Name = name,
                               FunctionName = functionName
                           };
            FunctionList.Add(item);
            return item;
        }


        public TextBlock AddTextBlock(string name)
        {
            var item = new TextBlock(GenData)
                           {
                               GenObject = GenData.CreateObject("BodyFragment", "TextBlock"),
                               Name = name
                           };
            TextBlockList.Add(item);
            return item;
        }
    }
}
