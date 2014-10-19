// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Windows.Forms;
using GenEdit.Utilities;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Profile;

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

            var newFragment = ProfileEditorTreeViewBuilder.GetNodeData(e.Node);
            if (newFragment == GenDataEditorViewModel.Data.Profile.Fragment) return;
            RefreshProfile(GenDataEditorViewModel.Data.GenDataBase, GenDataEditorViewModel.Data.GenObject);
        }

        private void GenProfileEditor_Load(object sender, EventArgs e)
        {
        }

        public void LoadData()
        {
            ProfileNavigatorTreeView.Nodes.Clear();
            ProfileExpansionTextBox.Clear();
            ProfileTextBox.Clear();
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.ProfileIsUndefined())
                return;

            var data = GenDataEditorViewModel.Data;
            data.Profile.Fragment = data.Profile.Profile;
            var builder = new ProfileEditorTreeViewBuilder(data);

            IsBuilding = true;
            builder.CreateBodyChildTrees(ProfileNavigatorTreeView.Nodes, data.Profile.GetBody());
            IsBuilding = false;
            if (ProfileNavigatorTreeView.Nodes.Count > 0)
                ProfileNavigatorTreeView.SelectedNode = ProfileNavigatorTreeView.Nodes[0];
        }

        public void RefreshProfile(GenDataBase genDataBase, GenObject genObject)
        {
            var selectedItem = ProfileNavigatorTreeView.SelectedNode;
            var dataProfile = GenDataEditorViewModel.Data.Profile;
            dataProfile.Fragment = ProfileEditorTreeViewBuilder.GetNodeData(selectedItem);
            var text = dataProfile.GetNodeProfileText();

            // Don't change to prevent unnecessary rendering and side effects
            if (text != ProfileTextBox.Text)
            {
                var start = ProfileTextBox.SelectionStart;
                ProfileTextBox.Clear();
                ProfileTextBox.Text = text;
                ProfileTextBox.SelectionStart = start;
            }


            text = dataProfile.GetNodeExpansionText(genDataBase, genObject);

            // Don't change to prevent unnecessary rendering and side effects
            if (text != ProfileExpansionTextBox.Text)
            {
                var start = ProfileExpansionTextBox.SelectionStart;
                ProfileExpansionTextBox.Clear();
                ProfileExpansionTextBox.Text = text;
                ProfileExpansionTextBox.SelectionStart = start;
            }
        }
    }
}