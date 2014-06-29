using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenSubClasses : List<ISubClassBase>
    {
        private GenObject Parent { get; set; }

        public GenSubClasses(GenObject parent)
        {
            Parent = parent;
            var parentDef = Parent.Definition;
            AddInheritedSubClasses(parentDef, parent.GenDataBase);
            AddSubClasses(parentDef, parent.GenDataBase);
        }

        private void AddSubClasses(GenDataDefClass parentDef, GenDataBase genDataBase)
        {
            for (var i = 0; i < parentDef.SubClasses.Count; i++)
                AddSubClass(parentDef.SubClasses[i], genDataBase);
        }

        private void AddInheritedSubClasses(GenDataDefClass parentDef, GenDataBase genDataBase)
        {
            if (parentDef.IsInherited)
            {
                AddInheritedSubClasses(parentDef.Parent, genDataBase);
                AddSubClasses(parentDef.Parent, genDataBase);
            }
        }

        private void AddSubClass(GenDataDefSubClass subClassDef, GenDataBase genDataBase)
        {
            var subClassClassDef = subClassDef.SubClass;
            if (string.IsNullOrEmpty(subClassDef.ReferenceDefinition))
                Add(new GenSubClass(genDataBase, Parent, subClassClassDef.ClassId, subClassDef));
            else
                Add(new SubClassReference(genDataBase, Parent, subClassClassDef.ClassId, subClassDef));
        }
    }
}