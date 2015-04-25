// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// Table index
    /// </summary>
    public class Index : GenNamedApplicationBase
    {
        public Index()
        {
            SubClasses.Add("KeyColumn");
            SubClasses.Add("DataColumn");
            Properties.Add("Name");
            Properties.Add("IsPrimaryKey");
            Properties.Add("IsUnique");
            Properties.Add("IsClusterKey");
        }

        public Index(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// Is this the table's primary key?
        /// </summary>
        public string IsPrimaryKey
        {
            get { return AsString("IsPrimaryKey"); }
            set
            {
                if (IsPrimaryKey == value) return;
                SetString("IsPrimaryKey", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Is this a unique key?
        /// </summary>
        public string IsUnique
        {
            get { return AsString("IsUnique"); }
            set
            {
                if (IsUnique == value) return;
                SetString("IsUnique", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// Is this a cluster key?
        /// </summary>
        public string IsClusterKey
        {
            get { return AsString("IsClusterKey"); }
            set
            {
                if (IsClusterKey == value) return;
                SetString("IsClusterKey", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<KeyColumn> KeyColumnList { get; private set; }
        public GenNamedApplicationList<DataColumn> DataColumnList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            KeyColumnList = new GenNamedApplicationList<KeyColumn>(this, 11, 0);
            base.GenObjectSetNotification();
            DataColumnList = new GenNamedApplicationList<DataColumn>(this, 12, 1);
        }

        public KeyColumn AddKeyColumn(string name, string order = "")
        {
            var item = new KeyColumn(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("KeyColumn"),
                               Name = name,
                               Order = order
                           };
            KeyColumnList.Add(item);
            return item;
        }


        public DataColumn AddDataColumn(string name)
        {
            var item = new DataColumn(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("DataColumn"),
                               Name = name
                           };
            DataColumnList.Add(item);
            return item;
        }

    }
}
