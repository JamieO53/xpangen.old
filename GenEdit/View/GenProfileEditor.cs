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
        private object _selectedItem;
        private GenFragment _fragment;
        private bool IsBuilding { get; set; }
        
        public GenProfileEditor()
        {
            InitializeComponent();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (IsBuilding) return;

            var contextData = GenDataEditorViewModel.Data.GenData.DuplicateContext();
            _selectedItem = e.Node;
            _fragment = ProfileEditorTreeViewBuilder.GetNodeData(_selectedItem);
            RefreshProfile(contextData);
        }

        private void GenProfileEditor_Load(object sender, System.EventArgs e)
        {
        }

        public void LoadData()
        {
            ProfileNavigatorTreeView.Nodes.Clear();
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null || GenDataEditorViewModel.Data.Profile == null) return;
            
            var builder = new ProfileEditorTreeViewBuilder(GenDataEditorViewModel.Data) { ShowText = false };

            IsBuilding = true;
            builder.CreateBodyChildTrees(ProfileNavigatorTreeView.Nodes, GenDataEditorViewModel.Data.Profile);
            IsBuilding = false;
            _selectedItem = ProfileNavigatorTreeView.Nodes.Count > 0 ? ProfileNavigatorTreeView.Nodes[0] : null;
            RefreshProfile(GenDataEditorViewModel.Data.GenData);
        }

        public void RefreshProfile(GenData genData)
        {
            var text = _fragment != null
                           ? ProfileEditorTreeViewBuilder.GetNodeProfileText(_selectedItem, _fragment,
                                                                             ProfileFragmentSyntaxDictionary
                                                                                 .ActiveProfileFragmentSyntaxDictionary)
                           : "";

            // Don't change to prevent unnecessary rendering and side effects
            if (text != ProfileTextBox.Text)
            {
                ProfileTextBox.Clear();
                ProfileTextBox.Text = text;
            }


            text = _fragment != null
                       ? ProfileEditorTreeViewBuilder.GetNodeExpansionText(_selectedItem, _fragment, genData)
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
