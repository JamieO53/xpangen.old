// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Definition
{
    /// <summary>
    /// Classes allowing access to generator data for the Definition definition
    /// </summary>
    public class Definition : GenApplicationBase
    {
        public Definition(): this(new GenData(GetDefinition()))
        {
        }

        public Definition(GenData genData)
        {
            GenData = genData;
            base.GenObject = genData.Root;
		}

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.Definition = "Definition";
            f.AddSubClass("", "Class");
            f.AddSubClass("Class", "SubClass");
            f.AddSubClass("Class", "Property");
            f.Classes[1].Properties.Add("Name");
            f.Classes[1].Properties.Add("Title");
            f.Classes[2].Properties.Add("Name");
            f.Classes[2].Properties.Add("Reference");
            f.Classes[3].Properties.Add("Name");
            f.Classes[3].Properties.Add("Title");
            f.Classes[3].Properties.Add("DataType");
            f.Classes[3].Properties.Add("Default");
            f.Classes[3].Properties.Add("LookupTable");
            f.Classes[3].Properties.Add("LookupPath");
            f.Classes[3].Properties.Add("LookupType");
            return f;
        }

        public GenNamedApplicationList<Class> ClassList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ClassList = new GenNamedApplicationList<Class>(this);
        }

        public Class AddClass(string name, string title = "")
        {
            var item = new Class(GenData)
                           {
                               GenObject = GenData.CreateObject("", "Class"),
                               Name = name,
                               Title = title
                           };
            ClassList.Add(item);
            return item;
        }
    }
}
