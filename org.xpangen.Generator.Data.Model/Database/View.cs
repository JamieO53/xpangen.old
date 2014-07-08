// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// View definition
    /// </summary>
    public class View : GenNamedApplicationBase
    {
        public View()
        {
        }

        public View(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
            Properties.Add("ViewName");
        }

        /// <summary>
        /// View name
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
        /// The view name as on the database
        /// </summary>
        public string ViewName
        {
            get { return AsString("ViewName"); }
            set
            {
                if (ViewName == value) return;
                SetString("ViewName", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
