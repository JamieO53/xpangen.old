// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFunction : GenContainerFragmentBase
    {
        public GenFunction(GenFragmentParams genFragmentParams)
            : base(genFragmentParams.SetFragmentType(FragmentType.Function))
        {
        }

        public string FunctionName
        {
            get { return Function.FunctionName; }
            set { Function.FunctionName = value; }
        }

        public Function Function { get { return (Function) Fragment; } set { Fragment = value; } }
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
                (writer.Stream == null || writer.Stream is FileStream))
                return (writer.FileName = Body.Expand(genData)) != "";
            return base.Generate(prefix, genData, writer);
        }
    }
}