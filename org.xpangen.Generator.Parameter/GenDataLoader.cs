using System.IO;
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
            return new GenParameters(GenParameters.CreateStream(path));
        }

        /// <summary>
        /// Load the GenData from the specified path using the given definition.
        /// </summary>
        /// <param name="dataDef">The definition of the data being loaded.</param>
        /// <param name="path">The path of the data to be loaded.</param>
        /// <returns>The loaded data.</returns>
        public GenData LoadData(GenDataDef dataDef, string path)
        {
            return new GenParameters(dataDef, GenParameters.CreateStream(path));
        }
    }
}
