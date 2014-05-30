using System.Collections;
using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenSubClass : List<GenObject>, IGenObjectListBase, IGenObject
    {
        /// <summary>
        /// Create a new <see cref="GenObjectList"/> list.
        /// </summary>
        /// <param name="data">The generator data containing the list.</param>
        /// <param name="parent">The generator object owning the list.</param>
        /// <param name="classId">The ID of objects in the list.</param>
        /// <param name="subClassDef">The definition of the subclass</param>
        public GenSubClass(GenDataBase data, GenObject parent, int classId, GenDataDefSubClass subClassDef)
        {
            Properties = new NameList {"Reference", "Relationship"};
            Attributes = new TextList {"", ""};
            Reference = "";
            Definition = subClassDef;
            GenDataBase = data;
            Parent = parent;
            ClassId = classId;
            IsReset = false;
        }

        private const int ReferenceIdx = 0;
        private const int RelationshipIdx = 1;

        /// <summary>
        /// The generator data containing the list.
        /// </summary>
        public GenDataBase GenDataBase { get; private set; }

        /// <summary>
        /// The generator object owning the list.
        /// </summary>
        public GenObject Parent { get; private set; }

        public TextList Attributes { get; private set; }

        public NameList Properties { get; private set; }
        public string ClassName { get { return Parent.Definition.Name + "_" + Definition.SubClass.Name; } }

        /// <summary>
        /// The ID of objects in the list.
        /// </summary>
        public int ClassId { get; private set; }

        public string Reference
        {
            get { return Attributes[ReferenceIdx]; }
            set { Attributes[ReferenceIdx] = value; }
        }

        public string Relationship
        {
            get { return Attributes[RelationshipIdx]; }
            set { Attributes[RelationshipIdx] = value; }
        }

        public GenDataDefSubClass Definition { get; private set; }
        public bool IsReset { get; set; }

        public void Reset()
        {
            // Do nothing
        }

        /// <summary>
        /// Create a new <see cref="Data.GenObject"/> and add it to the list.
        /// </summary>
        /// <returns>The new item.</returns>
        public GenObject CreateObject()
        {
            var o = new GenObject(Parent, this, ClassId);
            Add(o);
            return o;

        }

        public override string ToString()
        {
            return GenDataBase + "." + Parent.Definition.Name + "_" + Definition.SubClass.Name;
        }
    }
}