// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Basic
{
    /// <summary>
    /// 
    /// </summary>
    public class Property : GenApplicationBase
    {
        public Property(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// Property name: must be well formed
        /// </summary>
        public string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                SaveFields();
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
                SaveFields();
            }
        }

    }
}
