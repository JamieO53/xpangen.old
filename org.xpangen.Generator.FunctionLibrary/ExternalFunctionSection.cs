using System.Configuration;

namespace org.xpangen.Generator.FunctionLibrary
{
    public class ExternalFunctionSection : ConfigurationSection
    {
        public const string SectionName = "ExternalFunctionSection";
        private const string ExternalFunctionClassCollectionName = "ExternalFunctionClassCollection";

        [ConfigurationProperty("ExternalFunctionClassCollection")]
        [ConfigurationCollection(typeof(ExternalFunctionClassCollection), AddItemName = "add")]
        public ExternalFunctionClassCollection ExternalFunctionClassCollection
        {
            get { return (ExternalFunctionClassCollection) base[ExternalFunctionClassCollectionName]; }
        }
    }
}