namespace org.xpangen.Generator.Data
{
    public class GenDataBase : BindableObject
    {
        private GenDataBaseReferences _references;

        public GenDataBase(GenDataDef genDataDef)
        {
            GenDataDef = genDataDef;
            IgnorePropertyValidation = true;
            Root = new GenObject(null, null, 0) { GenDataBase = this };
        }

        public GenDataDef GenDataDef { get; private set; }
        public GenObject Root { get; private set; }
        public GenDataBaseReferences References
        {
            get
            {
                if (HasReferences)
                    _references = new GenDataBaseReferences();
                return _references;
            }
        }

        public bool HasReferences
        {
            get { return _references == null; }
        }

        public bool Changed { get; set; }

        public string DataName { get; set; }

        public void RaiseDataChanged(string className, string propertyName)
        {
            RaisePropertyChanged(className + '.' + propertyName);
        }

        public GenDataDef AsDef()
        {
            var x = new GenDataToDef(this);
            return x.AsDef();
        }

        public GenObject CreateGenObject(string className, GenObject parent)
        {
            var classId = GenDataDef.Classes.IndexOf(className);
            var k = GenDataDef.IndexOfSubClass(parent.ClassId, classId);
            var l = parent.SubClass[k] as GenObjectListBase;
            if (l != null)
            {
                var o = new GenObject(l.Parent, l, classId);
                l.Add(o);
                Changed = true;
                return o;
            }
            return null;
        }

        public override string ToString()
        {
            return DataName;
        }
    }
}