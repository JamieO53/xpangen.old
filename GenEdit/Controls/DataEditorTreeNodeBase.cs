using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Definition;

namespace GenEdit.Controls
{
    public abstract class DataEditorTreeNodeBase : TreeNode
    {
        /// <summary>
        /// The tree node of the parent subclass of this class.
        /// </summary>
        protected DataEditorTreeNodeBase ParentNode { get; set; }

        /// <summary>
        /// The data being navigated.
        /// </summary>
        protected GenDataBase GenDataBase { get; set; }

        /// <summary>
        /// The definition data for the data being navigated.
        /// </summary>
        protected Definition Definition { get; set; }

        /// <summary>
        /// The ID of this class.
        /// </summary>
        public int ClassId { get; protected set; }

        /// <summary>
        /// The editor view model for this node
        /// </summary>
        public GenDataViewModelBase ViewModel
        {
            get { return Tag as GenDataViewModelBase; }
        }

        /// <summary>
        /// Add a new tree node of the current class.
        /// </summary>
        /// <returns>The added tree node.</returns>
        public abstract ClassTreeNode AddNewNode();
    }
}