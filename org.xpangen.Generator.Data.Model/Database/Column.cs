// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Table column definition
    /// </summary>
    public class Column : GenNamedApplicationBase
    {
        public Column()
        {
            SubClasses.Add("Default");
            Properties.Add("Name");
            Properties.Add("ColumnName");
            Properties.Add("NativeDataType");
            Properties.Add("ODBCDataType");
            Properties.Add("Length");
            Properties.Add("Precision");
            Properties.Add("Scale");
            Properties.Add("IsNullable");
            Properties.Add("IsKey");
            Properties.Add("IsIdentity");
        }

        public Column(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// Column name as on the database
        /// </summary>
        public string ColumnName
        {
            get { return AsString("ColumnName"); }
            set
            {
                if (ColumnName == value) return;
                SetString("ColumnName", value);
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

        /// <summary>
        /// Is this column an Identity?
        /// </summary>
        public string IsIdentity
        {
            get { return AsString("IsIdentity"); }
            set
            {
                if (IsIdentity == value) return;
                SetString("IsIdentity", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Default> DefaultList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            DefaultList = new GenNamedApplicationList<Default>(this, 9, 0);
        }

        public Default AddDefault(string name, string value = "")
        {
            var item = new Default(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("Default"),
                               Name = name,
                               Value = value
                           };
            DefaultList.Add(item);
            return item;
        }

    }
}
