// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Minimal
{
    /// <summary>
    /// Class Definition
    /// </summary>
    public class Class : GenApplicationBase
    {
        public Class(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// Object name: must be well formed
        /// </summary>
        public string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                SaveFields();
            }
        }

        public GenApplicationList<SubClass> SubClassList { get; private set; }
        public GenApplicationList<Property> PropertyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            SubClassList = new GenApplicationList<SubClass>();
            var list = new GenObjectList(GenObject.SubClass[0]);
            list.First();
            while (!list.Eol)
            {
                SubClassList.Add(new SubClass(GenDataDef) {GenObject = list.GenObject});
                list.Next();
            }

            PropertyList = new GenApplicationList<Property>();
            list = new GenObjectList(GenObject.SubClass[0]);
            list.First();
            while (!list.Eol)
            {
                PropertyList.Add(new Property(GenDataDef) {GenObject = list.GenObject});
                list.Next();
            }

        }
    }
}
