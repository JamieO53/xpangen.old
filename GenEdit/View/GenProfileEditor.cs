using System.Windows.Forms;
using GenEdit.Utilities;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;

namespace GenEdit.View
{
    public partial class GenProfileEditor : UserControl
    {
        public GenDataEditorViewModel GenDataEditorViewModel { get; set; }
        private bool IsBuilding { get; set; }
        
        public GenProfileEditor()
        {
            InitializeComponent();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (IsBuilding) return;

            var contextData = GenDataEditorViewModel.Data.GenData.DuplicateContext();
            RefreshProfile(contextData);
        }

        private void GenProfileEditor_Load(object sender, System.EventArgs e)
        {
        }

        public void LoadData()
        {
            ProfileNavigatorTreeView.Nodes.Clear();
            ProfileExpansionTextBox.Clear();
            ProfileTextBox.Clear();
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null || GenDataEditorViewModel.Data.Profile == null) return;
            
            var builder = new ProfileEditorTreeViewBuilder(GenDataEditorViewModel.Data) { ShowText = false };

            IsBuilding = true;
            builder.CreateBodyChildTrees(ProfileNavigatorTreeView.Nodes, GenDataEditorViewModel.Data.Profile);
            IsBuilding = false;
            RefreshProfile(GenDataEditorViewModel.Data.GenData);
            if (ProfileNavigatorTreeView.Nodes.Count > 0)
                ProfileNavigatorTreeView.SelectedNode = ProfileNavigatorTreeView.Nodes[0];
        }

        public void RefreshProfile(GenData genData)
        {
            var selectedItem = ProfileNavigatorTreeView.SelectedNode;
            var text = ProfileEditorTreeViewBuilder.GetNodeData(selectedItem) != null
                           ? ProfileEditorTreeViewBuilder.GetNodeProfileText(selectedItem, ProfileEditorTreeViewBuilder.GetNodeData(selectedItem),
                                                                             ProfileFragmentSyntaxDictionary
                                                                                 .ActiveProfileFragmentSyntaxDictionary)
                           : "";

            // Don't change to prevent unnecessary rendering and side effects
            if (text != ProfileTextBox.Text)
            {
                ProfileTextBox.Clear();
                ProfileTextBox.Text = text;
            }


            text = ProfileEditorTreeViewBuilder.GetNodeData(selectedItem) != null
                       ? ProfileEditorTreeViewBuilder.GetNodeExpansionText(selectedItem, ProfileEditorTreeViewBuilder.GetNodeData(selectedItem), genData)
                       : "";

            // Don't change to prevent unnecessary rendering and side effects
            if (text != ProfileExpansionTextBox.Text)
            {
                ProfileExpansionTextBox.Clear();
                ProfileExpansionTextBox.Text = text;

            }
        }
    }
}
