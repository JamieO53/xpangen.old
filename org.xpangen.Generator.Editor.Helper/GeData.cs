// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Codes;
using org.xpangen.Generator.Data.Model.Settings;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeData
    {
        private readonly Stack<IGenUndoRedo> _undoStack = new Stack<IGenUndoRedo>();
        private readonly Stack<IGenUndoRedo> _redoStack = new Stack<IGenUndoRedo>();
        public bool Changed
        {
            get
            {
                return GenDataBase != null && GenDataBase.Changed;
            }
            set { if (GenDataBase != null) GenDataBase.Changed = value; }
        }

        public GenDataBase DefGenDataBase
        {
            get { return GenDataStore.DefGenDataBase; }
        }

        /// <summary>
        /// The open data file.
        /// </summary>
        public GenDataBase GenDataBase
        {
            get { return GenDataStore.GenDataBase; }
        }
        
        /// <summary>
        /// The open data file's definition.
        /// </summary>
        public GenDataDef GenDataDef
        {
            get { return GenDataBase != null ? GenDataBase.GenDataDef : null; }
        }

        public IGenDataProfile Profile { get; private set; } 
        public IGenData GenDataStore { get; private set; }
        public IGenDataSettings Settings { get; set; }

        public GeData()
        {
            GenDataStore = new GeGenData();
        }

        /// <summary>
        /// Set the current files from the selected file group.
        /// </summary>
        /// <param name="fileGroup">The name of the selected file group.</param>
        public void SetFileGroup(string fileGroup)
        {
            if (Settings.GetFileGroup(fileGroup) == null)
            {
                GenDataStore.SetData("");
                GenDataStore.SetBase("");
                Profile.Profile = null;
                return;
            }
            GenDataStore.SetBase(Settings.BaseFilePath);
            GenDataStore.SetData(Settings.FilePath);
            //SetProfile();
        }

        /// <summary>
        /// Create a new file group.
        /// </summary>
        /// <returns>The new file group.</returns>
        public FileGroup NewFileGroup()
        {
            var fileGroup = Settings.Model.GenSettingsList[0].AddFileGroup("", "", "", "Definition");
            Settings.FileGroup = fileGroup;
            return fileGroup;
        }

        public void SaveFile(FileGroup fileGroup)
        {
            fileGroup.SaveFields();
            var fileName = BuildFilePath(fileGroup.FilePath, fileGroup.FileName);
            GenParameters.SaveToFile(GenDataBase, fileName);

            if (Settings.FindFileGroup(fileGroup.Name) == null)
                Settings.GetFileGroups().Add(fileGroup);
            SetFileGroup(fileGroup.Name);
            SaveSettings();
        }

        /// <summary>
        /// Create a new file and save the file group.
        /// </summary>
        /// <param name="fileGroup">The file group defining the new file.</param>
        public void CreateFile(FileGroup fileGroup)
        {
            fileGroup.SaveFields();
            if (!File.Exists(BuildFilePath(fileGroup.FilePath, fileGroup.FileName).Replace('/', '\\')))
            {
                var dataDef = Settings.FindBaseFile(fileGroup.BaseFileName);
                var f = GenDataBase.DataLoader.LoadData(BuildFilePath(dataDef.FilePath, dataDef.FileName)).AsDef();
                var d = new GenDataBase(f);
                var fileName = BuildFilePath(fileGroup.FilePath, fileGroup.FileName);
                GenParameters.SaveToFile(d, fileName);
            }
            if (Settings.FindFileGroup(fileGroup.Name) == null)
                Settings.Model.GenSettingsList[0].AddFileGroup(fileGroup.Name, fileGroup.FileName, fileGroup.FilePath,
                    fileGroup.BaseFileName, fileGroup.Profile, fileGroup.GeneratedFile);

            if (fileGroup.BaseFileName == "Definition")
                AddBaseFile(fileGroup);
            
            SetFileGroup(fileGroup.Name);
            SaveSettings();
        }

        private void SaveSettings()
        {
            if ((SaveToDisk &&
                 (Settings.FileGroup == null || Settings.FileGroup.FileName == null ||
                  !Settings.FileGroup.FileName.Equals("Settings.dcb", StringComparison.InvariantCultureIgnoreCase))))
                GenParameters.SaveToFile(Settings.Model.GenDataBase, "Data/Settings.dcb");
        }

        private static string BuildFilePath(string filePath, string fileName)
        {
            return (filePath == "" ? "" : filePath + "/") + fileName;
        }

        public IGenDataSettings GetDefaultSettings()
        {
            var data = GenDataBase.DataLoader.LoadData("Settings");
            var settings = LoadSettingsFromData(data);
            settings.Check();
            SaveToDisk = true;
            return settings;
        }

        public IGenDataSettings GetDesignTimeSettings()
        {
            var r = new GeneratorEditor();
            var settings = new GeSettings(r);
            settings.Check();
            r.GenSettingsList[0].AddFileGroup("Definition", "Definition.dcb", "Data", "Definition");
            settings.FindFileGroup("Definition");
            return settings;
        }

        private IGenDataProfile GetDefaultProfile(GeData geData)
        {
            return new GeProfile(geData);
        }

        private IGenDataProfile GetDesignTimeProfile(GeData geData)
        {
            return new GeProfile(geData);
        }

        private ComboServer GetDefaultComboServer()
        {
            var data = GenDataBase.DataLoader.LoadData(GenDataBase.DataLoader.LoadData("CodesDefinition").AsDef(),
                                                   "Data/Standard Editor Codes.dcb");
            return new ComboServer(data);
        }

        public static ComboServer GetDesignTimeComboServer()
        {
            var r = new CodesDefinition();
            var t = r.AddCodesTable("YesNo", "Select True or False values");
            t.AddCode("Yes", "Yes", "True");
            t.AddCode("No", "No");
            t = r.AddCodesTable("DataType", "Select the editor field data type");
            t.AddCode("String", "String", "String");
            t.AddCode("Integer", "Integer", "Integer");
            t.AddCode("Boolean", "Boolean", "Boolean");
            t.AddCode("Identifier", "Identifier", "Identifier");
            return new ComboServer(r.GenDataBase);
        }

        private bool SaveToDisk { get; set; }

        public IGenDataSettings LoadSettingsFromData(GenDataBase data)
        {
            var model = new GeneratorEditor(data) {GenObject = data.Root};
            SaveToDisk = false;
            return new GeSettings(model);
        }

        private void AddBaseFile(FileGroup fileGroup)
        {
            Settings.AddBaseFile(fileGroup);
        }

        public ComboServer ComboServer { get; private set; }
        public GenObject GenObject { get; set; }

        /// <summary>
        /// Get values to populate a data editor combo.
        /// </summary>
        /// <param name="name">The name of the combo list.</param>
        /// <returns>The populated combo list, or null if no such list exists.</returns>
        public List<GeComboItem> GetCodesCombo(string name)
        {
            return ComboServer.GetComboItems(name);
        }

        public void Generate()
        {
            if (Settings.FileGroup.Profile == "") return;
            
            GenParameters d;
            var data = BuildFilePath(Settings.FileGroup.FilePath, Settings.FileGroup.FileName);
            using (var dataStream = new FileStream(data, FileMode.Open))
                d = new GenParameters(GenDataDef, dataStream) { DataName = Path.GetFileNameWithoutExtension(data) };
            var baseFile = Settings.GetBaseFiles().Find(Settings.FileGroup.BaseFileName);
            var profile = baseFile.ProfileList.Find(Settings.FileGroup.Profile);
            var profileFileName = BuildFilePath(profile.FilePath, profile.FileName);

            var p = new GenCompactProfileParser(GenDataDef, profileFileName, "") {GenObject = d.Root};
            using (var writer = new GenWriter(null) {FileName = Settings.GeneratedFile.Replace('/', '\\')})
                p.Generate(d, writer);
        }

        public void SetProfile(Data.Model.Settings.Profile profile)
        {
            Settings.Profile = profile != null ? profile.Name : "";
            Profile.LoadProfile(Settings.Profile, GenDataDef);
        }

        public static bool CheckIfDataExists(string value)
        {
            return File.Exists(GenParameters.GetFullPath(value));
        }


        public static GeData GetDefaultGeData(bool isInDesignMode = false)
        {
            var geData = new GeData();
            geData.Settings = isInDesignMode ? geData.GetDesignTimeSettings() : geData.GetDefaultSettings();
            geData.ComboServer = isInDesignMode ? GetDesignTimeComboServer() : geData.GetDefaultComboServer();
            geData.Profile = isInDesignMode ? geData.GetDesignTimeProfile(geData) : geData.GetDefaultProfile(geData);

            return geData;
        }

        public void Undo()
        {
            if (_undoStack.Count == 0) return;
            var undoRedo = _undoStack.Pop();
            _redoStack.Push(undoRedo);
            undoRedo.Undo();
        }

        public void Redo()
        {
            var undoRedo = _redoStack.Pop();
            if (_redoStack.Count == 0) return;
            _undoStack.Push(undoRedo);
            undoRedo.Redo();
        }

        public void AddRedoUndo(IGenUndoRedo item)
        {
            _undoStack.Push(item);
            _redoStack.Clear();
        }

        /// <summary>
        /// Remove the specified fileGroup
        /// </summary>
        /// <param name="fileGroup">The file group being removed</param>
        public void RemoveFileGroup(string fileGroup)
        {
            Settings.RemoveFileGroup(fileGroup);
            SaveSettings();
        }
    }
}