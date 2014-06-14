using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenSubClasses : List<ISubClassBase>
    {
        public GenObject Parent { get; private set; }
        public GenDataBase GenData { get { return Parent.GenDataBase; } }
        
        public GenSubClasses(GenObject parent)
        {
            Parent = parent;
            var parentDef = Parent.Definition;
            for (var i = 0; i < parentDef.SubClasses.Count; i++)
            {
                var subClassDef = parentDef.SubClasses[i];
                var subClassClassDef = subClassDef.SubClass;
                if (string.IsNullOrEmpty(subClassDef.ReferenceDefinition))
                    Add(new GenSubClass(parent.GenDataBase, Parent, subClassClassDef.ClassId, subClassDef));
                else
                    Add(new SubClassReference(parent.GenDataBase, Parent, subClassClassDef.ClassId, subClassDef));
            }
        }
    }
}