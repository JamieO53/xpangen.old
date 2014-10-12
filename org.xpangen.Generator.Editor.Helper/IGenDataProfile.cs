using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Editor.Helper
{
    public interface IGenDataProfile
    {
        IList GetDataSource(object context, string className);
        GenCompactProfileParser Profile { get; set; }
    }
}
