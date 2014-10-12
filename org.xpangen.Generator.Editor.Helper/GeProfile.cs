using System.Collections;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace org.xpangen.Generator.Editor.Helper
{
    public class GeProfile : IGenDataProfile
    {
        public ComboServer ComboServer { get; set; }

        public GeProfile(ComboServer comboServer)
        {
            ComboServer = comboServer;
        }

        public IList GetDataSource(object context, string className)
        {
            return ComboServer.GetComboItems(className);
        }

        public GenCompactProfileParser Profile { get; set; }
    }
}