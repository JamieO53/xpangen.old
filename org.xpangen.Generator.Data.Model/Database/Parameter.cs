// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class Parameter : GenNamedApplicationBase
    {
        public Parameter()
        {
        }

        public Parameter(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Parameter name
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
        /// Parameter name as on the database
        /// </summary>
        public string ParameterName
        {
            get { return AsString("ParameterName"); }
            set
            {
                if (ParameterName == value) return;
                SetString("ParameterName", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The native database type of the parameter
        /// </summary>
        public string NativeDataType
        {
            get { return AsString("NativeDataType"); }
            set
            {
                if (NativeDataType == value) return;
                SetString("NativeDataType", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The ODBC data type of the parameter
        /// </summary>
        public string ODBCDataType
        {
            get { return AsString("ODBCDataType"); }
            set
            {
                if (ODBCDataType == value) return;
                SetString("ODBCDataType", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The length of the data parameter
        /// </summary>
        public string Length
        {
            get { return AsString("Length"); }
            set
            {
                if (Length == value) return;
                SetString("Length", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The precision of the data parameter
        /// </summary>
        public string Precision
        {
            get { return AsString("Precision"); }
            set
            {
                if (Precision == value) return;
                SetString("Precision", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Numeric data scale
        /// </summary>
        public string Scale
        {
            get { return AsString("Scale"); }
            set
            {
                if (Scale == value) return;
                SetString("Scale", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Is the parameter nullable?
        /// </summary>
        public string IsNullable
        {
            get { return AsString("IsNullable"); }
            set
            {
                if (IsNullable == value) return;
                SetString("IsNullable", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The direction of the parameter data
        /// </summary>
        public string Direction
        {
            get { return AsString("Direction"); }
            set
            {
                if (Direction == value) return;
                SetString("Direction", value);
                if (!DelayedSave) SaveFields();
            }
        }


    }
}
