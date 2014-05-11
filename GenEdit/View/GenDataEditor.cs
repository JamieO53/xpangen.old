using System;
using System.ComponentModel;
using System.Windows.Forms;
using GenEdit.Controls;
using GenEdit.Utilities;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;

namespace GenEdit.View
{
    public partial class GenDataEditor : UserControl
    {
        public GenDataEditorViewModel GenDataEditorViewModel { get; set; }

        public delegate void DataChanged();
        public DataChanged OnDataChanged;

        public delegate void FocusChanged();
        public FocusChanged OnFocusChanged;

        private int NameColumnIndex { get; set; }
        private int ValueColumnIndex { get; set; }

        public GenDataEditor()
        {
            InitializeComponent();
        }

        private void DataNavigatorTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (DataNavigatorTreeView.SelectedNode == null) return;

            var node = DataEditorTreeViewBuilder.GetNodeData(DataNavigatorTreeView.SelectedNode);
            if (node != null && (node.IsNew && !node.Changed))
            {
                RemoveNewNode();
                return;
            }

            if (node == null || !node.Changed) return;

            var mr = MessageBox.Show("Data editor - data changed", "Do you wish to save the changes?",
                         MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                         MessageBoxDefaultButton.Button1);
            switch (mr)
            {
                case DialogResult.Yes:
                    node.Save();
                    break;
                case DialogResult.No:
                    node.Cancel();
                    break;
                default:
                    e.Cancel = true;
                    break;
            }

        }

