using System;
using System.IO;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Editor.Model;

namespace GenEdit.View
{
    public partial class GenLibrary : UserControl
    {
        public delegate void DataLoaded();

        public DataLoaded OnDataLoaded;

        private FileGroup Selected { get; set; }

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

        public void DataChanged()
        {
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null) return;

            var changed = GenDataEditorViewModel.Data.GenDataStore.Changed;
            comboBoxFileGroup.Enabled = !changed;
            comboBoxBaseFile.Enabled = !changed;
            textBoxFileName.Enabled = !changed;
            textBoxFilePath.Enabled = !changed;
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

            buttonClose.Enabled = false;
            buttonNew.Enabled = true;
            buttonOpen.Enabled = false;
            buttonClose.Enabled = false;
            buttonSave.Enabled = false;
            buttonSaveAs.Enabled = false;
            
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
            
            SetFileGroupFields(selected);

            buttonNew.Enabled = true;
            buttonOpen.Enabled = selected != data.Settings.FileGroup;
            buttonClose.Enabled = true;
            buttonSave.Enabled = data.Changed;
            buttonSaveAs.Enabled = true;
        }

        private void comboBoxBaseFile_SelectedValueChanged(object sender, EventArgs e)
        {
            comboBoxProfile.ResetText();
            comboBoxProfile.Items.Clear();
            
            var fileGroup = comboBoxFileGroup.SelectedItem as FileGroup;
            if (fileGroup == null) return;
            var selected = comboBoxBaseFile.SelectedItem as BaseFile;
            if (selected == null) return;

            var n = -1;
            for (var i = 0; i < selected.ProfileList.Count; i++)
            {
                comboBoxProfile.Items.Add(selected.ProfileList[i]);
                if (fileGroup.Profile == selected.ProfileList[i].Name)
                    n = i;
            }
            if (n != -1 && n != comboBoxProfile.SelectedIndex)
                comboBoxProfile.SelectedIndex = n;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            var data = GenDataEditorViewModel.Data;
            Selected = comboBoxFileGroup.SelectedItem as FileGroup;
            if (Selected == null) return;
            data.SetFileGroup(Selected.Name);

            buttonOpen.Enabled = false;
            buttonClose.Enabled = true;
            buttonSave.Enabled = data.Changed;
            buttonSaveAs.Enabled = true;

            RaiseDataLoaded();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            GenDataEditorViewModel.Data.SetFileGroup("");
            if (Selected == null) return;
            var data = GenDataEditorViewModel.Data;

            buttonNew.Enabled = true;
            buttonOpen.Enabled = Selected != data.Settings.FileGroup;
            buttonClose.Enabled = true;
            buttonSave.Enabled = data.Changed;
            buttonSaveAs.Enabled = true;

            RaiseDataLoaded();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            buttonClose_Click(sender, e);
            var data = GenDataEditorViewModel.Data;

            Selected = data.NewFileGroup();

            SetFileGroupFields(Selected);

            buttonNew.Enabled = true;
            buttonOpen.Enabled = true;
            buttonClose.Enabled = true;
            buttonSave.Enabled = true;
            buttonSaveAs.Enabled = true;

        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            SetFileGroupFields(Selected);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var data = GenDataEditorViewModel.Data;

            if (Selected != data.Settings.FileGroup)
            {
                var i = comboBoxFileGroup.Items.IndexOf(Selected);
                if (i != 0)
                {
                    if (i > 0)
                    {
                        comboBoxFileGroup.Items.RemoveAt(i);
                    }
                    else if (i == -1)
                    {
                        // todo: validate this stuff.
                        Selected.Name = Path.GetFileNameWithoutExtension(textBoxFileName.Text);
                        Selected.FileName = textBoxFileName.Text +
                                            (Path.GetExtension(textBoxFileName.Text) == "" ? ".dcb" : "");
                        Selected.FilePath = textBoxFilePath.Text;
                        Selected.BaseFileName = ((BaseFile)comboBoxBaseFile.SelectedItem).Name;
                        Selected.Profile = comboBoxProfile.SelectedText;
                        Selected.Generated = textBoxGenerated.Text;
                        data.CreateFile(Selected);
                    }
                    comboBoxFileGroup.Items.Insert(0, Selected);
                    Background = true;
                    comboBoxFileGroup.SelectedIndex = 0;
                    Background = false;
                    if (i == -1)
                        buttonOpen_Click(sender, e);
                }
            }
        }

        private void SetFileGroupFields(FileGroup selected)
        {
            comboBoxFileGroup.ResetText();
            comboBoxFileGroup.SelectedItem = selected;
            textBoxFileName.Text = selected.FileName;
            textBoxFilePath.Text = selected.FilePath;
            textBoxGenerated.Text = selected.Generated;
            var settings = GenDataEditorViewModel.Data.Settings;
            comboBoxBaseFile.SelectedIndex =
                selected.BaseFileName == ""
                    ? comboBoxBaseFile.Items.IndexOf(settings.FindBaseFile("Definition"))
                    : comboBoxBaseFile.Items.IndexOf(settings.FindBaseFile(selected.BaseFileName));
        }
    }
}
