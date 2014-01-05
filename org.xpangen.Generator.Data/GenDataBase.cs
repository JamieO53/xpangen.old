namespace org.xpangen.Generator.Data
{
    public class GenDataBase : BindableObject
    {
        public GenDataBase(GenDataDef genDataDef)
        {
            GenDataDef = genDataDef;
            IgnorePropertyValidation = true;
            Root = new GenObject(null, null, 0) { GenData = this };
        }

        public GenDataDef GenDataDef { get; private set; }
        public GenObject Root { get; protected set; }
        public bool Changed { get; set; }

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
            var l = parent.SubClass[k];
            var o = new GenObject(l.Parent, l, classId);
            l.Add(o);
            return o;
        }
    }
}