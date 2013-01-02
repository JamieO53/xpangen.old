using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenData
    {
        bool Changed { get; set; }
        GenData DefGenData { get; set; }
        GenData GenData { get; set; }
        void SetBase(string filePath);
        void SetData(string filePath);
    }
}