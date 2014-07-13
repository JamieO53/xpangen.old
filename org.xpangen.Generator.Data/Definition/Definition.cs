// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Definition
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
            Classes.Add("Class");
            Classes.Add("SubClass");
            Classes.Add("Property");
            SubClasses.Add("Class");
            base.GenObject = genData.Root;
        }

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.Definition = "Definition";
            f.AddSubClass("", "Class");
            f.AddSubClass("Class", "SubClass");
            f.AddSubClass("Class", "Property");
            f.Classes[1].InstanceProperties.Add("Name");
            f.Classes[1].InstanceProperties.Add("Title");
            f.Classes[1].InstanceProperties.Add("Inheritance");
            f.Classes[2].InstanceProperties.Add("Name");
            f.Classes[2].InstanceProperties.Add("Reference");
            f.Classes[2].InstanceProperties.Add("Relationship");
            f.Classes[3].InstanceProperties.Add("Name");
            f.Classes[3].InstanceProperties.Add("Title");
            f.Classes[3].InstanceProperties.Add("DataType");
            f.Classes[3].InstanceProperties.Add("Default");
            f.Classes[3].InstanceProperties.Add("LookupType");
            f.Classes[3].InstanceProperties.Add("LookupDependence");
            f.Classes[3].InstanceProperties.Add("LookupTable");
            return f;
        }

        public GenNamedApplicationList<Class> ClassList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ClassList = new GenNamedApplicationList<Class>(this, 1, 0);
        }

        public Class AddClass(string name, string title = "", string inheritance = "")
        {
            var item = new Class(GenData)
                           {
                               GenObject = GenData.CreateObject("", "Class"),
                               Name = name,
                               Title = title,
                               Inheritance = inheritance
                           };
            ClassList.Add(item);
            return item;
        }
    }
}
