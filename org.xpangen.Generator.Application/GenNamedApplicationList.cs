using System;
using System.Collections.Generic;
using System.Text;

namespace org.xpangen.Generator.Application
{
    public class GenNamedApplicationList<T> : GenApplicationList<T> where T: GenNamedApplicationBase
    {
        /// <summary>
        /// Find the named object.
        /// </summary>
        /// <param name="name">The name of the sought object.</param>
        /// <returns>The named object if it is in the list, otherwise the default object.</returns>
        public T Find(string name)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return this[i];
            }
            return default(T);
        }
    }
}
