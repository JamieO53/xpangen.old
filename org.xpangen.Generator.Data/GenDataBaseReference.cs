namespace org.xpangen.Generator.Data
{
    public struct GenDataBaseReference
    {
        internal GenDataBaseReference(string data, string definition) : this()
        {
            Definition = definition;
            Data = data;
        }

        public string Data { get; private set; }
        public string Definition { get; private set; }
    }
}