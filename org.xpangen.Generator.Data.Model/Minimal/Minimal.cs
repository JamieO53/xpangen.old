// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Minimal
{
    /// <summary>
    /// Classes allowing access to generator data for the Definition definition
    /// </summary>
    public class Minimal : GenApplicationBase
    {
        public Minimal(): this(new GenDataBase(GetDefinition()))
        {
        }

        public Minimal(GenDataBase genDataBase)
        {
            GenDataBase = genDataBase;
            Classes.Add("Class");
            Classes.Add("SubClass");
            Classes.Add("Property");
            SubClasses.Add("Class");
            base.GenObject = genDataBase.Root;
        }

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.DefinitionName = "Minimal";
            f.AddSubClass("", "Class");
            f.AddSubClass("Class", "SubClass");
            f.AddSubClass("Class", "Property");
            f.Classes[1].AddInstanceProperty("Name");
            f.Classes[1].AddInstanceProperty("Inheritance");
            f.Classes[2].AddInstanceProperty("Name");
            f.Classes[2].AddInstanceProperty("Reference");
            f.Classes[2].AddInstanceProperty("Relationship");
            f.Classes[3].AddInstanceProperty("Name");
            return f;
        }

        public GenNamedApplicationList<Class> ClassList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ClassList = new GenNamedApplicationList<Class>(this, 1, 0);
        }

        public Class AddClass(string name, string inheritance = "")
        {
            var item = new Class(GenDataBase)
                           {
                               GenObject = GenDataBase.Root.CreateGenObject("Class"),
                               Name = name,
                               Inheritance = inheritance
                           };
            ClassList.Add(item);
            return item;
        }
    }
}
