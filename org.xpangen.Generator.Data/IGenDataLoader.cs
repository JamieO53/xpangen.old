namespace org.xpangen.Generator.Data
{
    public interface IGenDataLoader
    {
        /// <summary>
        /// Load the GenData from the specified path.
        /// </summary>
        /// <param name="path">The path of the data to be loaded.</param>
        /// <returns>The loaded data.</returns>
        GenDataBase LoadData(string path);

        /// <summary>
        /// Load the GenData from the specified path using the given definition.
        /// </summary>
        /// <param name="dataDef">The definition of the data being loaded.</param>
        /// <param name="path">The path of the data to be loaded.</param>
        /// <returns>The loaded data.</returns>
        GenDataBase LoadData(GenDataDef dataDef, string path);
    }
}