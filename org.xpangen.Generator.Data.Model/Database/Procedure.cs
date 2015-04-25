// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database procedure
    /// </summary>
    public class Procedure : GenNamedApplicationBase
    {
        public Procedure()
        {
            Properties.Add("Name");
            Properties.Add("ProcedureName");
        }

        public Procedure(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// Procedure name as on the database
        /// </summary>
        public string ProcedureName
        {
            get { return AsString("ProcedureName"); }
            set
            {
                if (ProcedureName == value) return;
                SetString("ProcedureName", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
