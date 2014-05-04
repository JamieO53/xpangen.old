namespace org.xpangen.Generator.Application
{
    public class GenNamedApplicationBase: GenApplicationBase
    {

        protected GenNamedApplicationBase()
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
