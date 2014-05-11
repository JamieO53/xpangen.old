using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;

namespace GenEdit.Controls
{
    public class SubClassTreeNode: DataEditorTreeNodeBase
    {
        /// <summary>
        /// The tree node of the parent class of this subclass.
        /// </summary>
        public new ClassTreeNode ParentNode
        {
            get
            {
                return (ClassTreeNode)base.ParentNode;
            }
            private set
            {
                base.ParentNode = value;
            }
        }
        /// <summary>
        /// The data definition of this subclass.
        /// </summary>
        public GenDataDefSubClass SubClassDef { get; private set; }
        /// <summary>
        /// The Definition of the subclass.
        /// </summary>
        public SubClass Def { get; private set; }
        /// <summary>
        /// The list that contains the <see cref="GenObject"/> items in the subclass.
        /// </summary>
        public IGenObjectListBase GenObjectListBase { get; private set; }
        /// <summary>
        /// The saved context.
        /// </summary>
        public new GenSavedContext SavedContext { get { return base.SavedContext; } private set { base.SavedContext = value; } }

        /// <summary>
        /// Create a new <see cref="SubClassTreeNode"/> for the selected subclass.
        /// </summary>
        /// <param name="parentNode">The tree node of the parent class of this subclass.</param>
        /// <param name="genData">The data being navigated.</param>
        /// <param name="definition">The definition data for the data being navigated.</param>
        /// <param name="classId">The ClassId of this subclass.</param>
        public SubClassTreeNode(ClassTreeNode parentNode, GenData genData, Definition definition, int classId)
        {
            ClassId = classId;
            ParentNode = parentNode;
            GenData = genData;
            GenObjectListBase = GenData.Context[ClassId].GenObjectListBase;
            SavedContext = ClassId == 1 ? null : ParentNode.SavedContext;
            Definition = definition;
            
            var genDataDef = GenData.GenDataDef;
            var parentClassId = ParentNode == null ? 0 : ParentNode.ClassId;
            var i = genDataDef.IndexOfSubClass(parentClassId, ClassId);
            SubClassDef = genDataDef.Classes[parentClassId].SubClasses[i];
            Def = parentClassId == 0 || parentClassId > definition.ClassList.Count ? null : definition.ClassList[parentClassId - 1].SubClassList[i];
            
            Text = genDataDef.Classes[ClassId].Name +
                   (!string.IsNullOrEmpty(SubClassDef.Reference) ? ":" + GenData.Context[ClassId].Reference : "");
            ImageIndex = 2;
            ToolTipText = Text;
            var genObject = GenData.Context[ClassId].GenObject;
            if (genObject != null)
                Tag = new SubClassViewModel(ParentNode == null ? null : ParentNode.GenObject.SubClass[i], GenObjectListBase, Def,
                                            SubClassDef,
                                            SavedContext, !string.IsNullOrEmpty(SubClassDef.Reference));

            GenData.First(ClassId);
            for (var j = 0; j < GenObjectListBase.Count; j++)
            {
                Nodes.Add(new ClassTreeNode(this, GenData, Definition, ClassId));
                GenData.Next(ClassId);
            }
        }

        /// <summary>
        /// Add a new tree node of the current class.
        /// </summary>
        /// <returns>The added tree node.</returns>
        public override ClassTreeNode AddNewNode()
        {
            var node = new ClassTreeNode(this, GenData, Definition, ClassId);
            Nodes.Add(node);
            return node;
        }
    }
}
