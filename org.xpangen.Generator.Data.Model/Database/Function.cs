// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database function definition
    /// </summary>
    public class Function : GenNamedApplicationBase
    {
        public Function()
        {
        }

        public Function(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
            Properties.Add("FunctionName");
        }

        /// <summary>
        /// Function name
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
        /// The function name as on the database
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
