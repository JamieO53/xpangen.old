// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Database procedure
    /// </summary>
    public class Procedure : GenNamedApplicationBase
    {
        public Procedure()
        {
        }

        public Procedure(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
            Properties.Add("ProcedureName");
        }

        /// <summary>
        /// Procedure name
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
