// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Definition
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

        public GenNamedApplicationList<SubClass> SubClassList { get; private set; }
        public GenNamedApplicationList<Property> PropertyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            SubClassList = new GenNamedApplicationList<SubClass>(this);
            PropertyList = new GenNamedApplicationList<Property>(this);
        }

        public SubClass AddSubClass(string name, string reference = "")
        {
            var item = new SubClass(GenData)
                           {
                               GenObject = GenData.CreateObject("Class", "SubClass"),
                               Name = name,
                               Reference = reference
                           };
            SubClassList.Add(item);
            return item;
        }


        public Property AddProperty(string name, string title = "", string dataType = "String", string @default = "")
        {
            var item = new Property(GenData)
                           {
                               GenObject = GenData.CreateObject("Class", "Property"),
                               Name = name,
                               Title = title,
                               DataType = dataType,
                               Default = @default
                           };
            PropertyList.Add(item);
            return item;
        }
    }
}
