namespace org.xpangen.Generator.Data
{
    public interface IGenObject
    {
        GenDataBase GenDataBase { get; }
        TextList Attributes { get; }
        int ClassId { get; }
        NameList Properties { get; }
        string ClassName { get; }
    }
}