using System;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Definition;

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
        public IGenDataDefSubClass SubClassDef { get; private set; }
        /// <summary>
        /// The Definition of the subclass.
        /// </summary>
        public SubClass Def { get; private set; }
        /// <summary>
        /// The list that contains the <see cref="GenObject"/> items in the subclass.
        /// </summary>
        public ISubClassBase SubClassBase { get; private set; }
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
            SubClassBase = GenData.Context[ClassId].SubClassBase;
            SavedContext = ClassId == 1 ? null : ParentNode.SavedContext;
            Definition = definition;
            
            var genDataDef = GenData.GenDataDef;
            var parentClassId = ParentNode == null ? 0 : ParentNode.ClassId;
            var parentClassName = genDataDef.Classes[parentClassId].Name;
            var parentClass = definition == null ? null : definition.ClassList.Find(parentClassName);
            var i = genDataDef.IndexOfSubClass(parentClassId, ClassId);
            SubClassDef = genDataDef.Classes[parentClassId].SubClasses[i];
            Def = parentClass == null ? null : parentClass.SubClassList[i];
            
            Text = genDataDef.Classes[ClassId].Name +
                   (!string.IsNullOrEmpty(SubClassDef.Reference) ? ":" + GenData.Context[ClassId].Reference : "");
            ImageIndex = 2;
            ToolTipText = Text;
            var genObject = GenData.Context[ClassId].GenObject;
            //if (genObject != null)
                Tag = new SubClassViewModel(ParentNode == null ? null : ParentNode.GenObject.SubClass[i], SubClassBase, Def,
                                            SubClassDef,
                                            SavedContext, !string.IsNullOrEmpty(SubClassDef.Reference));

            GenData.First(ClassId);
            for (var j = 0; j < SubClassBase.Count; j++)
            {
                Nodes.Add(new ClassTreeNode(this, GenData, Definition, ClassId));
                GenData.Next(ClassId);
            }
        }

        /// <summary>
        /// The editor view model for this node
        /// </summary>
        public new SubClassViewModel ViewModel
        {
            get { return (SubClassViewModel)base.ViewModel; }
        }

        /// <summary>
        /// Add a new tree node of the current class.
        /// </summary>
        /// <returns>The added tree node.</returns>
        public override ClassTreeNode AddNewNode()
        {
            var parentClassName = ParentNode == null ? "" : ParentNode.ClassDef.Name;
            var className = SubClassDef.SubClass.Name;
            if (ParentNode != null && ParentNode.ViewModel != null) ParentNode.ViewModel.EstablishContext();
            else GenData.First(0);
            if (string.IsNullOrEmpty(SubClassDef.ReferenceDefinition))
            {
                var genObject = GenData.CreateObject(parentClassName, className);
                var idx = genObject.Definition.Properties.IndexOf("Name");
                if (idx >= 0)
                    genObject.Attributes[idx] = "new";
                GenData.Last(ClassId);
                var node = new ClassTreeNode(this, GenData, Definition, ClassId);
                node.ViewModel.IsNew = true;
                node.ViewModel.Save();
                Nodes.Add(node);
                return node;
            }
            return null;
        }

        /// <summary>
        /// Do nothing. Only Class nodes can be moved.
        /// </summary>
        /// <param name="move">The specified move.</param>
        public override bool MoveItem(ListMove move)
        {
            return false;
        }
        
        internal bool MakeMove(ClassTreeNode node, ListMove move)
        {
            var nodeData = node.ViewModel;
            if (nodeData == null) return false;
            var index = Nodes.IndexOf(node);
            var genData = nodeData.SavedContext.GenData;
            var classId = nodeData.GenAttributes.GenObject.ClassId;
            bool result;
            switch (move)
            {
                case ListMove.ToTop:
                    result =  MoveToTop(index, node);
                    break;
                case ListMove.Up:
                    result =  MoveUp(index, node);
                    break;
                case ListMove.Down:
                    result =  MoveDown(index, node);
                    break;
                case ListMove.ToBottom:
                    result =  MoveToBottom(index, node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("move");
            }

            genData.Context[classId].MoveItem(move, index);
            TreeView.SelectedNode = node;
            return result;
        }

        private bool MoveToTop(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index <= 0 || index >= nodes.Count) return false;
            TreeView.BeginUpdate();
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Insert(0, node);
            TreeView.SelectedNode = node;
            TreeView.EndUpdate();
            return true;
        }

        private bool MoveUp(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index <= 0 || index >= nodes.Count) return false;
            TreeView.BeginUpdate();
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Insert(index - 1, node);
            TreeView.SelectedNode = node;
            TreeView.EndUpdate();
            return true;
        }

        private bool MoveDown(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index < 0 || index >= nodes.Count - 1) return false;
            TreeView.BeginUpdate();
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Insert(index + 1, node);
            TreeView.SelectedNode = node;
            TreeView.EndUpdate();
            return true;
        }

        private bool MoveToBottom(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index < 0 || index >= nodes.Count - 1) return false;
            TreeView.BeginUpdate();
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Add(node);
            TreeView.SelectedNode = node;
            TreeView.EndUpdate();
            return true;
        }

    }
}
