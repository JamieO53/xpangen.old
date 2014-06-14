namespace org.xpangen.Generator.Data
{
    public class GenSavedContext
    {
        public GenData GenData { get; set; }
        public GenObject GenObject { get; set; }

        public int ClassId { get; set; }

        public ISubClassBase SubClassBase { get; set; }

        public string Reference { get; set; }

        public int RefClassId { get; set; }

        public GenData ReferenceData { get; set; }

        public GenSavedContext ParentContext { get; set; }

        public void EstablishContext()
        {
            GenData.EstablishContext(this);
        }
    }
}