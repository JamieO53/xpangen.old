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

        public void CreateBodyChildTrees(TreeNodeCollection items, FragmentBody container)
        {
            var i = 0;
            while (i < container.FragmentList.Count)
            {
                var fragment = container.FragmentList[i];
                var node = CreateFragmentTree(fragment);
                items.Add(node);
                i++;
            }
        }

        public static Fragment GetNodeData(object selectedItem)
        {
            return selectedItem != null ? ((TreeNode) selectedItem).Tag as Fragment : null;
        }

        private TreeNode CreateFragmentTree(Fragment genFragment)
        {
            var genFragmentLabel = new GenFragmentLabel(genFragment);
            var item = CreateTreeNode((int)genFragment.FragmentType, genFragmentLabel.ProfileLabel(), null, genFragment);
            if (genFragment is ContainerFragment && !(genFragment is Function))
                CreateBodyChildTrees(item.Nodes, ((ContainerFragment) genFragment).Body());
            return item;
        }

        public static string GetNodeExpansionText(GenDataBase genData, GenObject genObject, Fragment fragment)
        {
            var context = GenObject.GetContext(genObject, fragment.ClassName());
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
