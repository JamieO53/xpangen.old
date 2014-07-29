namespace org.xpangen.Generator.Data
{
    /// <summary>
    /// A sub class reference
    /// </summary>
    public class GenDataDefSubClass : IGenDataDefSubClass
    {
        public IGenDataDefClass SubClass { get; set; }

        public string Reference { get; set; }

        public string ReferenceDefinition { get; set; }
    }
}