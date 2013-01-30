using System.ComponentModel;
using System.Windows.Forms;
using GenEdit.Utilities;
using GenEdit.ViewModel;

namespace GenEdit.View
{
    public partial class GenDataEditor : UserControl
    {
        public GenDataEditorViewModel GenDataEditorViewModel { get; set; }

        public delegate void DataChanged();

        public DataChanged OnDataChanged;

        public GenDataEditor()
        {
            InitializeComponent();
        }

        private void DataNavigatorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var data = GenDataEditorViewModel;
            data.SelectedNode = DataEditorTreeViewBuilder.GetNodeData(DataNavigatorTreeView.SelectedNode);
            GenDataDataGrid.DataSource = data.SelectedNode != null ? data.SelectedNode.Fields : null;
            RaiseDataChanged();
        }

        private void RaiseDataChanged()
        {
            if (OnDataChanged != null)
                OnDataChanged();
        }

        private void GenDataEditor_Load(object sender, System.EventArgs e)
        {
            DataNavigatorTreeView.Nodes.Clear();
            var builder = new DataEditorTreeViewBuilder(GenDataEditorViewModel.Data);
            builder.CreateSubClassTrees(DataNavigatorTreeView.Nodes, 0);
            GenDataEditorViewModel.Data.GenData.PropertyChanged += GenData_PropertyChanged;

        }

        private void GenData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseDataChanged();
        }
    }
}
