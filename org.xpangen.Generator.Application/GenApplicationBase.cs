using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Application
{
    public class GenApplicationBase: GenAttributes
    {
        protected GenApplicationBase(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        protected virtual void GenObjectSetNotification()
        {
            
        }

        public override GenObject GenObject
        {
            get
            {
                return base.GenObject;
            }
            set
            {
                if (base.GenObject == value) return;
                base.GenObject = value;
                GenObjectSetNotification();
            }
        }
    }
}
