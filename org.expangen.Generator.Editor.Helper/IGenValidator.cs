using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenValidator
    {
        void Disable();
        void DisplayObject(bool created);
        bool DataChanged { get; set; }
        GenObject GenObject { get; set; }
        char Key { get; set; }
        void Validate();
        void Save();
    }
}