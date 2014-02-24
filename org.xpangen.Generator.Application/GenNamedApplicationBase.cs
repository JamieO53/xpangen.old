using System;
using System.Collections.Generic;
using System.Text;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Application
{
    public class GenNamedApplicationBase: GenApplicationBase
    {

        protected GenNamedApplicationBase(GenDataDef genDataDef) : base(genDataDef)
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
    }
}
