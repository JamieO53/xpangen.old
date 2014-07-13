// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.NewProfile
{
    /// <summary>
    /// A fragment that forms part of the body of a container fragment
    /// </summary>
    public class FragmentBody : Fragment
    {
        public FragmentBody()
        {
        }

        public FragmentBody(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
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
            base.GenObjectSetNotification();
            FragmentList = new GenNamedApplicationList<Fragment>(this);
        }
        public Null AddNull(string name)
        {
            var item = new Null(GenData)
                           {
                               GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                               Name = name
                           };
            FragmentList.Add(item);
            return item;
        }
        public Text AddText(string name, string textValue = "")
        {
            var item = new Text(GenData)
                           {
                               GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                               Name = name,
                               TextValue = textValue
                           };
            FragmentList.Add(item);
            return item;
        }
        public Placeholder AddPlaceholder(string name, string @class = "", string property = "")
        {
            var item = new Placeholder(GenData)
                           {
                               GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                               Name = name,
                               Class = @class,
                               Property = property
                           };
            FragmentList.Add(item);
            return item;
        }
        public ContainerFragment AddContainerFragment(string name)
        {
            var item = new ContainerFragment(GenData)
                           {
                               GenObject = GenData.CreateObject("FragmentBody", "Fragment"),
                               Name = name
                           };
            FragmentList.Add(item);
            return item;
        }
    }
}
