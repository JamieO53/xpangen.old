// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using org.xpangen.Generator.Profile;

namespace gen
{
    class Program
    {
        static int Main(string[] args)
        {
            var a = Args.Parse(args);
            if (!a.Valid) return 1;

            var data = Loader.Load(a);

            using (var writer = new GenWriter(null) { FileName = a.Output })
            {
                try
                {
                    data.GenProfileFragment.Generate(data.GenParameters, writer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return 1;
                }
            }

            return 0;
        }
    }
}
