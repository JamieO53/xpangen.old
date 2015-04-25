// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Definition
{
    /// <summary>
    /// Class to SubClass link
    /// </summary>
    public class SubClass : GenNamedApplicationBase
    {
        public SubClass()
        {
            Properties.Add("Name");
            Properties.Add("Reference");
            Properties.Add("Relationship");
        }

        public SubClass(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// Location of the subclass
        /// </summary>
        public string Reference
        {
            get { return AsString("Reference"); }
            set
            {
                if (Reference == value) return;
                SetString("Reference", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// SubClass relationship between parent and child
        /// </summary>
        public string Relationship
        {
            get { return AsString("Relationship"); }
            set
            {
                if (Relationship == value) return;
                SetString("Relationship", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
