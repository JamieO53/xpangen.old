﻿using System;
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

        public delegate void FocusChanged();

        public FocusChanged OnFocusChanged;

        public GenDataEditor()
        {
            InitializeComponent();
        }

        private void DataNavigatorTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            var data = GenDataEditorViewModel;
            var node = data.SelectedNode;
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
            var data = GenDataEditorViewModel;
            data.SelectedNode = DataEditorTreeViewBuilder.GetNodeData(DataNavigatorTreeView.SelectedNode);
            GenDataDataGrid.DataSource = data.SelectedNode != null ? data.SelectedNode.Fields : null;
            RaiseFocusChanged();
        }

        private void RaiseDataChanged()
        {
            if (DataNavigatorTreeView.SelectedNode != null && GenDataEditorViewModel.SelectedNode != null)
                DataNavigatorTreeView.SelectedNode.Text =
                    GenDataEditorViewModel.SelectedNode.GenAttributes.AsString("Name");
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

            var builder = new DataEditorTreeViewBuilder(GenDataEditorViewModel.Data);
            builder.CreateSubClassTrees(DataNavigatorTreeView.Nodes, null);
            GenDataEditorViewModel.Data.GenData.First(0);
            GenDataEditorViewModel.Data.GenData.GenDataBase.PropertyChanged += GenData_PropertyChanged;
            RaiseDataChanged();
        }

        private void SaveItemChangesButton_Click(object sender, EventArgs e)
        {
            if (!SaveEditorChanges()) return;
            RaiseDataChanged();
        }

        public bool SaveEditorChanges()
        {
            var data = GenDataEditorViewModel;
            var node = data.SelectedNode;
            if (node == null) return true;
            if (GenDataDataGrid.CurrentCell != null && GenDataDataGrid.CurrentCell.IsInEditMode)
                GenDataDataGrid.CommitEdit(0);

            if (!node.Changed) return false;
            node.Save();
            return true;
        }

        private void CancelItemChangesButton_Click(object sender, EventArgs e)
        {
            var data = GenDataEditorViewModel;
            var node = data.SelectedNode;
            if (node == null) return;
            if (GenDataDataGrid.CurrentCell.IsInEditMode)
                GenDataDataGrid.CommitEdit(0);

            if (node.Changed)
                node.Cancel();
            var save = GenDataDataGrid.DataSource;
            GenDataDataGrid.DataSource = null;
            GenDataDataGrid.DataSource = save;
        }
    }
}
