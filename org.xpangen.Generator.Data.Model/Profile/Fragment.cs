// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// The root class of all profile fragments
    /// </summary>
    public class Fragment : GenNamedApplicationBase
    {
        public Fragment()
        {
        }

        public Fragment(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Generated name of the fragment
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

        /// <summary>
        /// The type of the fragment
        /// </summary>
        public string FragmentType
        {
            get { return AsString("FragmentType"); }
            set
            {
                if (FragmentType == value) return;
                SetString("FragmentType", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<BodyFragment> BodyFragmentList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            BodyFragmentList = new GenNamedApplicationList<BodyFragment>(this);
        }

        public BodyFragment AddBodyFragment(string name)
        {
            var item = new BodyFragment(GenData)
                           {
                               GenObject = GenData.CreateObject("Fragment", "BodyFragment"),
                               Name = name
                           };
            BodyFragmentList.Add(item);
            return item;
        }
    }
}
