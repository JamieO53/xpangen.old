using System;
using System.ComponentModel;
using System.Windows.Forms;
using GenEdit.Controls;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;

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

        private GenDataViewModelBase NodeData
        {
            get { return GetNodeData(DataNavigatorTreeView.SelectedNode); }
        }

        private void DataNavigatorTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (DataNavigatorTreeView.SelectedNode == null) return;

            var node = NodeData;
            if (node != null && (node.IsNew && !node.Changed))
            {
                RemoveNewNode();
                return;
            }

            if (node == null || !node.Changed) return;

            if (node.IsNew) GenDataEditorViewModel.Data.Changed = true;


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

        public static GenDataViewModelBase GetNodeData(TreeNode selectedItem)
        {
            return GenDataViewModelBase.GetNodeData(selectedItem);
        }

        private void DataNavigatorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // See http://stackoverflow.com/questions/569673/treeview-re-grabs-focus-on-ctrlclick for this trick
            BeginInvoke(new TreeViewEventHandler(DelayedClick), sender, e);
        }

        private void DelayedClick(object sender, TreeViewEventArgs e)
        {
            var nodeData = NodeData;
            if (nodeData != null)
            {
                GenDataDataGrid.DataSource = nodeData.Fields;
                splitContainer1.ActiveControl = GenDataDataGrid;
                GenDataEditorViewModel.Data.GenObject = nodeData.GenObject;
            }
            else
            {
                GenDataDataGrid.DataSource = null;
                GenDataEditorViewModel.Data.GenObject = null;
            }
            RaiseFocusChanged();
        }

        private void RaiseDataChanged()
        {
            var nodeData = NodeData;
            if (nodeData != null)
                DataNavigatorTreeView.SelectedNode.Text =
                    nodeData.Name;
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
            GenDataDataGrid.CurrentCell.Selected = true;
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
            var myNode = DataNavigatorTreeView.SelectedNode as ClassTreeNode;
            if (myNode != null && myNode.MoveItem(move)) RaiseFocusChanged();
        }

        public void LoadData()
        {
            DataNavigatorTreeView.Nodes.Clear();
            GenDataDataGrid.DataSource = null;
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null ||
                GenDataEditorViewModel.Data.GenDataBase == null) 
                return;

            var saveCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            DataNavigatorTreeView.BeginUpdate();
            try
            {
                DataNavigatorTreeView.Nodes.Add(CreateRootNode());
            }
            finally
            {
                DataNavigatorTreeView.EndUpdate();
                Cursor.Current = saveCursor;
            }
            
            GenDataEditorViewModel.Data.GenDataBase.PropertyChanged += GenData_PropertyChanged;
            RaiseDataChanged();
        }

        private SubClassTreeNode CreateRootNode()
        {
            var data = GenDataEditorViewModel.Data;
            return ClassTreeNode.CreateRootNode(data);
        }

        private void SaveItemChangesButton_Click(object sender, EventArgs e)
        {
            if (!SaveEditorChanges()) return;
            RaiseDataChanged();
        }

        public bool SaveEditorChanges()
        {
            var nodeData = NodeData;
            if (nodeData == null) return true;
            if (GenDataDataGrid.CurrentCell != null && GenDataDataGrid.CurrentCell.IsInEditMode)
                GenDataDataGrid.CommitEdit(0);

            if (!nodeData.Changed) return false;
            nodeData.Save();
            return true;
        }

        private void CancelItemChangesButton_Click(object sender, EventArgs e)
        {
            var nodeData = NodeData;
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
            var nodeData = (GenObjectViewModel)GetNodeData(selectedNode);
            var genObject = (GenObject) nodeData.GenAttributes.GenObject;
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
                var newNode = node.AddNewNode();
                if (newNode != null)
                    DataNavigatorTreeView.SelectedNode = newNode;
            }
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            var selectedNode = DataNavigatorTreeView.SelectedNode;
            var data = GenDataEditorViewModel;
            var nodeData = (GenObjectViewModel)GetNodeData(selectedNode);
            var genObject = (GenObject) nodeData.GenAttributes.GenObject;
            var parentList = genObject.ParentSubClass;
            parentList.Remove(genObject);
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
    }
}
