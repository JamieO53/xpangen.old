// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Model;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeSettings : IGenDataSettings
    {
        private FileGroup _fileGroup;
        /// <summary>
        /// The Root of the GenData container of the editor's settings.
        /// </summary>
        public Root Model { get; private set; }

        /// <summary>
        /// Create a new editor settings object
        /// </summary>
        /// <param name="model"></param>
        public GeSettings(Root model)
        {
            Model = model;
        }

        /// <summary>
        /// The relative path of the file group's base file.
        /// </summary>
        public string BaseFilePath
        {
            get { return BaseFile.FilePath + "/" + BaseFile.FileName; }
        }

        /// <summary>
        /// Searches for the requested file group, and moves it to the top of the 
        /// </summary>
        /// <param name="name">The name of the selected file group</param>
        /// <returns>The selected file group.</returns>
        public FileGroup GetFileGroup(string name)
        {
            FileGroup = Model.GenSettingsList[0].FileGroupList.Find(name);
            return FileGroup;
        }
        
        /// <summary>
        /// The base file information for the selected file group.
        /// </summary>
        public BaseFile BaseFile
        {
            get
            {
                return Model.GenSettingsList[0].BaseFileList.Find(FileGroup.BaseFileName) ??
                       Model.GenSettingsList[0].BaseFileList.Find("Definition");
            } 
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
            private set
            {
                _fileGroup = value;
                Model.GenSettingsList[0].FileGroupList.Move(ListMove.ToTop, Model.GenSettingsList[0].FileGroupList.IndexOf(FileGroup));
            }
        }

        /// <summary>
        /// The relative path of the selected file group.
        /// </summary>
        public string FilePath
        {
            get { return FileGroup.FilePath + "/" + FileGroup.FileName; }
            set 
            {
                var name = Path.GetFileNameWithoutExtension(value);
                var filePath = Path.GetDirectoryName(value);
                var fileName = Path.GetFileName(value);
                if (name != FileGroup.Name)
                {
                    if (FileGroup.Changed)
                        FileGroup.SaveFields();
                    var newFileGroup = Model.GenSettingsList[0].FileGroupList.Find(FileGroup.Name);
                    if (Model.GenSettingsList[0].FileGroupList.Find(FileGroup.Name) == null)
                    {
                        newFileGroup = new FileGroup(Model.GenData)
                                           {
                                               GenObject =
                                                   Model.GenData.CreateObject("GenSettings",
                                                                              "FileGroup"),
                                               Name = name,
                                               FilePath = filePath,
                                               FileName = fileName
                                           };
                    }
                    FileGroup = newFileGroup;
                }
                FileGroup.FilePath = filePath;
                FileGroup.FileName = fileName;
            }
        }
        
        /// <summary>
        /// The full path of the generated file.
        /// </summary>
        public string Generated
        {
            get { return FileGroup.Generated; }
            set { FileGroup.Generated = value; }
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
                var profile = FileGroup.Profile;
                var p = BaseFile.ProfileList.Find(profile);
                return ((string.IsNullOrEmpty(p.FilePath) ? "" : p.FilePath + "/") + p.FileName).Replace('/', '\\');
            }
            set { FileGroup.Profile = value; }
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
    }
}