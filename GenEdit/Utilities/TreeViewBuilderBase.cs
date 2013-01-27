using System.Windows.Forms;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.Utilities
{
    public class TreeViewBuilderBase
    {
        protected TreeViewBuilderBase(GeData data)
        {
            Data = data;
            dat = data.GenData;
            def = data.GenDataDef;
            dat.First(0);
        }

        protected GeData Data { get; set; }
        protected GenDataDef def { get; set; }
        protected GenData dat { get; set; }

        protected TreeNode CreateTreeNode(int iconIndex, string name, string hint, object nodeItem)
        {
            var classItem = new TreeNode();
            classItem.Text = name;
            classItem.ImageIndex = iconIndex;
            if (hint != "") classItem.ToolTipText = hint;
            classItem.Tag = nodeItem;
            return classItem;
        }
    }
}