// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Minimal
{
    /// <summary>
    /// 
    /// </summary>
    public class SubClass : GenApplicationBase
    {
        public SubClass(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// Subclass name: refers to a class and is used for hierarchical browsing
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

    }
}
