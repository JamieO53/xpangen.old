using System.IO;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Parameter
{
    /// <summary>
    /// Global generator data loader.
    /// </summary>
    public class GenDataLoader : IGenDataLoader
    {
        public static void Register()
        {
            GenDataBase.DataLoader = new GenDataLoader();
        }
        
        /// <summary>
        /// Load the GenData from the specified path.
        /// </summary>
        /// <param name="path">The path of the data to be loaded.</param>
        /// <returns>The loaded data.</returns>
        public GenDataBase LoadData(string path)
        {
            using (var stream = GenParameters.CreateStream(GetFullPath(path)))
                return new GenParameters(stream) {DataName = GetDataName(path)};
        }

        private static string GetDataName(string path)
        {
            return Path.GetFileNameWithoutExtension(path.Replace('\\', '/'));
        }

        private static string GetFullPath(string path)
        {
            var fixedPath = path.Replace('/', '\\');
            if (Path.GetExtension(fixedPath) == "")
                fixedPath = Path.ChangeExtension(fixedPath, ".dcb");
            var dataFixedPath = Path.Combine("Data", fixedPath);
            if (!File.Exists(fixedPath) && File.Exists(dataFixedPath))
                fixedPath = dataFixedPath;
            return Path.GetFullPath(fixedPath);
        }

        /// <summary>
        /// Load the GenData from the specified path using the given definition.
        /// </summary>
        /// <param name="dataDef">The definition of the data being loaded.</param>
        /// <param name="path">The path of the data to be loaded.</param>
        /// <returns>The loaded data.</returns>
        public GenDataBase LoadData(GenDataDef dataDef, string path)
        {
            using (var stream = GenParameters.CreateStream(GetFullPath(path)))
                return new GenParameters(dataDef, stream) {DataName = GetDataName(path)};
        }
    }
}
