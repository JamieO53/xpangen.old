// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// View definition
    /// </summary>
    public class View : GenNamedApplicationBase
    {
        public View()
        {
            Properties.Add("Name");
            Properties.Add("ViewName");
        }

        public View(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
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
