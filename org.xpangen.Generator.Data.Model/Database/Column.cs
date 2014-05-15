// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Table column definition
    /// </summary>
    public class Column : GenNamedApplicationBase
    {
        public Column()
        {
        }

        public Column(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// Column name
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
        /// The native database type of the column
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
        /// The ODBC data type of the column
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
        /// The length of the data column
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
        /// The precision of the data column
        /// </summary>
        public string Precison
        {
            get { return AsString("Precison"); }
            set
            {
                if (Precison == value) return;
                SetString("Precison", value);
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
        /// Is the data column nullable?
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
        /// Is this column part of the primary key
        /// </summary>
        public string IsKey
        {
            get { return AsString("IsKey"); }
            set
            {
                if (IsKey == value) return;
                SetString("IsKey", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Default> DefaultList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            DefaultList = new GenNamedApplicationList<Default>(this);
        }

        public Default AddDefault(string name, string value = "")
        {
            var item = new Default(GenData)
                           {
                               GenObject = GenData.CreateObject("Column", "Default"),
                               Name = name,
                               Value = value
                           };
            DefaultList.Add(item);
            return item;
        }
    }
}