using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Editor.Model;

namespace GenEdit.View
{
    public partial class GenLibrary : UserControl
    {
        public delegate void DataLoaded();

        public DataLoaded OnDataLoaded;

        public GenLibrary()
        {
            InitializeComponent();
        }

        public GenDataEditorViewModel GenDataEditorViewModel { get; set; }

        private bool Background { get; set; }

        private void RaiseDataLoaded()
        {
            if (OnDataLoaded != null)
                OnDataLoaded();
        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            var s = sender as Panel;
            var w = s.Width - 25;
            comboBoxFileGroup.Width = w;
            comboBoxBaseFile.Width = w;
            comboBoxProfile.Width = w;
            textBoxFileName.Width = w;
            textBoxFilePath.Width = w;
            textBoxGenerated.Width = w;
        }

        private void GenLibrary_Load(object sender, EventArgs e)
        {
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null) return;

            var data = GenDataEditorViewModel.Data;
            comboBoxFileGroup.Items.Clear();
            comboBoxFileGroup.DisplayMember = "Name";
            comboBoxBaseFile.Items.Clear();
            comboBoxBaseFile.DisplayMember = "Name";
            comboBoxProfile.DisplayMember = "Name";
            
            var fileGroups = data.Settings.GetFileGroups();
            for (var i = 0; i < fileGroups.Count; i++)
                comboBoxFileGroup.Items.Add(fileGroups[i]);
            var baseFiles = data.Settings.GetBaseFiles();
            for (var i = 0; i < baseFiles.Count; i++)
                comboBoxBaseFile.Items.Add(baseFiles[i]);
        }

        private void comboBoxFileGroup_SelectedValueChanged(object sender, EventArgs e)
        {
            if (Background) return;
            var selected = comboBoxFileGroup.SelectedItem as FileGroup;
            if (selected == null) return;
            var data = GenDataEditorViewModel.Data;
            
            if (selected != data.Settings.FileGroup)
            {
                data.SetFileGroup(selected.Name);
                if (comboBoxFileGroup.SelectedIndex != 0)
                {
                    comboBoxFileGroup.Items.RemoveAt(comboBoxFileGroup.SelectedIndex);
                    comboBoxFileGroup.Items.Insert(0, selected);
                    Background = true;
                    comboBoxFileGroup.SelectedIndex = 0;
                    Background = false;
                }
            }
            textBoxFileName.Text = selected.FileName;
            textBoxFilePath.Text = selected.FilePath;
            textBoxGenerated.Text = selected.Generated;
            comboBoxBaseFile.SelectedIndex = comboBoxBaseFile.Items.IndexOf(data.Settings.BaseFile);
            RaiseDataLoaded();
        }

        private void comboBoxBaseFile_SelectedValueChanged(object sender, EventArgs e)
        {
            var selected = comboBoxFileGroup.SelectedItem as FileGroup;
            if (selected == null) return;
            var data = GenDataEditorViewModel.Data;

            var n = -1;
            comboBoxProfile.ResetText();
            comboBoxProfile.Items.Clear();
            for (var i = 0; i < data.Settings.BaseFile.ProfileList.Count; i++)
            {
                comboBoxProfile.Items.Add(data.Settings.BaseFile.ProfileList[i]);
                if (selected.Profile == data.Settings.BaseFile.ProfileList[i].Name)
                    n = i;
            }
            if (n != -1 && n != comboBoxProfile.SelectedIndex)
                comboBoxProfile.SelectedIndex = n;
        }
    }
}
