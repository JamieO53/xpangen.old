using System;
using System.IO;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Profile
{
    public class GenFunction : GenContainerFragmentBase
    {
        public GenFunction(GenDataDef genDataDef) : base(genDataDef, null)
        {
            FragmentType = FragmentType.Function;
        }

        public string FunctionName { get; set; }

        public override string ProfileLabel()
        {
            return FunctionName;
        }

        public override string ProfileText(ProfileFragmentSyntaxDictionary syntaxDictionary)
        {
            var format = syntaxDictionary[FragmentType.ToString()].Format;
            
            var param = new string[Body.Count];
            for (var i = 0; i < Body.Count; i++)
                param[i] = Body.Fragment[i].ProfileText(syntaxDictionary);
            var p = string.Join(" ", param);
            
            return string.Format(format, new object[]
                                             {
                                                 FunctionName,
                                                 p
                                             }
                );
        }

        public override string Expand(GenData genData)
        {
            var param = new string[Body.Count];
            for (var i = 0; i < Body.Count; i++)
                param[i] = Body.Fragment[i].Expand(genData);
            return LibraryManager.GetInstance().Execute(FunctionName, param);
        }

        public override bool Generate(GenFragment prefix, GenData genData, GenWriter writer)
        {
            if (String.Compare(FunctionName, "File", StringComparison.OrdinalIgnoreCase) == 0 &&
                writer.Stream is FileStream)
                return (writer.FileName = Body.Expand(genData)) != "";
            return base.Generate(prefix, genData, writer);
        }
    }
}
