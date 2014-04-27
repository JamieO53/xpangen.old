// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Codes
{
    /// <summary>
    /// The code and its description
    /// </summary>
    public class Code : GenNamedApplicationBase
    {
        public Code(GenData genData) : base(genData)
        {
        }

        /// <summary>
        /// The name of the code
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
        /// The description of the code to be used for dropdowns
        /// </summary>
        public string Description
        {
            get { return AsString("Description"); }
            set
            {
                if (Description == value) return;
                SetString("Description", value);
                if (!DelayedSave) SaveFields();
            }
        }

    }
}
