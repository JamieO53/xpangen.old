using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenSubClasses : List<IGenObjectListBase>
    {
        public GenObject Parent { get; private set; }
        public GenDataBase GenData { get { return Parent.GenData; } }
        
        public GenSubClasses(GenObject parent)
        {
            Parent = parent;
            var parentDef = Parent.GenDataDefClass;
            for (var i = 0; i < parentDef.SubClasses.Count; i++)
            {
                var subClassDef = parentDef.SubClasses[i];
                var subClassClassDef = subClassDef.SubClass;
                if (string.IsNullOrEmpty(subClassDef.ReferenceDefinition))
                    Add(new GenObjectListBase(parent.GenData, Parent, subClassClassDef.ClassId, subClassDef));
                else
                {
                    Add(new GenObjectListReference(parent.GenData, Parent, subClassClassDef.ClassId, subClassDef));
                }
            }
        }
    }
}