namespace org.xpangen.Generator.Data
{
    public interface IGenApplicationData
    {
        int ClassId { get; set; }
        string ClassName { get; }
        NameList Properties { get; }
        NameList SubClasses { get; }
        bool Changed { get; set; }
        NameList Classes { get; set; }
        void GetFields();
        void SetString(string name, string value);
        string AsString(string name);
    }
}