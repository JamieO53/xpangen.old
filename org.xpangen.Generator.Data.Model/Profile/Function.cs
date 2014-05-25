// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Profile
{
    /// <summary>
    /// The Function fragment data
    /// </summary>
    public class Function : GenNamedApplicationBase
    {
        public Function()
        {
        }

        public Function(GenData genData)
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
        /// The name of the function
        /// </summary>
        public string FunctionName
        {
            get { return AsString("FunctionName"); }
            set
            {
                if (FunctionName == value) return;
                SetString("FunctionName", value);
                if (!DelayedSave) SaveFields();
            }
        }

    }
}
