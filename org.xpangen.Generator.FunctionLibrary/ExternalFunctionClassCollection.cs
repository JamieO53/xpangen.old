using System.Configuration;

namespace org.xpangen.Generator.FunctionLibrary
{
    public class ExternalFunctionClassCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExternalFunctionClass();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var xfc = ((ExternalFunctionClass) element);
            return xfc.Assemblyname + "." + xfc.Functionclass;
        }
    }
}