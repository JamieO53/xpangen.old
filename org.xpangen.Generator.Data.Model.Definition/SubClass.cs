// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Definition
{
    /// <summary>
    /// Class to SubClass link
    /// </summary>
    public class SubClass : GenNamedApplicationBase
    {
        public SubClass(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// Object description: used as a hint when editing
        /// </summary>
        public string Title
        {
            get { return AsString("Title"); }
            set
            {
                if (Title == value) return;
                SetString("Title", value);
                SaveFields();
            }
        }

        public GenApplicationList<SubClass> SubClassList { get; private set; }
        public GenApplicationList<Property> PropertyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            SubClassList = new GenApplicationList<SubClass>();
            var classId = GenDataDef.Classes.IndexOf("SubClass");
            var classIdx = GenDataDef.IndexOfSubClass(ClassId, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(GenObject.SubClass[classIdx]);
                list.First();
                while (!list.Eol)
                {
                    SubClassList.Add(new SubClass(GenDataDef) {GenObject = list.GenObject});
                    list.Next();
                }
            }

            PropertyList = new GenApplicationList<Property>();
            classId = GenDataDef.Classes.IndexOf("Property");
            classIdx = GenDataDef.IndexOfSubClass(ClassId, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(GenObject.SubClass[classIdx]);
                list.First();
                while (!list.Eol)
                {
                    PropertyList.Add(new Property(GenDataDef) {GenObject = list.GenObject});
                    list.Next();
                }
            }

        }
    }
}