        private void DataNavigatorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedItem = DataNavigatorTreeView.SelectedNode;
            var nodeData = selectedItem != null ? selectedItem.Tag as GenDataViewModelBase : null;
            if (nodeData != null)
            {
                nodeData.EstablishContext();
                GenDataDataGrid.DataSource = nodeData.Fields;
            }
            else GenDataDataGrid.DataSource = null;
            RaiseFocusChanged();
        }

        private void RaiseDataChanged()
        {
            var nodeData = DataEditorTreeViewBuilder.GetNodeData(DataNavigatorTreeView.SelectedNode);
            if (DataNavigatorTreeView.SelectedNode != null && nodeData != null)
                DataNavigatorTreeView.SelectedNode.Text =
                    nodeData.GenAttributes.AsString("Name");
            if (OnDataChanged != null)
                OnDataChanged();
        }

        private void RaiseFocusChanged()
        {
            if (OnFocusChanged != null)
                OnFocusChanged();
        }

        private void GenDataEditor_Load(object sender, EventArgs e)
        {
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
                e.Column.Width = 120;
                NameColumnIndex = e.Column.Index;
            }
            else if (header == "Value")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                e.Column.MinimumWidth = 200;
                ValueColumnIndex = e.Column.Index;
           }
        }

        private void GenDataDataGrid_SelectionChanged(object sender, System.EventArgs e)
        {
            if (GenDataDataGrid.CurrentRow == null)
            {
                DataEditorHintLabel.Text = "";
                return;
            }
            var data = (FieldViewModelBase) GenDataDataGrid.CurrentRow.DataBoundItem;
            DataEditorHintLabel.Text = data == null ? "" : data.Hint;
            var valueCell = GenDataDataGrid.CurrentRow.Cells[ValueColumnIndex];
            GenDataDataGrid.CurrentCell = valueCell;
        }

        private void MoveToTopButton_Click(object sender, System.EventArgs e)
        {
            MoveItem(ListMove.ToTop);
        }

        private void MoveUpButton_Click(object sender, System.EventArgs e)
        {
            MoveItem(ListMove.Up);
        }

        private void MoveDownButton_Click(object sender, System.EventArgs e)
        {
            MoveItem(ListMove.Down);
        }

        private void MoveToBottomButton_Click(object sender, System.EventArgs e)
        {
            MoveItem(ListMove.ToBottom);
        }

        private void MoveItem(ListMove move)
        {
            var myNode = DataNavigatorTreeView.SelectedNode;
            var nodeData = (GenObjectViewModel) myNode.Tag;
            if (nodeData == null) return;
            var index = myNode.Parent.Nodes.IndexOf(myNode);
            var genData = nodeData.SavedContext.GenData;
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
            DataNavigatorTreeView.BeginUpdate();
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Insert(0, node);
            DataNavigatorTreeView.SelectedNode = node;
            DataNavigatorTreeView.EndUpdate();
            RaiseFocusChanged();
        }

        private void MoveUp(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index <= 0 || index >= nodes.Count) return;
            DataNavigatorTreeView.BeginUpdate();
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Insert(index - 1, node);
            DataNavigatorTreeView.SelectedNode = node;
            DataNavigatorTreeView.EndUpdate();
            RaiseFocusChanged();
        }

        private void MoveDown(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index < 0 || index >= nodes.Count - 1) return;
            DataNavigatorTreeView.BeginUpdate();
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Insert(index + 1, node);
            DataNavigatorTreeView.SelectedNode = node;
            DataNavigatorTreeView.EndUpdate();
            RaiseFocusChanged();
        }

        private void MoveToBottom(int index, TreeNode myNode)
        {
            var nodes = myNode.Parent.Nodes;
            if (index < 0 || index >= nodes.Count - 1) return;
            DataNavigatorTreeView.BeginUpdate();
            var node = nodes[index];
            nodes.RemoveAt(index);
            nodes.Add(node);
            DataNavigatorTreeView.SelectedNode = node;
            DataNavigatorTreeView.EndUpdate();
            RaiseFocusChanged();
        }

        public void LoadData()
        {
            DataNavigatorTreeView.Nodes.Clear();
            GenDataDataGrid.DataSource = null;
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null ||
                GenDataEditorViewModel.Data.GenData == null) 
                return;

            //DataEditorTreeViewBuilder = new DataEditorTreeViewBuilder(GenDataEditorViewModel.Data);
            var saveCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            DataNavigatorTreeView.BeginUpdate();
            try
            {
                DataNavigatorTreeView.Nodes.Add(
                    new SubClassTreeNode(null, GenDataEditorViewModel.Data.GenData,
                                         new Definition(GenDataEditorViewModel.Data.DefGenData), 1));
                //DataEditorTreeViewBuilder.CreateSubClassTrees(DataNavigatorTreeView.Nodes, null);
            }
            finally
            {
                DataNavigatorTreeView.EndUpdate();
                Cursor.Current = saveCursor;
            }
            
            GenDataEditorViewModel.Data.GenData.First(0);
            GenDataEditorViewModel.Data.GenData.GenDataBase.PropertyChanged += GenData_PropertyChanged;
            RaiseDataChanged();
        }

        //private DataEditorTreeViewBuilder DataEditorTreeViewBuilder { get; set; }

        private void SaveItemChangesButton_Click(object sender, EventArgs e)
        {
            if (!SaveEditorChanges()) return;
            RaiseDataChanged();
        }

        public bool SaveEditorChanges()
        {
            var nodeData = DataEditorTreeViewBuilder.GetNodeData(DataNavigatorTreeView.SelectedNode);
            if (nodeData == null) return true;
            if (GenDataDataGrid.CurrentCell != null && GenDataDataGrid.CurrentCell.IsInEditMode)
                GenDataDataGrid.CommitEdit(0);

            if (!nodeData.Changed) return false;
            nodeData.Save();
            return true;
        }

        private void CancelItemChangesButton_Click(object sender, EventArgs e)
        {
            var nodeData = DataEditorTreeViewBuilder.GetNodeData(DataNavigatorTreeView.SelectedNode);
            if (nodeData == null) return;
            if (GenDataDataGrid.CurrentCell.IsInEditMode)
                GenDataDataGrid.CommitEdit(0);

            if (nodeData.IsNew)
            {
                RemoveNewNode();
                return;
            }

            if (nodeData.Changed)
                nodeData.Cancel();
            var save = GenDataDataGrid.DataSource;
            GenDataDataGrid.DataSource = null;
            GenDataDataGrid.DataSource = save;
        }

        private void RemoveNewNode()
        {
            var selectedNode = DataNavigatorTreeView.SelectedNode;
            var nodeData = DataEditorTreeViewBuilder.GetNodeData(selectedNode);
            var genObject = nodeData.GenAttributes.GenObject;
            genObject.ParentSubClass.Remove(genObject);
            selectedNode.Parent.Nodes.Remove(selectedNode);
        }

        private void AddItemButton_Click(object sender, EventArgs e)
        {
            if (DataNavigatorTreeView.SelectedNode == null)
                MessageBox.Show("Select a class before adding a new item", "Generator data error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            var node = DataNavigatorTreeView.SelectedNode as DataEditorTreeNodeBase;
            if (node != null)
            {
                ClassTreeNode newNode = node.AddNewNode();
                DataNavigatorTreeView.SelectedNode = newNode;
            }
            //DataEditorTreeViewBuilder.CreateNewChildItem(DataNavigatorTreeView);
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            var selectedNode = DataNavigatorTreeView.SelectedNode;
            var data = GenDataEditorViewModel;
            var nodeData = DataEditorTreeViewBuilder.GetNodeData(selectedNode);
            var genObject = nodeData.GenAttributes.GenObject;
            data.Data.GenData.Context[genObject.ClassId].Delete();
            selectedNode.Parent.Nodes.Remove(selectedNode);
            RaiseDataChanged();
        }

        private void GenDataDataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in GenDataDataGrid.Rows)
            {
                var field = row.DataBoundItem as GenObjectFieldViewModel;
                if (field == null) return;
                if (field.ComboValues != null)
                {
                    row.Cells[ValueColumnIndex] = new DataGridViewComboBoxCell
                                                      {
                                                          DataSource=field.ComboValues,
                                                          DisplayMember = "DisplayValue",
                                                          ValueMember = "DataValue"
                                                      };
                }
            }
        }

        private void GenDataDataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // to do
        }
    }
}
