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
                    var childItem = CreateTreeNode(1, obj.Name, obj.Hint, obj);
                    classItem.Nodes.Add(childItem);
                    CreateSubClassTrees(childItem.Nodes, obj.SavedContext);
                    Dat.Next(classId);
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
