using System;
using System.ComponentModel;
using System.Windows.Forms;
using GenEdit.Utilities;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;

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
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null) return;
            
            var builder = new DataEditorTreeViewBuilder(GenDataEditorViewModel.Data);
            builder.CreateSubClassTrees(DataNavigatorTreeView.Nodes, 0);
            GenDataEditorViewModel.Data.GenData.PropertyChanged += GenData_PropertyChanged;
        }

        private void GenData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseDataChanged();
        }

        private void GenDataDataGrid_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            var header = e.Column.DataPropertyName;
            if (header != "Name" && header != "Value")
                e.Column.Visible = false;
            else if (header == "Name")
            {
                e.Column.ReadOnly = true;
                e.Column.Width = 150;
            }
            else if (header == "Value")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                e.Column.MinimumWidth = 200;
            }
        }

        private void GenDataDataGrid_SelectionChanged(object sender, System.EventArgs e)
        {
            if (GenDataDataGrid.CurrentRow == null)
            {
                DataEditorHintLabel.Text = "";
                return;
            }
            var data = (GenObjectFieldViewModel) GenDataDataGrid.CurrentRow.DataBoundItem;
            DataEditorHintLabel.Text = data == null ? "" : data.Hint;
        }

        private void MoveToTopButton_Click(object sender, System.EventArgs e)
        {
            MoveItem(ListMove.ToTop);
        }

        private void MoveItem(ListMove move)
        {
            var myNode = DataNavigatorTreeView.SelectedNode;
            var nodeData = (GenObjectViewModel) myNode.Tag;
            if (nodeData == null) return;
            var index = myNode.Parent.Nodes.IndexOf(myNode);
            var genData = nodeData.GenAttributes.GenData;
            var classId = nodeData.GenAttributes.GenObject.ClassId;
            switch (move)
            {
                case ListMove.ToTop:
                    MoveToTop(index, myNode);
                    break;
                case ListMove.Up:
                    MoveUp(index, myNode);
                    break;
                case ListMove.Down:
                    MoveDown(index, myNode);
                    break;
                case ListMove.ToBottom:
                    MoveToBottom(index, myNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("move");
            }

            genData.Context[classId].MoveItem(move, index);
        }

        private void MoveToTop(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index <= 0 || index >= nodes.Count) return;
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Insert(0, node);
        }

        private void MoveUp(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index <= 0 || index >= nodes.Count) return;
            var node = nodes[index];
            nodes[index] = nodes[index - 1];
            nodes[index - 1] = node;
        }

        private void MoveDown(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index < 0 || index >= nodes.Count - 1) return;
            var genObject = nodes[index];
            nodes[index] = nodes[index + 1];
            nodes[index + 1] = genObject;
        }

        private void MoveToBottom(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index < 0 || index >= nodes.Count - 1) return;
            var genObject = nodes[index];
            nodes.RemoveAt(index);
            nodes.Add(genObject);
        }
    }
}
