// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Definition
{
    /// <summary>
    /// Property definition
    /// </summary>
    public class Property : GenNamedApplicationBase
    {
        public Property()
        {
        }

        public Property(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Property name: must be well formed
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

    }
}
