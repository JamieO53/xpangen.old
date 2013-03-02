// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Application;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.Utilities
{
    public class DataEditorTreeViewBuilder : TreeViewBuilderBase
    {
        public DataEditorTreeViewBuilder(GeData data)
            : base(data)
        {
        }

        public void CreateSubClassTrees(IList parentItems, int parentClassId)
        {
            for (var i = 0; i < def.SubClasses[parentClassId].Count; i++)
            {
                var classId = def.SubClasses[parentClassId][i];
                var classSubTree = CreateClassSubTree(classId);
                parentItems.Add(classSubTree);
            }
        }

        private TreeNode CreateClassSubTree(int classId)
        {
            var className = Data.GenDataDef.Classes[classId];
            GenApplicationBase definition = null;
            for (var i = 0; i < Data.ClassList.Count; i++)
            {
                if (Data.ClassList[i].Name == className)
                {
                    definition = Data.ClassList[i];
                    break;
                }
            }

            var classItem = CreateTreeNode(2, def.Classes[classId], def.Classes[classId],
                                           definition);

            var objectList = dat.Context[classId];
            if (objectList != null)
            {
                dat.First(classId);
                while (!dat.Eol(classId))
                {
                    TreeNode childItem;
                    var obj = new GenObjectViewModel(objectList.Context, definition);
                    childItem = CreateTreeNode(1, obj.Name, obj.Hint, obj);
                    classItem.Nodes.Add(childItem);
                    CreateSubClassTrees(childItem.Nodes, classId);
                    dat.Next(classId);
                }
            }
            return classItem;
        }

        public static GenObjectViewModel GetNodeData(object selectedItem)
        {
            return ((TreeNode)selectedItem).Tag as GenObjectViewModel;
        }
    }
}
