using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Definition;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.Controls
{
    public class ClassTreeNode: DataEditorTreeNodeBase
    {
        /// <summary>
        /// The tree node of the parent subclass of this class.
        /// </summary>
        public new SubClassTreeNode ParentNode
        {
            get
            {
                return (SubClassTreeNode) base.ParentNode;
            }
            private set
            {
                base.ParentNode = value;
            }
        }

        /// <summary>
        /// The data definition of this class.
        /// </summary>
        public IGenDataDefClass ClassDef { get; private set; }

        /// <summary>
        /// The Definition of the class.
        /// </summary>
        public Class Def { get; private set; }
        /// <summary>
        /// The selected node object.
        /// </summary>
        public GenObject GenObject { get; private set; }
        /// <summary>
        /// The saved context.
        /// </summary>
        public GenAttributes GenAttributes { get; private set; }

        /// <summary>
        /// The editor view model for this node
        /// </summary>
        public new GenObjectViewModel ViewModel
        {
            get { return (GenObjectViewModel) base.ViewModel; }
        }

        /// <summary>
        /// Create a new <see cref="ClassTreeNode"/> for the selected class
        /// </summary>
        /// <param name="parentNode">The tree node of the parent subclass of this class.</param>
        /// <param name="genData">The data being navigated.</param>
        /// <param name="definition">The definition data for the data being navigated.</param>
        /// <param name="classId">The ID of this class.</param>
        public ClassTreeNode(SubClassTreeNode parentNode, GenData genData, Definition definition, int classId)
        {
            ClassId = classId;
            GenData = genData;
            ParentNode = parentNode;
            GenObject = GenData.Context[ClassId].GenObject;
            SavedContext = GenData.SaveContext(ClassId, ParentNode.SavedContext);
            ClassDef = GenData.GenDataDef.Classes[ClassId];
            Def = ClassId > definition.ClassList.Count ? null : definition.ClassList[ClassId-1];
            GenAttributes = new GenAttributes(GenData.GenDataDef, classId) { GenObject = GenObject };

            if (ClassDef.Properties.IndexOf("Name") == -1)
                Text = ClassDef.Name;
            else
                Text = GenAttributes.AsString("Name");
            ImageIndex = 1;
            ToolTipText = Def == null ? "" : Def.Title;
            Tag = new GenObjectViewModel(GenObject, Def, SavedContext, ClassDef.IsReference);

            for (var i = 0; i < ClassDef.SubClasses.Count; i++)
                Nodes.Add(new SubClassTreeNode(this, GenData, definition, ClassDef.SubClasses[i].SubClass.ClassId));
        }

        /// <summary>
        /// Add a new tree node of the current class.
        /// </summary>
        /// <returns>The added tree node.</returns>
        public override ClassTreeNode AddNewNode()
        {
            return ParentNode.AddNewNode();
        }

        /// <summary>
        /// Make the specified item move.
        /// </summary>
        /// <param name="move">The specified move.</param>
        public override bool MoveItem(ListMove move)
        {
            return ParentNode.MakeMove(this, move);
        }

        /// <summary>
        /// Create the root node of the data explorer tree view.
        /// </summary>
        /// <param name="data">The data to be explored.</param>
        /// <returns>The root node.</returns>
        public static SubClassTreeNode CreateRootNode(GeData data)
        {
            return new SubClassTreeNode(null, data.GenData, new Definition(data.DefGenData), 1);
        }
    }
}
