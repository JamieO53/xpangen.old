// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;
using org.xpangen.Generator.Editor.Model;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using Root = org.xpangen.Generator.Editor.Model.Root;

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

        public bool Created { get; set; }
        public bool DataChanged { get; set; }
        public IGenTreeNavigator DataNavigator { get; set; }
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

        public GenSegment Profile { get; set; } 
        public IGenData GenDataStore { get; set; }
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

        protected bool SaveToDisk { get; set; }

        public IGenDataSettings LoadSettingsFromData(GenData data)
        {
            var model = new Root(data) {GenObject = data.Root};
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

        /// <summary>
        /// Get values to populate a data editor combo.
        /// </summary>
        /// <param name="name">The name of the combo list.</param>
        /// <returns>The populated combo list, or null if no such list exists.</returns>
        public List<GeComboItem> GetCodesCombo(string name)
        {
            if (name == "YesNo")
                return new List<GeComboItem>
                           {
                               new GeComboItem("Yes", "True"),
                               new GeComboItem("No", "")
                           };
            if (name == "DataType")
                return new List<GeComboItem>
                            {
                                new GeComboItem("String", "String"),
                                new GeComboItem("Integer", "Integer"),
                                new GeComboItem("Boolean", "Boolean"),
                                new GeComboItem("Identifier", "Identifier")
                            };
            return null;
        }
    }
}