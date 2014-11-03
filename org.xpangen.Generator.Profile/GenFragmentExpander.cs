// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;

namespace org.xpangen.Generator.Profile
{
    public class GenFragmentExpander
    {
        private readonly GenDataDef _genDataDef;

        private GenFragmentExpander(GenDataDef genDataDef, GenObject genObject, Fragment fragment)
        {
            _genDataDef = genDataDef;
            Fragment = fragment;
            GenObject = genObject;
        }

        private GenDataDef GenDataDef
        {
            get { return _genDataDef; }
        }

        private Fragment Fragment { get; set; }

        private string Expand()
        {
            var outputSize = GetOutputSize(true);
            if (outputSize == 0) return "";
            using (var stream = new MemoryStream(outputSize))
            {
                using (var writer = new GenWriter(stream))
                {
                    GenFragmentGenerator.Generate(GenDataDef, writer, GenObject, Fragment);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
        }

        private string ExpandSecondary()
        {
            var outputSize = GetOutputSize(false);
            if (outputSize == 0) return "";
            using (var stream = new MemoryStream(outputSize))
            {
                using (var writer = new GenWriter(stream))
                {
                    GenFragmentGenerator.GenerateSecondary(GenDataDef, writer, GenObject, Fragment);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
        }

        private int GetOutputSize(bool primary)
        {
            using (var stream = new NullStream())
            {
                using (var writer = new GenWriter(stream))
                {
                    if (primary)
                        GenFragmentGenerator.Generate(GenDataDef, writer, GenObject, Fragment);
                    else
                        GenFragmentGenerator.GenerateSecondary(GenDataDef, writer, GenObject, Fragment);
                    writer.Flush();
                    return (int) writer.Stream.Length;
                }
            }
        }

        private GenObject GenObject { get; set; }

        public static string Expand(GenDataDef genDataDef, GenObject genObject, Fragment fragment)
        {
            return Create(genDataDef, genObject, fragment).Expand();
        }

        private static GenFragmentExpander Create(GenDataDef genDataDef, GenObject genObject, Fragment fragment)
        {
            return new GenFragmentExpander(genDataDef, genObject, fragment);
        }

        public static string ExpandSecondary(GenDataDef genDataDef, GenObject genObject, Fragment fragment)
        {
            if (fragment is ContainerFragment)
                return Create(genDataDef, genObject, fragment).ExpandSecondary();
            return "";
        }
    }
}
