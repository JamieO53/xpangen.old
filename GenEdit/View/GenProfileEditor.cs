﻿// // This Source Code Form is subject to the terms of the Mozilla Public
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

            var contextData = GenDataEditorViewModel.Data.GenData.DuplicateContext();
            RefreshProfile(contextData, GenDataEditorViewModel.Data.GenObject);
        }

        private void GenProfileEditor_Load(object sender, EventArgs e)
        {
        }

        public void LoadData()
        {
            ProfileNavigatorTreeView.Nodes.Clear();
            ProfileExpansionTextBox.Clear();
            ProfileTextBox.Clear();
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null ||
                GenDataEditorViewModel.Data.Profile == null) return;

            var builder = new ProfileEditorTreeViewBuilder(GenDataEditorViewModel.Data);

            IsBuilding = true;
            builder.CreateBodyChildTrees(ProfileNavigatorTreeView.Nodes, GenDataEditorViewModel.Data.Profile);
            IsBuilding = false;
            RefreshProfile(GenDataEditorViewModel.Data.GenData, GenDataEditorViewModel.Data.GenObject);
            if (ProfileNavigatorTreeView.Nodes.Count > 0)
                ProfileNavigatorTreeView.SelectedNode = ProfileNavigatorTreeView.Nodes[0];
        }

        public void RefreshProfile(GenData genData, GenObject genObject)
        {
            var selectedItem = ProfileNavigatorTreeView.SelectedNode;
            GenFragment fragment1 = ProfileEditorTreeViewBuilder.GetNodeData(selectedItem);
            var text = ProfileEditorTreeViewBuilder.GetNodeData(selectedItem) != null
                ? ProfileEditorTreeViewBuilder.GetNodeProfileText(ProfileFragmentSyntaxDictionary
                        .ActiveProfileFragmentSyntaxDictionary, fragment1.Fragment)
                : "";

            // Don't change to prevent unnecessary rendering and side effects
            if (text != ProfileTextBox.Text)
            {
                ProfileTextBox.Clear();
                ProfileTextBox.Text = text;
            }


            var fragment = ProfileEditorTreeViewBuilder.GetNodeData(selectedItem);
            var context = fragment != null && (genObject ?? genData.Root) != null
                ? genData.GetContext(genObject ?? genData.Root, fragment.Fragment.ClassName())
                : null;
            text = context != null
                ? ProfileEditorTreeViewBuilder.GetNodeExpansionText(genData, context, fragment.Fragment)
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