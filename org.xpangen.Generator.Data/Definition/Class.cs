// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Definition
{
    /// <summary>
    /// Class Definition
    /// </summary>
    public class Class : GenNamedApplicationBase
    {
        public Class()
        {
        }

        public Class(GenData genData)
        {
            GenData = genData;
            Properties.Add("Name");
            Properties.Add("Title");
            Properties.Add("Inheritance");
        }

        /// <summary>
        /// Class name: must be well formed
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
        /// Class description: used as a hint when editing
        /// </summary>
        public string Title
        {
            get { return AsString("Title"); }
            set
            {
                if (Title == value) return;
                SetString("Title", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// What kind of inheritance do extended subclasses have
        /// </summary>
        public string Inheritance
        {
            get { return AsString("Inheritance"); }
            set
            {
                if (Inheritance == value) return;
                SetString("Inheritance", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<SubClass> SubClassList { get; private set; }
        public GenNamedApplicationList<Property> PropertyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            SubClassList = new GenNamedApplicationList<SubClass>(this, 2, 0);
            base.GenObjectSetNotification();
            PropertyList = new GenNamedApplicationList<Property>(this, 3, 1);
        }

        public SubClass AddSubClass(string name, string reference = "", string relationship = "")
        {
            var item = new SubClass(GenData)
                           {
                               GenObject = GenData.CreateObject("Class", "SubClass"),
                               Name = name,
                               Reference = reference,
                               Relationship = relationship
                           };
            SubClassList.Add(item);
            return item;
        }


        public Property AddProperty(string name, string title = "", string dataType = "String", string @default = "", string lookupType = "", string lookupDependence = "", string lookupTable = "")
        {
            var item = new Property(GenData)
                           {
                               GenObject = GenData.CreateObject("Class", "Property"),
                               Name = name,
                               Title = title,
                               DataType = dataType,
                               Default = @default,
                               LookupType = lookupType,
                               LookupDependence = lookupDependence,
                               LookupTable = lookupTable
                           };
            PropertyList.Add(item);
            return item;
        }

    }
}
