﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Windows.Forms;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Profile;

namespace GenEdit.Utilities
{
    public class ProfileEditorTreeViewBuilder: TreeViewBuilderBase
    {
        public ProfileEditorTreeViewBuilder(GeData data) : base(data)
        {
        }

        public void CreateBodyChildTrees(TreeNodeCollection items, GenContainerFragmentBase container)
        {
            var i = 0;
            while (i < container.Body.Count)
            {
                var fragment = container.Body.Fragment[i];
                var node = CreateFragmentTree(fragment);
                items.Add(node);
                i++;
            }
        }


        public static GenFragment GetNodeData(object selectedItem)
        {
            return selectedItem != null ? ((TreeNode) selectedItem).Tag as GenFragment : null;
        }

        private TreeNode CreateFragmentTree(GenFragment fragment)
        {
            var item = CreateTreeNode((int) fragment.FragmentType, fragment.ProfileLabel(), null, fragment);
            if (fragment is GenContainerFragmentBase && !(fragment is GenFunction))
                CreateBodyChildTrees(item.Nodes, (GenContainerFragmentBase) fragment);
            return item;
        }

        public static string GetNodeExpansionText(object selectedItem, GenFragment fragment, GenData genData)
        {
            string text;
            fragment.GenObject = genData.Context[fragment.ClassId].GenObject;
            if (GetNodeHeaderText(selectedItem) == "Text" && fragment is GenBlock)
                text = ((GenBlock) fragment).Body.Expand(genData);
            else
                text = GenFragmentExpander.Expand(genData, fragment.GenObject, fragment.Fragment);
            return text;
        }

        public static string GetNodeProfileText(object selectedItem, GenFragment fragment, ProfileFragmentSyntaxDictionary dictionary)
        {
            string text;
            if (GetNodeHeaderText(selectedItem) == "Text" && fragment is GenBlock)
                text = ((GenBlock)fragment).Body.ProfileText(dictionary);
            else
                text = fragment.ProfileText(dictionary);
            return text;
        }

        private static string GetNodeHeaderText(object selectedItem)
        {
            return ((TreeNode) selectedItem).Text;
        }
    }
}
