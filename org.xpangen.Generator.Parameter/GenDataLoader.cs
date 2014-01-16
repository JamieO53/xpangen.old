using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Parameter
{
    /// <summary>
    /// Global generator data loader.
    /// </summary>
    public class GenDataLoader : IGenDataLoader
    {
        static GenDataLoader()
        {
            GenData.DataLoader = new GenDataLoader();
        }

        /// <summary>
        /// Load the GenData from the specified path.
        /// </summary>
        /// <param name="path">The path of the data to be loaded.</param>
        /// <returns>The loaded data.</returns>
        public GenData LoadData(string path)
        {
            var s = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new GenParameters(s);
        }

        /// <summary>
        /// Load the GenData from the specified path using the given definition.
        /// </summary>
        /// <param name="dataDef">The definition of the data being loaded.</param>
        /// <param name="path">The path of the data to be loaded.</param>
        /// <returns>The loaded data.</returns>
        public GenData LoadData(GenDataDef dataDef, string path)
        {
            var s = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new GenParameters(dataDef, s);
            
        }
    }
}
