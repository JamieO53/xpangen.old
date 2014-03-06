// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using org.xpangen.Generator.Editor.Model;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeSettings : IGenDataSettings
    {
        public Root Model { get; private set; }

        public GeSettings(Root model)
        {
            Model = model;
        }

        public void DeleteFileGroup()
        {
            throw new NotImplementedException();
        }

        public FileGroup GetFileFromCaption(string caption)
        {
            FileGroup = Model.GenSettingsList[0].FileGroupList.Find(caption);
            return FileGroup;
        }

        public void GetFileGroup()
        {
            throw new NotImplementedException();
        }

        public List<string> GetFileHistory()
        {
            throw new NotImplementedException();
        }

        public void SetFileGroup()
        {
            throw new NotImplementedException();
        }
        //TGESettings = class(TBlackboardComponent, IGenSettings)
        //private
        //  RegIni: TRegIniFile;
        //  FFileGroup: string;
        //  FHomeDir: string;
        //  function GetBase: string;
        //  function GetFileName: string;
        //  function GetFileGroupName: string;
        //  function GetGenerated: string;
        //  function GetHomeDir: string;
        //  function GetProfile: string;
        //  procedure SetBase(const Value: string);
        //  procedure SetFileGroupName(const Value: string);
        //  procedure SetFileName(const Value: string);
        //  procedure SetGenerated(const Value: string);
        //  procedure SetProfile(const Value: string);
        //public
        //  property Base: string
        //    read GetBase write SetBase;
        //  property FileGroup: string
        //    read GetFileGroupName write SetFileGroupName;
        //  property FileName: string
        //    read GetFileName write SetFileName;
        //  property Generated: string
        //    read GetGenerated write SetGenerated;
        //  property HomeDir: string
        //    read FHomeDir;
        //  property Profile: string
        //    read GetProfile write SetProfile;
        //  constructor Create(AOwner: TComponent); override;
        //  destructor Destroy; override;
        //  procedure DeleteFileGroup;
        //  procedure GetFileGroup;
        //  procedure GetFileHistory(sl: TStrings);
        //  function GetFileFromCaption(Caption: string): string;
        //  procedure SetFileGroup;
        //end;
        public BaseFile BaseFile
        {
            get
            {
                return Model.GenSettingsList[0].BaseFileList.Find(FileGroup.BaseFileName);
            } set
            {
                FileGroup.BaseFileName = value.Name;
            }
        }
        public FileGroup FileGroup { get; set; }
        public string FilePath
        {
            get { return FileGroup.FilePath + "/" + FileGroup.FileName; }
            set {}
        }
        public string Generated { get; set; }
        public string HomeDir { get; set; }
        public string Profile { get; set; }
    }
}