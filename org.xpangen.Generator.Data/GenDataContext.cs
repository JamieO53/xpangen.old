using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class GenDataContext : List<GenObjectList>
    {
        public GenDataContext(GenDataBase genData)
        {
            GenDataBase = genData;
        }

        public GenDataBase GenDataBase { get; private set; }
    }
}