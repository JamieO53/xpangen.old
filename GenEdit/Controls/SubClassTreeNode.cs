﻿using System;
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
        public GenDataDefSubClass SubClassDef { get; private set; }
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
//        public new GenSavedContext SavedContext { get { return base.SavedContext; } private set { base.SavedContext = value; } }

        /// <summary>
        /// Create a new <see cref="SubClassTreeNode"/> for the selected subclass.
        /// </summary>
        /// <param name="parentNode">The tree node of the parent class of this subclass.</param>
        /// <param name="genDataBase">The data being navigated.</param>
        /// <param name="definition">The definition data for the data being navigated.</param>
        /// <param name="classId">The ClassId of this subclass.</param>
        public SubClassTreeNode(ClassTreeNode parentNode, GenDataBase genDataBase, Definition definition, int classId)
        {
            ClassId = classId;
            ParentNode = parentNode;
            GenDataBase = genDataBase;
            Definition = definition;
            var genDataDef = GenDataBase.GenDataDef;
                int parentClassId;
            string parentClassName;
            Class parentClass;
                Def = null;
            if (ParentNode == null)
                SubClassBase = genDataBase.Root.SubClass[0];
            else
            {
                parentClassId = ParentNode.ClassId;
                parentClassName = genDataDef.GetClassName(parentClassId);
                SubClassBase = ParentNode.GenObject.GetSubClass(genDataDef.GetClassName(classId));
                parentClass = definition.ClassList.Find(parentClassName);
                var i = genDataDef.Classes[parentClassId].IndexOfSubClass(genDataDef.GetClassName(ClassId));
                SubClassDef = genDataDef.GetClassSubClasses(parentClassId)[i];
                if (parentClass != null) Def = parentClass.SubClassList[i];
            }

            Text = genDataDef.GetClassName(ClassId) +
                   (SubClassDef != null && !string.IsNullOrEmpty(SubClassDef.Reference)
                       ? ":" + SubClassBase.Reference
                       : "");
            ImageIndex = 2;
            ToolTipText = Text;
            Tag = new SubClassViewModel(ParentNode == null ? null : ParentNode.GenObject.ParentSubClass, SubClassBase, Def,
                                        SubClassDef, SubClassDef != null && !string.IsNullOrEmpty(SubClassDef.Reference));

            for (var j = 0; j < SubClassBase.Count; j++)
            {
                Nodes.Add(new ClassTreeNode(this, GenDataBase, Definition, ClassId, SubClassBase[j]));
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
            if (string.IsNullOrEmpty(SubClassDef.ReferenceDefinition))
            {
                
                var parent = ParentNode == null ? GenDataBase.Root : ParentNode.GenObject;
                var genObject = parent.CreateGenObject(className);
                var idx = genObject.Definition.Properties.IndexOf("Name");
                if (idx >= 0)
                    genObject.Attributes[idx] = "new";
                var node = new ClassTreeNode(this, GenDataBase, Definition, ClassId, genObject);
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
        public virtual bool MoveItem(ListMove move)
        {
            return false;
        }
        
        internal bool MakeMove(ClassTreeNode node, ListMove move)
        {
            var nodeData = node.ViewModel;
            if (nodeData == null) return false;
            var index = Nodes.IndexOf(node);
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
