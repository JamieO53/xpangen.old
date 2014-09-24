// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Database
{
    /// <summary>
    /// The foreign key definition
    /// </summary>
    public class ForeignKey : GenNamedApplicationBase
    {
        public ForeignKey()
        {
            SubClasses.Add("ForeignKeyColumn");
            Properties.Add("Name");
            Properties.Add("ReferenceSchema");
            Properties.Add("ReferenceTable");
            Properties.Add("DeleteAction");
            Properties.Add("UpdateAction");
        }

        public ForeignKey(GenDataBase genDataBase) : this()
        {
            GenDataBase = genDataBase;
        }

        /// <summary>
        /// The name of the foreign key constraint
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
        /// The schema of the related table
        /// </summary>
        public string ReferenceSchema
        {
            get { return AsString("ReferenceSchema"); }
            set
            {
                if (ReferenceSchema == value) return;
                SetString("ReferenceSchema", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The table referred to
        /// </summary>
        public string ReferenceTable
        {
            get { return AsString("ReferenceTable"); }
            set
            {
                if (ReferenceTable == value) return;
                SetString("ReferenceTable", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The action to be taken when the referred row is deleted
        /// </summary>
        public string DeleteAction
        {
            get { return AsString("DeleteAction"); }
            set
            {
                if (DeleteAction == value) return;
                SetString("DeleteAction", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The action to be taken when the referred key is updated
        /// </summary>
        public string UpdateAction
        {
            get { return AsString("UpdateAction"); }
            set
            {
                if (UpdateAction == value) return;
                SetString("UpdateAction", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<ForeignKeyColumn> ForeignKeyColumnList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            ForeignKeyColumnList = new GenNamedApplicationList<ForeignKeyColumn>(this, 14, 0);
        }

        public ForeignKeyColumn AddForeignKeyColumn(string name, string relatedColumn = "")
        {
            var item = new ForeignKeyColumn(GenDataBase)
                           {
                               GenObject = ((GenObject) GenObject).CreateGenObject("ForeignKeyColumn"),
                               Name = name,
                               RelatedColumn = relatedColumn
                           };
            ForeignKeyColumnList.Add(item);
            return item;
        }

    }
}
