// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Minimal
{
    /// <summary>
    /// 
    /// </summary>
    public class SubClass : GenNamedApplicationBase
    {
        public SubClass(GenData genData) : base(genData)
        {
        }

        /// <summary>
        /// Subclass name: refers to a class and is used for hierarchical browsing
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

    }
}
