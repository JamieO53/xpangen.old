using System;
using System.IO;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Model;

namespace GenEdit.View
{
    public partial class GenLibrary : UserControl
    {
        public delegate void DataLoaded();

        public DataLoaded OnDataLoaded;

        public delegate void ProfileChanged();

        public ProfileChanged OnProfileChanged;

        private FileGroup Selected { get; set; }

        public GenLibrary()
        {
            InitializeComponent();
            fileGroupUserControl1.OnProfileSelected += FileGroupProfileSelected;
        }

        public GenDataEditorViewModel GenDataEditorViewModel { get; set; }

        private bool Background { get; set; }

        private void RaiseDataLoaded()
        {
            if (OnDataLoaded != null)
                OnDataLoaded();
        }

        private void RaiseProfileChanged()
        {
            if (OnProfileChanged != null)
                OnProfileChanged();
        }

        public void DataChanged()
        {
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null) return;

            var changed = GenDataEditorViewModel.Data.GenDataStore.Changed;
            EnableControls(newEnabled: false, closeEnabled: true, saveEnabled: changed,
                           saveAsEnabled: true, fileGroupEnabled: false, generateEnabled: comboBoxProfile.SelectedIndex != -1);
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

            EnableControls(newEnabled: true, closeEnabled: false, saveEnabled: false,
                           saveAsEnabled: false, fileGroupEnabled: true, generateEnabled: false);
            
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
            
            data.SetFileGroup(selected.Name);

            SetFileGroupFields(selected);

            EnableControls(newEnabled: false, closeEnabled: true, saveEnabled: false,
                           saveAsEnabled: false, fileGroupEnabled: false, generateEnabled: comboBoxProfile.SelectedIndex != -1);
            fileGroupUserControl1.ViewModel = selected;
            RaiseDataLoaded();
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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            GenDataEditorViewModel.Data.SetFileGroup("");
            comboBoxFileGroup.SelectedIndex = -1;
            ClearFileGroupFields();

            EnableControls(newEnabled: true, closeEnabled: false, saveEnabled: false,
                           saveAsEnabled: false, fileGroupEnabled: true, generateEnabled: false);

            RaiseDataLoaded();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            buttonClose_Click(sender, e);
            var data = GenDataEditorViewModel.Data;

            Selected = data.NewFileGroup();

            SetFileGroupFields(Selected);

            EnableControls(newEnabled: false, closeEnabled: false, saveEnabled: true,
                           saveAsEnabled: false, fileGroupEnabled: false, generateEnabled: false);
        }

        private void EnableControls(bool newEnabled, bool closeEnabled, bool saveEnabled, bool saveAsEnabled,
                                    bool fileGroupEnabled, bool generateEnabled)
        {
            comboBoxFileGroup.Enabled = fileGroupEnabled;
            buttonNew.Enabled = newEnabled;
            buttonClose.Enabled = closeEnabled;
            buttonSave.Enabled = saveEnabled;
            buttonSaveAs.Enabled = saveAsEnabled;
            buttonGenerate.Enabled = generateEnabled;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var i = SaveOrCreateFile();

            if (i != 0)
            {
                FileGroup selected;

                if (i > 0)
                {
                    selected = comboBoxFileGroup.SelectedItem as FileGroup;
                    comboBoxFileGroup.Items.RemoveAt(i);
                }
                else
                    selected = Selected;

                comboBoxFileGroup.Items.Insert(0, selected);
                Background = true;
                comboBoxFileGroup.SelectedIndex = 0;
                Background = false;
                if (i == -1 && selected != null)
                {
                    GenDataEditorViewModel.Data.SetFileGroup(selected.Name);

                    RaiseDataLoaded();
                }
                return;
            }

            EnableControls(newEnabled: false, closeEnabled: true, saveEnabled: false,
                            saveAsEnabled: true, fileGroupEnabled: false, generateEnabled: comboBoxProfile.SelectedIndex != -1);
        }

        public int SaveOrCreateFile()
        {
            var i = comboBoxFileGroup.SelectedIndex;
            if (comboBoxFileGroup.SelectedItem as FileGroup == null)
            {
                var selected = Selected;
                selected.Name = Path.GetFileNameWithoutExtension(textBoxFileName.Text);
                selected.FileName = textBoxFileName.Text +
                                    (Path.GetExtension(textBoxFileName.Text) == "" ? ".dcb" : "");
                selected.FilePath = textBoxFilePath.Text;
                selected.BaseFileName = ((BaseFile) comboBoxBaseFile.SelectedItem).Name;
                selected.Profile = comboBoxProfile.SelectedText;
                selected.Generated = textBoxGenerated.Text;
                GenDataEditorViewModel.Data.CreateFile(selected);
            }
            else
                GenDataEditorViewModel.Data.SaveFile(comboBoxFileGroup.SelectedItem as FileGroup);
            return i;
        }

        private void SetFileGroupFields(FileGroup selected)
        {
            comboBoxFileGroup.ResetText();
            comboBoxFileGroup.SelectedItem = selected;
            textBoxFileName.Text = selected != null ? selected.FileName : "";
            textBoxFilePath.Text = selected != null ? selected.FilePath : "";
            textBoxGenerated.Text = selected != null ? selected.Generated : "";
            var settings = GenDataEditorViewModel.Data.Settings;
            comboBoxBaseFile.SelectedIndex =
                selected == null || selected.BaseFileName == ""
                    ? comboBoxBaseFile.Items.IndexOf(settings.FindBaseFile("Definition"))
                    : comboBoxBaseFile.Items.IndexOf(settings.FindBaseFile(selected.BaseFileName));
        }

        private void ClearFileGroupFields()
        {
            comboBoxFileGroup.ResetText();
            comboBoxFileGroup.SelectedItem = null;
            textBoxFileName.Text = "";
            textBoxFilePath.Text = "";
            textBoxGenerated.Text = "";
            comboBoxBaseFile.SelectedIndex = -1;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            var data = GenDataEditorViewModel.Data;
            var saveCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var done = false;
                while (!done)
                    try
                    {
                        data.Generate();
                        done = true;
                    }
                    catch (GeneratorException exception)
                    {
                        if (exception.GenErrorType != GenErrorType.NoOutputFile)
                            throw;
                        saveGeneratedFileDialog.InitialDirectory = data.Settings.HomeDir == "."
                                                                       ? Directory.GetCurrentDirectory()
                                                                       : data.Settings.HomeDir;
                        switch (saveGeneratedFileDialog.ShowDialog())
                        {
                            case DialogResult.OK:
                                data.Settings.Generated =
                                    saveGeneratedFileDialog.FileName
                                                           .Replace(saveGeneratedFileDialog.InitialDirectory + "\\", "")
                                                           .Replace('\\', '/');
                                data.SaveFile(data.Settings.FileGroup);
                                textBoxGenerated.Text = data.Settings.Generated;
                                break;
                            case DialogResult.Cancel:
                                done = true;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
            }
            finally
            {
                Cursor.Current = saveCursor;
            }
        }

        private void comboBoxProfile_SelectedValueChanged(object sender, EventArgs e)
        {
            var data = GenDataEditorViewModel.Data;
            buttonGenerate.Enabled = comboBoxProfile.SelectedItem != null;
            data.SetProfile((Profile) comboBoxProfile.SelectedItem);
            RaiseProfileChanged();
        }

        private void FileGroupProfileSelected()
        {
            var data = GenDataEditorViewModel.Data;
            buttonGenerate.Enabled = fileGroupUserControl1.Profile != null;
            data.SetProfile(fileGroupUserControl1.Profile);
            RaiseProfileChanged();
        }
    }
}
