using System.Windows.Forms;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Profile;

namespace GenEdit.Utilities
{
    public class ProfileEditorTreeViewBuilder: TreeViewBuilderBase
    {
        public bool ShowText { get; set; }
        public ProfileEditorTreeViewBuilder(GeData data) : base(data)
        {
        }

        public void CreateBodyChildTrees(TreeNodeCollection items, GenContainerFragmentBase container)
        {
            var i = 0;
            while (i < container.Body.Count)
            {
                var fragment = container.Body.Fragment[i];
                TreeNode node;
                if (!ShowText && fragment.IsTextFragment && i + 1 < container.Body.Count &&
                    container.Body.Fragment[i + 1].IsTextFragment)
                {
                    var block = new GenBlock(container.GenDataDef, container);
                    block.Body.Add(fragment);

                    while (i + 1 < container.Body.Count && container.Body.Fragment[i + 1].IsTextFragment)
                    {
                        block.Body.Add(container.Body.Fragment[i + 1]);
                        i++;
                    }
                    node = CreateTreeNode((int) FragmentType.Text, "Text", null, block);
                }
                else
                    node = CreateFragmentTree(fragment);
                items.Add(node);
                i++;
            }
        }


        public static GenFragment GetNodeData(object selectedItem)
        {
            return ((TreeNode)selectedItem).Tag as GenFragment;
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
            if (GetNodeHeaderText(selectedItem) == "Text" && fragment is GenBlock)
                text = ((GenBlock) fragment).Body.Expand(genData);
            else
                text = fragment.Expand(genData);
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
