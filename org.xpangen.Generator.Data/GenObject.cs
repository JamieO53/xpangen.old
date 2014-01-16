// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public class GenObject
    {
        private GenDataBase _genData;

        public GenObject(GenObject parent, GenObjectListBase parentSubClass, int classId)
        {
            Parent = parent;
            ParentSubClass = parentSubClass;
            ClassId = classId;
            Attributes = new TextList();
            if (Parent != null) GenData = Parent.GenData;
        }

        public GenDataBase GenData
        {
            get { return _genData; }
            set 
            {
                if (_genData == value) return;
                _genData = value;
                for (var i = 0; i < GenDataDefClass.Properties.Count; i++)
                    Attributes.Add("");
                SubClass = new GenSubClasses(this);
            }
        }

        public TextList Attributes { get; private set; }

        public GenObjectListBase ParentSubClass { get; private set; }
        public int ClassId { get; private set; }

        public GenObject Parent { get; private set; }
        public GenSubClasses SubClass { get; private set; }
        public GenDataDefClass GenDataDefClass { get { return GenData.GenDataDef.Classes[ClassId]; } }
    }
}
