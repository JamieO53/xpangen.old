using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenData
    {
        bool Changed { get; }
        GenData DefGenData { get; }
        GenData GenData { get; }
        void SetBase(string filePath);
        void SetData(string filePath);
    }
}