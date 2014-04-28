// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.Utilities
{
    public class DataEditorTreeViewBuilder : TreeViewBuilderBase
    {
        public DataEditorTreeViewBuilder(GeData data)
            : base(data)
        {
        }

        public void CreateSubClassTrees(IList parentItems, GenSavedContext parentContext)
        {
            var classId = parentContext == null ? 0 : parentContext.ClassId;
            for (var i = 0; i < Def.Classes[classId].SubClasses.Count; i++)
            {
                var subClassId = Def.Classes[classId].SubClasses[i].SubClass.ClassId;
                var classSubTree = CreateClassSubTree(subClassId, parentContext);
                parentItems.Add(classSubTree);
            }
        }

        private TreeNode CreateClassSubTree(int classId, GenSavedContext parentContext)
        {
            var definition = Data.FindClassDefinition(classId);

            var classItem = CreateTreeNode(2, Def.Classes[classId].Name, Def.Classes[classId].Name,
                                           definition);

            var objectList = Dat.Context[classId];
            if (objectList != null && !Dat.Eol(classId))
            {
                Dat.First(classId);
                while (!Dat.Eol(classId))
                {
                    var obj = new GenObjectViewModel(objectList.GenObject, definition, Data.GenData.SaveContext(classId, parentContext));
                    var childItem = ChildItem(obj, classItem);
                    Dat.Next(classId);
                }
            }
            return classItem;
        }

        public TreeNode ChildItem(GenObjectViewModel obj, TreeNode classItem)
        {
            var childItem = CreateTreeNode(1, obj.Name, obj.Hint, obj);
            classItem.Nodes.Add(childItem);
            CreateSubClassTrees(childItem.Nodes, obj.SavedContext);
            return childItem;
        }

        public static GenObjectViewModel GetNodeData(object selectedItem)
        {
            if (selectedItem == null) return null;
            var item = (TreeNode) selectedItem;
            var tag = item.Tag is GenObjectViewModel ? item.Tag : (item.Parent != null ? item.Parent.Tag : null);
            var nodeData = tag as GenObjectViewModel;
            if (nodeData != null)
                nodeData.EstablishContext();
            return nodeData;
        }

        public void CreateNewChildItem(TreeView treeView)
        {
            var parentClassNode = treeView.SelectedNode.Tag is GenObjectViewModel
                                      ? treeView.SelectedNode.Parent
                                      : treeView.SelectedNode;
            var parentNode = parentClassNode.Parent;

            string parentClassName;
            GenSavedContext parentContext;
            if (parentNode != null)
            {
                var parentObj = DataEditorTreeViewBuilder.GetNodeData(parentNode);
                parentClassName = parentObj.GenAttributes.GenObject.Definition.Name;
                parentContext = parentObj.SavedContext;
            }
            else
            {
                parentClassName = "";
                parentContext = null;
            }

            var className = parentClassNode.Text;
            var o = Data.GenData.CreateObject(parentClassName, className);
            o.Attributes[0] = "new";

            var obj = new GenObjectViewModel(o, Data.FindClassDefinition(o.ClassId),
                                             Data.GenData.SaveContext(o.ClassId, parentContext));
            obj.IsNew = true;
            var childItem = ChildItem(obj, parentClassNode);
            treeView.SelectedNode = childItem;
        }
    }
}
