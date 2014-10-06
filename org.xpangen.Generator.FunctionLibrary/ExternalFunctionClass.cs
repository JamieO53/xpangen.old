using System.Configuration;

namespace org.xpangen.Generator.FunctionLibrary
{
    public class ExternalFunctionClass : ConfigurationElement
    {
        [ConfigurationProperty("assemblypath", IsRequired = true)]
        public string Assemblypath
        {
            get { return (string) this["assemblypath"]; }
            private set { this["assemblypath"] = value; }
        }
        [ConfigurationProperty("assemblyname", IsRequired = true)]
        public string Assemblyname
        {
            get { return (string)this["assemblyname"]; }
            private set { this["assemblyname"] = value; }
        }
        [ConfigurationProperty("functionclass", IsRequired = true)]
        public string Functionclass 
        {
            get { return (string)this["functionclass"]; }
            private set { this["functionclass"] = value; } 
        }
    }
}