namespace org.xpangen.Generator.Data
{
    public class GenNamedApplicationBase: GenApplicationBase
    {
        public GenNamedApplicationBase()
        {
        }

        /// <summary>
        /// Object name: must be well formed
        /// </summary>
        public virtual string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public override string ToString()
        {
            return GetType().Name + "." + Name;
        }
    }
}
