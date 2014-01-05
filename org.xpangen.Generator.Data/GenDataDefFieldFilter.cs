namespace org.xpangen.Generator.Data
{
    public class GenDataDefFieldFilter
    {
        public GenDataDefId Target { get; set; }
        public GenDataDefId Source { get; set; }
    }

    public struct GenDataDefId
    {
        public int ClassId { get; set; }
        public int PropertyId { get; set; }
        public string ClassName { get; set; }
        public string PropertyName { get; set; }
    }
}