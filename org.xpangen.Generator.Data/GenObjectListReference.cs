using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenObjectListReference : List<GenObject>, IGenObjectListBase
    {
        public GenObjectListReference(GenDataBase genDataBase, GenObject parent, int classId, GenDataDefSubClass subClassDef)
        {
            GenDataBase = genDataBase;
            Definition = subClassDef;
            ClassId = classId;
            Parent = parent;
            IsReset = true;
        }

        public GenDataBase GenDataBase { get; private set; }
        public GenObject Parent { get; private set; }
        public int ClassId { get; private set; }
        public GenDataDefSubClass Definition { get; private set; }
        public void Reset()
        {
            Clear();
            IsReset = true;
        }

        public bool IsReset { get; set; }
    }
}