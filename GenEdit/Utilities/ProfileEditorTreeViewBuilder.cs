// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Windows.Forms;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Profile;

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

        private TreeNode CreateFragmentTree(GenFragment genFragment)
        {
            var genFragmentLabel = new GenFragmentLabel(genFragment.Fragment);
            var item = CreateTreeNode((int)genFragment.FragmentType, genFragmentLabel.ProfileLabel(), null, genFragment);
            if (genFragment is GenContainerFragmentBase && !(genFragment is GenFunction))
                CreateBodyChildTrees(item.Nodes, (GenContainerFragmentBase) genFragment);
            return item;
        }

        public static string GetNodeExpansionText(GenData genData, GenObject genObject, Fragment fragment)
        {
            var context = genData.GetContext(genObject, fragment.ClassName());
            if (context != null)
                return GenFragmentExpander.Expand(genData.GenDataDef, context, fragment);
            return "";
        }

        public static string GetNodeProfileText(ProfileFragmentSyntaxDictionary dictionary, Fragment fragment)
        {
            var text = new GenProfileTextExpander(dictionary).GetText(fragment);
            return text;
        }
    }
}
