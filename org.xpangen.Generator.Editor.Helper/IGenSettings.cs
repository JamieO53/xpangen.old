using System.Collections.Generic;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenSettings
    {
        void DeleteFileGroup();
        string GetFileFromCaption(string caption);
        void GetFileGroup();
        List<string> GetFileHistory(); 
        void SetFileGroup();
    }
}