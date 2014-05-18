// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Codes;
using org.xpangen.Generator.Data.Model.Definition;
using org.xpangen.Generator.Data.Model.Settings;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeData
    {
        public bool Changed
        {
            get
            {
                return GenData != null && GenData.Changed;
            }
        }

        public GenData DefGenData
        {
            get { return GenDataStore.DefGenData; }
        }

        /// <summary>
        /// The open data file.
        /// </summary>
        public GenData GenData
        {
            get { return GenDataStore.GenData; }
        }
        
        /// <summary>
        /// The open data file's definition.
        /// </summary>
        public GenDataDef GenDataDef
        {
            get { return GenData != null ? GenData.GenDataDef : null; }
        }

        private GenList<Class> ClassList
        {
            get
            {
                var def = DefGenData ?? (GenDataDef != null ? GenDataDef.AsGenData() : null);
                if (def == null)
                    return null;
                
                var list = new GenList<Class>();
                AddClasses(def, list);
                var references = def.Cache.References;
                for (var i = 0; i < references.Count; i++)
                {
                    var reference = references[i];
                    AddClasses(reference.GenData, list);
                }
                return list;
            }
        }

        private static void AddClasses(GenData def, GenList<Class> list)
        {
            def.First(1);
            while (!def.Eol(1))
            {
                list.Add(new Class(def) {GenObject = def.Context[1].GenObject});
                def.Next(1);
            }
        }

        public GenSegment Profile { get; private set; } 
        public IGenData GenDataStore { get; private set; }
        public IGenDataSettings Settings { get; set; }
        public bool Testing { get; set; }
        public IGenValidator Validator { get; set; }

        public GeData()
        {
            GenDataStore = new GeGenData();
            Validator = new GeGridDataValidator(this);
        }
        public void GridKeyPress()
        {
            // todo: signature for key press event handler
            throw new NotImplementedException("GridKeyPress method not implemented");
        }

        /// <summary>
        /// Find the editor properties for the class.
        /// </summary>
        /// <param name="classId">The ID of the class.</param>
        /// <returns>The class definition.</returns>
        public Class FindClassDefinition(int classId)
        {
            var defClass = GenDataDef.Classes[classId];
            for (var i = 0; i < ClassList.Count; i++)
                if (ClassList[i].Name == defClass.Name)
                    return ClassList[i];
            return null;
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
                Profile = null;
                return;
            }
            GenDataStore.SetBase(Settings.BaseFilePath);
            GenDataStore.SetData(Settings.FilePath);
            SetProfile();
        }

        private void SetProfile()
        {
            Profile = Settings.Profile != "" ? new GenCompactProfileParser(GenData, Settings.Profile, "") : null;
        }

        /// <summary>
        /// Create a new file group.
        /// </summary>
        /// <returns>The new file group.</returns>
        public FileGroup NewFileGroup()
        {
            return new FileGroup(Settings.Model.GenData)
                       {
                           GenObject =
                               Settings.Model.GenData.CreateObject("GenSettings",
                                                                   "FileGroup"),
                           DelayedSave = true,
                           BaseFileName = "Definition"
                       };
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
                var f = GenData.DataLoader.LoadData(BuildFilePath(dataDef.FilePath, dataDef.FileName)).AsDef();
                var d = new GenData(f);
                GenParameters.SaveToFile(d, BuildFilePath(fileGroup.FilePath, fileGroup.FileName));
            }
            if (Settings.FindFileGroup(fileGroup.Name) == null)
                Settings.GetFileGroups().Add(fileGroup);
            SetFileGroup(fileGroup.Name);
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (SaveToDisk)
                GenParameters.SaveToFile(Settings.Model.GenData, "Data/Settings.dcb");
        }

        private static string BuildFilePath(string filePath, string fileName)
        {
            return (filePath == "" ? "" : filePath + "/") + fileName;
        }

        public IGenDataSettings GetDefaultSettings()
        {
            var data = GenData.DataLoader.LoadData("Settings");
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

        public ComboServer GetDefaultComboServer()
        {
            var data = GenData.DataLoader.LoadData(GenData.DataLoader.LoadData("CodesDefinition").AsDef(),
                                                   "Data/Standard Editor Codes.dcb");
            return new ComboServer(data);
        }

        public ComboServer GetDesignTimeComboServer()
        {
            var r = new CodesDefinition();
            var t = r.AddCodesTable("YesNo", "Select True or False values");
            t.AddCode("Yes", "Yes", "True");
            t.AddCode("No", "No", "");
            t = r.AddCodesTable("DataType", "Select the editor field data type");
            t.AddCode("String", "String", "String");
            t.AddCode("Integer", "Integer", "Integer");
            t.AddCode("Boolean", "Boolean", "Boolean");
            t.AddCode("Identifier", "Identifier", "Identifier");
            return new ComboServer(r.GenData);
        }

        protected bool SaveToDisk { get; set; }

        public IGenDataSettings LoadSettingsFromData(GenData data)
        {
            var model = new GeneratorEditor(data) {GenObject = data.Root};
            SaveToDisk = false;
            return new GeSettings(model);
        }

        public void SaveFile(FileGroup fileGroup)
        {
            fileGroup.SaveFields();
            GenParameters.SaveToFile(GenData, BuildFilePath(fileGroup.FilePath, fileGroup.FileName));

            if (Settings.FindFileGroup(fileGroup.Name) == null)
                Settings.GetFileGroups().Add(fileGroup);
            SetFileGroup(fileGroup.Name);
            SaveSettings();
        }

        public ComboServer ComboServer { get; set; }
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
                d = new GenParameters(dataStream) { DataName = Path.GetFileNameWithoutExtension(data) };
            var baseFile = Settings.GetBaseFiles().Find(Settings.FileGroup.BaseFileName);
            var profile = baseFile.ProfileList.Find(Settings.FileGroup.Profile);
            var profileFileName = BuildFilePath(profile.FilePath, profile.FileName);
            
            var p = new GenCompactProfileParser(d, profileFileName, "");
            using (var writer = new GenWriter(null) {FileName = Settings.GeneratedFile.Replace('/', '\\')})
                p.Generate(null, d, writer);
        }

        public void SetProfile(Data.Model.Settings.Profile profile)
        {
            Settings.Profile = profile != null ? profile.Name : "";
            SetProfile();
        }

        public bool CheckIfDataExists(string value)
        {
            var fileName = Path.GetExtension(value) == "" ? Path.ChangeExtension(value, ".dcb") : value;
            return File.Exists(fileName) || File.Exists(Path.Combine("Data", fileName));
        }
    }
}