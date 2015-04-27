// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Settings;
using org.xpangen.Generator.Editor.Helper;

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
            fileGroupUserControl1.ProfileSelected += FileGroupProfileSelected;
        }

        public GenDataEditorViewModel GenDataEditorViewModel { private get; set; }

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
            EnableControls(false, true, changed, true, false, ProfileIsSpecified());
        }

        private bool ProfileIsSpecified()
        {
            return fileGroupUserControl1.Profile != null;
        }

        private void GenLibrary_Load(object sender, EventArgs e)
        {
            if (GenDataEditorViewModel == null || GenDataEditorViewModel.Data == null) return;

            var data = GenDataEditorViewModel.Data;

            EnableControls(true, false, false, false, true, false);
            
            comboBoxFileGroup.Items.Clear();
            comboBoxFileGroup.DisplayMember = "Name";
            var fileGroups = data.Settings.GetFileGroups();
            foreach (var fileGroup in fileGroups)
                comboBoxFileGroup.Items.Add(fileGroup);
        }

        private void comboBoxFileGroup_SelectedValueChanged(object sender, EventArgs e)
        {
            if (Background) return;
            var selected = comboBoxFileGroup.SelectedItem as FileGroup;

            if (selected == null)
            {
                fileGroupUserControl1.FileGroup = null;
                return;
            }
            var data = GenDataEditorViewModel.Data;

            if (!GeData.CheckIfDataExists(Path.Combine(selected.FilePath, selected.FileName)))
            {
                var msg = MessageBox.Show("Remove the file group?", "The data file does not exist",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                var savedBackground = Background;
                Background = true;
                if (msg == DialogResult.Yes)
                {
                    data.RemoveFileGroup(selected.Name);
                    comboBoxFileGroup.Items.RemoveAt(comboBoxFileGroup.SelectedIndex);
                }
                comboBoxFileGroup.SelectedItem = null;
                Background = savedBackground;
                return;
            }
            data.SetFileGroup(selected.Name);

            EnableControls(false, true, false, false, false, ProfileIsSpecified());
            Loading = true;
            fileGroupUserControl1.FileGroup = selected;
            Loading = false;
            RaiseDataLoaded();
            FileGroupProfileSelected(sender, e);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            GenDataEditorViewModel.Data.SetFileGroup("");
            comboBoxFileGroup.SelectedIndex = -1;
            EnableControls(true, false, false, false, true, false);

            RaiseDataLoaded();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            buttonClose_Click(sender, e);
            var data = GenDataEditorViewModel.Data;

            Selected = data.NewFileGroup();
            fileGroupUserControl1.FileGroup = Selected;

            EnableControls(false, false, true, false, false, false);
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

            EnableControls(false, true, false, true, false, ProfileIsSpecified());
        }

        public int SaveOrCreateFile()
        {
            var i = comboBoxFileGroup.SelectedIndex;
            if (comboBoxFileGroup.SelectedItem as FileGroup == null)
            {

                fileGroupUserControl1.SaveChanges();
                var data = GenDataEditorViewModel.Data;
                var extension = data.Settings.FindBaseFile(Selected.BaseFileName).FileExtension;
                if (extension == "")
                    extension = ".dcb";
                else if (extension[0] != '.')
                    extension = "." + extension;
                if (Selected.Name == "")
                    Selected.Name = Path.ChangeExtension(Selected.FileName, "").Replace(".", "");
                if (Selected.FileName == "")
                    Selected.FileName = Path.ChangeExtension(Selected.Name, ".");
                if (Path.GetExtension(Selected.FileName) == "")
                    Selected.FileName = Path.ChangeExtension(Selected.FileName, extension);
                if (Selected.Name == "")
                {
                    MessageBox.Show("Specify the file name before saving the file", "Saving a new file");
                    return i;
                }

                GenDataEditorViewModel.Data.CreateFile(Selected);
            }
            else
                GenDataEditorViewModel.Data.SaveFile(comboBoxFileGroup.SelectedItem as FileGroup);
            return i;
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
                                data.Settings.GeneratedFile =
                                    saveGeneratedFileDialog.FileName
                                                           .Replace(saveGeneratedFileDialog.InitialDirectory + "\\", "")
                                                           .Replace('\\', '/');
                                data.SaveFile(data.Settings.FileGroup);
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

        private void FileGroupProfileSelected(object sender, EventArgs eventArgs)
        {
            var data = GenDataEditorViewModel.Data;
            buttonGenerate.Enabled = ProfileIsSpecified();
            data.SetProfile(fileGroupUserControl1.Profile);
            if (!Loading)
                RaiseProfileChanged();
        }

        private bool Loading { get; set; }
    }
}
