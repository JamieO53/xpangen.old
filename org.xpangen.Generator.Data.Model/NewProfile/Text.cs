// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.NewProfile
{
    /// <summary>
    /// The Text fragment data
    /// </summary>
    public class Text : Fragment
    {
        public Text()
        {
            Properties.Add("Name");
            Properties.Add("TextValue");
        }

        public Text(GenData genData) : this()
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
        /// The plain text being generated
        /// </summary>
        public string TextValue
        {
            get { return AsString("TextValue"); }
            set
            {
                if (TextValue == value) return;
                SetString("TextValue", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}