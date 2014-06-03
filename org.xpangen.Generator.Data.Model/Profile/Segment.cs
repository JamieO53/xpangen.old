// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// The Segment fragment data
    /// </summary>
    public class Segment : Fragment
    {
        public Segment()
        {
        }

        public Segment(GenData genData)
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
        /// The class of the fragment
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
        /// How the class objects are to be generated
        /// </summary>
        public string Cardinality
        {
            get { return AsString("Cardinality"); }
            set
            {
                if (Cardinality == value) return;
                SetString("Cardinality", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
