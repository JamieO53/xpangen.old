using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenSubClasses : List<GenObjectListBase>
    {
        public GenObject Parent { get; private set; }
        public GenDataBase GenData { get { return Parent.GenData; } }
        
        public GenSubClasses(GenObject parent)
        {
            Parent = parent;
            for (var i = 0; i < GenData.GenDataDef.Classes[Parent.ClassId].SubClasses.Count; i++)
                Add(new GenObjectListBase(GenData, Parent, GenData.GenDataDef.Classes[Parent.ClassId].SubClasses[i].SubClass.ClassId));
        }
    }
}