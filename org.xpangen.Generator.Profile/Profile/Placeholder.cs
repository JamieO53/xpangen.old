// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile.Profile
{
    /// <summary>
    /// The Placeholder fragment data
    /// </summary>
    public class Placeholder : Fragment
    {
        public Placeholder()
        {
            Properties.Add("Name");
            Properties.Add("Class");
            Properties.Add("Property");
        }

        public Placeholder(GenData genData) : this()
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
        /// The class containing the value to substitute in place
        /// </summary>
        public string Class
        {
            get { return AsString("Class"); }
            set
            {
                if (Class == value) return;
                SetString("Class", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The property whose value is to be substituted in place
        /// </summary>
        public string Property
        {
            get { return AsString("Property"); }
            set
            {
                if (Property == value) return;
                SetString("Property", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
