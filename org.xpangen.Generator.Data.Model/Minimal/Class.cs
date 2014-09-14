// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Minimal
{
    /// <summary>
    /// Class Definition
    /// </summary>
    public class Class : GenNamedApplicationBase
    {
        public Class()
        {
            SubClasses.Add("SubClass");
            SubClasses.Add("Property");
            Properties.Add("Name");
            Properties.Add("Inheritance");
        }

        public Class(GenData genData) : this()
        {
            GenData = genData;
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
                               GenObject = ((GenObject)GenObject).CreateGenObject("SubClass"),
                               Name = name,
                               Reference = reference,
                               Relationship = relationship
                           };
            SubClassList.Add(item);
            return item;
        }


        public Property AddProperty(string name)
        {
            var item = new Property(GenData)
                           {
                               GenObject = ((GenObject)GenObject).CreateGenObject("Property"),
                               Name = name
                           };
            PropertyList.Add(item);
            return item;
        }

    }
}
