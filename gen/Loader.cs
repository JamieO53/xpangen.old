// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace gen
{
    public class Loader
    {
        public GenParameters GenParameters { get; set; }
        public GenDataDef GenDataDef { get; set; }
        public GenProfileFragment GenProfileFragment { get; set; }
        public static Loader Load(Args args)
        {
            GenDataLoader.Register();
            var loader = new Loader();
            loader.LoadFiles(args);
            return loader;
        }

        private Args Args { get; set; }
        private void LoadFiles(Args args)
        {
            Args = args;
            if (Args.Definition != "")
            {
                LoadDef(args);
                LoadParametersWithDef();
                LoadProfile();
            }
            else
            {
                LoadParametersWithoutDef();
                LoadProfile();
            }
            GenProfileFragment.GenObject = GenParameters.Root;
        }

        private void LoadParametersWithoutDef()
        {
            using (var dataStream = new FileStream(Args.Data, FileMode.Open))
                GenParameters = new GenParameters(dataStream) {DataName = Path.GetFileNameWithoutExtension(Args.Data)};
            GenDataDef = GenParameters.GenDataDef;
        }

        private void LoadProfile()
        {
            GenProfileFragment = new GenCompactProfileParser(GenDataDef, Args.Profile, "", Args.Delimiter[0]);
        }

        private void LoadDef(Args args)
        {
            GenParameters f;
            using (var definitionStream = new FileStream(args.Definition, FileMode.Open))
                f = new GenParameters(definitionStream) {DataName = Path.GetFileNameWithoutExtension(args.Definition)};
            GenDataDef = f.AsDef();
        }

        private void LoadParametersWithDef()
        {
            using (var dataStream = new FileStream(Args.Data, FileMode.Open))
                GenParameters = new GenParameters(GenDataDef, dataStream)
                {
                    DataName = Path.GetFileNameWithoutExtension(Args.Data)
                };
        }
    }
}
