// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database function definition
    /// </summary>
    public class Function : GenNamedApplicationBase
    {
        public Function()
        {
            Properties.Add("Name");
            Properties.Add("FunctionName");
        }

        public Function(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
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
