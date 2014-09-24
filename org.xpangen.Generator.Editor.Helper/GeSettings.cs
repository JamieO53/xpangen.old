// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Settings;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeSettings : IGenDataSettings
    {
        private FileGroup _fileGroup;
        /// <summary>
        /// The Root of the GenData container of the editor's settings.
        /// </summary>
        public GeneratorEditor Model { get; private set; }

        /// <summary>
        /// Create a new editor settings object
        /// </summary>
        /// <param name="model"></param>
        public GeSettings(GeneratorEditor model)
        {
            Model = model;
        }

        /// <summary>
        /// The relative path of the file group's base file.
        /// </summary>
        public string BaseFilePath
        {
            get { return BaseFile != null ? BaseFile.FilePath + "/" + BaseFile.FileName : ""; }
        }

        /// <summary>
        /// Searches for the requested file group, and moves it to the top of the 
        /// </summary>
        /// <param name="name">The name of the selected file group</param>
        /// <returns>The selected file group.</returns>
        public FileGroup GetFileGroup(string name)
        {
            FileGroup = FindFileGroup(name);
            return FileGroup;
        }
        
        /// <summary>
        /// The base file information for the selected file group.
        /// </summary>
        public BaseFile BaseFile
        {
            get { return FileGroup != null ? (FindBaseFile(FileGroup.BaseFileName) ?? FindBaseFile("Definition")) : null; }
            set
            {
                FileGroup.BaseFileName = value.Name;
            }
        }
        
        /// <summary>
        /// The most recently selected file group
        /// </summary>
        public FileGroup FileGroup
        {
            get { return _fileGroup; }
            set
            {
                _fileGroup = value;
                if (value == null) return;

                var groups = GetFileGroups();
                var index = groups.IndexOf(FileGroup);
                if (index == 0) return;
                
                if (index == -1)
                {
                    index = groups.Count;
                    groups.Add(value);
                }
                groups.Move(ListMove.ToTop, index);
            }
        }

        /// <summary>
        /// The relative path of the selected file group.
        /// </summary>
        public string FilePath
        {
            get { return FileGroup != null ? FileGroup.FilePath + "/" + FileGroup.FileName : ""; }
            set 
            {
                var name = Path.GetFileNameWithoutExtension(value);
                var filePath = Path.GetDirectoryName(value);
                var fileName = Path.GetFileName(value);
                if (name != FileGroup.Name)
                {
                    if (FileGroup.Changed)
                        FileGroup.SaveFields();
                    FileGroup = FindFileGroup(FileGroup.Name) ??
                                new FileGroup(Model.GenDataBase)
                                    {
                                        GenObject =
                                            ((GenObject)Model.GenObject).CreateGenObject("FileGroup"),
                                        Name = name,
                                        FilePath = filePath,
                                        FileName = fileName
                                    };
                }
                FileGroup.FilePath = filePath;
                FileGroup.FileName = fileName;
            }
        }

        /// <summary>
        /// The full path of the generated file.
        /// </summary>
        public string GeneratedFile
        {
            get
            {
                return FileGroup != null ? FileGroup.GeneratedFile : "";
            }
            set
            {
                FileGroup.GeneratedFile = value;
            }
        }
        
        /// <summary>
        /// The home directory of the generator data
        /// </summary>
        public string HomeDir
        {
            get { return Model.GenSettingsList[0].HomeDir; }
            set { Model.GenSettingsList[0].HomeDir = value; }
        }
        
        /// <summary>
        /// The name of the most recently generated profile for the selected file group.
        /// </summary>
        public string Profile
        {
            get
            {
                if (FileGroup == null)
                    return "";
                var profile = FileGroup.Profile;
                var p = BaseFile.ProfileList.Find(profile);
                return p == null
                           ? ""
                           : (string.IsNullOrEmpty(p.FilePath) ? "" : p.FilePath.Replace('/', '\\') + "/") + p.FileName;
            }
            set { if (FileGroup != null) FileGroup.Profile = value; }
        }

        /// <summary>
        /// Get a list of all the file groups.
        /// </summary>
        /// <returns>The file group list.</returns>
        public GenNamedApplicationList<FileGroup> GetFileGroups()
        {
            return Model.GenSettingsList[0].FileGroupList;
        }

        /// <summary>
        /// Get a list of all the base files.
        /// </summary>
        /// <returns>The base file list.</returns>
        public GenNamedApplicationList<BaseFile> GetBaseFiles()
        {
            return Model.GenSettingsList[0].BaseFileList;
        }

        /// <summary>
        /// Find the specified file group.
        /// </summary>
        /// <param name="name">The name of the sought file group.</param>
        /// <returns>The requested base file</returns>
        public FileGroup FindFileGroup(string name)
        {
            return GetFileGroups().Find(name);
        }

        /// <summary>
        /// Check that the minimum structure exists in the settings file.
        /// </summary>
        public void Check()
        {
            if (Model.GenSettingsList.Count == 0)
            {
                var settings = Model.AddGenSettings(".");
                settings.AddBaseFile("Definition", "Definition.dcb", "Data", "The definition required by the editor",
                                     ".dcb");
            }
        }

        /// <summary>
        /// Get the data source.
        /// </summary>
        /// <param name="context">The current object, from which related lists can be found.</param>
        /// <param name="className">The name of the class list being sought.</param>
        /// <returns>The identified data source.</returns>
        public IList GetDataSource(object context, string className)
        {
            if (className == "BaseFile") return GetBaseFiles();
            if (context == null) return null;
            if (!(context is GenApplicationBase))
                throw new GeneratorException("The selected data item must be a GenApplicationBase but is " + context.GetType().Name);
            var contextObject = (GenApplicationBase) context;
            if (contextObject.Lists.ContainsKey(className))
                return contextObject.Lists[className];
            if (contextObject.Parent != null)
                return GetDataSource(contextObject.Parent, className);
            return null;
        }

        /// <summary>
        /// Find the specified base file.
        /// </summary>
        /// <param name="name">The name of the sought base file.</param>
        /// <returns>The requested base file</returns>
        public BaseFile FindBaseFile(string name)
        {
            return GetBaseFiles().Find(name);
        }

        public override string ToString()
        {
            return "GeSettings" + (FileGroup == null ? "" : "." + FileGroup.Name);
        }
    }
}