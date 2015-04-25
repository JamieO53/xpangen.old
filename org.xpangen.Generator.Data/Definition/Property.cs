// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Definition
{
    /// <summary>
    /// Property definition
    /// </summary>
    public class Property : GenNamedApplicationBase
    {
        public Property()
        {
            Properties.Add("Name");
            Properties.Add("Title");
            Properties.Add("DataType");
            Properties.Add("Default");
            Properties.Add("LookupType");
            Properties.Add("LookupDependence");
            Properties.Add("LookupTable");
        }

        public Property(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// Property description: used as a hint when editing
        /// </summary>
        public string Title
        {
            get { return AsString("Title"); }
            set
            {
                if (Title == value) return;
                SetString("Title", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Data type of property (used for editing) - String, integer or boolean
        /// </summary>
        public string DataType
        {
            get { return AsString("DataType"); }
            set
            {
                if (DataType == value) return;
                SetString("DataType", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Default value of the property when a new object is created (used for editing)
        /// </summary>
        public string Default
        {
            get { return AsString("Default"); }
            set
            {
                if (Default == value) return;
                SetString("Default", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// A standard lookup, a lookup to this data or the root of referenced data
        /// </summary>
        public string LookupType
        {
            get { return AsString("LookupType"); }
            set
            {
                if (LookupType == value) return;
                SetString("LookupType", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The lookup on which this one depends
        /// </summary>
        public string LookupDependence
        {
            get { return AsString("LookupDependence"); }
            set
            {
                if (LookupDependence == value) return;
                SetString("LookupDependence", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The lookup table used for the property's values
        /// </summary>
        public string LookupTable
        {
            get { return AsString("LookupTable"); }
            set
            {
                if (LookupTable == value) return;
                SetString("LookupTable", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
