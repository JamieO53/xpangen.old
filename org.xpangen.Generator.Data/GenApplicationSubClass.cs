namespace org.xpangen.Generator.Data
{
    public class GenApplicationSubClass : GenApplicationData
    {
        public GenApplicationSubClass(GenSubClass genSubClass)
        {
            GenSubClass = genSubClass;
            ClassId = GenSubClass.ClassId;
            ClassName = GenSubClass.ClassName;
            SubClasses = new NameList();
            Properties = GenSubClass.Properties ?? new NameList();
        }

        private GenSubClass GenSubClass { get; set; }

        public override void GetFields()
        {
            if (GenSubClass != null)
            {
                Fields.Clear();
                Values.Clear();
                for (var i = 0; i < Properties.Count; i++)
                    SetString(Properties[i], i < GenSubClass.Attributes.Count ? GenSubClass.Attributes[i] : "");
            }
            else
                ClearFieldValues();
        }
    }
}