using System;
using System.Collections.Generic;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GenSettings : IGenSettings
    {
        public void DeleteFileGroup()
        {
            throw new NotImplementedException();
        }

        public string GetFileFromCaption(string caption)
        {
            throw new NotImplementedException();
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
        public string Base { get; set; }
        public string FileGroup { get; set; }
        public string FilePath { get; set; }
        public string Generated { get; set; }
        public string HomeDir { get; set; }
        public string Profile { get; set; }
    }
}