using System.Windows.Forms;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;

namespace GenEdit.Controls
{
    public abstract class DataEditorTreeNodeBase : TreeNode
    {
        /// <summary>
        /// The tree node of the parent subclass of this class.
        /// </summary>
        public DataEditorTreeNodeBase ParentNode { get; protected set; }

        /// <summary>
        /// The data being navigated.
        /// </summary>
        public GenData GenData { get; protected set; }

        /// <summary>
        /// The definition data for the data being navigated.
        /// </summary>
        public Definition Definition { get; protected set; }

        /// <summary>
        /// The ID of this class.
        /// </summary>
        public int ClassId { get; protected set; }

        /// <summary>
        /// The saved context.
        /// </summary>
        public GenSavedContext SavedContext { get; set; }

        /// <summary>
        /// Add a new tree node of the current class.
        /// </summary>
        /// <returns>The added tree node.</returns>
        public abstract ClassTreeNode AddNewNode();
    }
}