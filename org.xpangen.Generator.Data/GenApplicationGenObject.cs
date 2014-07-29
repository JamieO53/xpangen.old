namespace org.xpangen.Generator.Data
{
    public class GenApplicationGenObject : GenApplicationData
    {
        private IGenDataDef GenDataDef { get; set; }
        private GenObject GenObject { get; set; }

        public GenApplicationGenObject(GenObject genObject)
        {
            GenDataDef = genObject.GenDataBase.GenDataDef;
            GenObject = genObject;
            ClassId = GenObject.ClassId;
            SubClasses = new NameList();
            if (GenDataDef == null)
            {
                if (Properties == null)
                    Properties = GenObject.Properties ?? new NameList();
                return;
            }

            ClassName = GenDataDef.Classes[ClassId].Name;
            if (Properties == null)
                Properties = GenDataDef.Classes[ClassId].Properties;
            foreach (var subClass in GenDataDef.Classes[ClassId].SubClasses)
                SubClasses.Add(subClass.SubClass.Name);
        }

        public override void GetFields()
        {
            if (GenObject != null)
            {
                Fields.Clear();
                Values.Clear();
                for (var i = 0; i < Properties.Count; i++)
                    SetString(Properties[i], i < GenObject.Attributes.Count ? GenObject.Attributes[i] : "");
            }
            else
                ClearFieldValues();
        }
    }
}
