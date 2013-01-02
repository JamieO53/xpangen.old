using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenObject
    {
        private GenData _genData;

        public GenObject(GenObject parent, GenObjectList parentSubClass, int classId)
        {
            Parent = parent;
            ParentSubClass = parentSubClass;
            ClassId = classId;
            Attributes = new TextList();
            SubClass = new List<GenObjectList>();
            if (Parent != null) GenData = Parent.GenData;
        }

        public GenData GenData
        {
            get { return _genData; }
            set 
            {
                if (_genData == value) return;
                _genData = value;
                for (var i = 0; i < GenData.GenDataDef.Properties[ClassId].Count; i++)
                    Attributes.Add("");
                for (var i = 0; i < GenData.GenDataDef.SubClasses[ClassId].Count; i++)
                    SubClass.Add(new GenObjectList(GenData, this, GenData.GenDataDef.SubClasses[ClassId][i]));
            }
        }

        public TextList Attributes { get; set; }

        public GenObjectList ParentSubClass { get; set; }
        public int ClassId { get; set; }

        public GenObject Parent { get; set; }
        public List<GenObjectList> SubClass { get; set; }
    }
}
