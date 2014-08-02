using System;
using System.IO;
using System.Linq;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace gen
{
    class Program
    {
        static int Main(string[] args)
        {
            var usage = new[]{"Usage:", "gen -p:\"profile path\" -d:\"parameter path\" [-o:\"output path\"]"};
            var profile = "";
            var data = "";
            var output = "";
            var valid = true;

            foreach (var arg in args)
            {
                var s = arg.Split(':');
                //                if (s[0] != "-p" && s[1] == "" || s[0] != "-d" && s[1] == "")

                if (s.Length != 2 || s[0] != "-p" && s[0] != "-d" && s[0] != "-o" || s[0] == "-p" && s[1] == "" || s[0] == "-d" && s[1] == "")
                {
                    Console.WriteLine("Invalid input: " + arg);
                    valid = false;
                }
                if (!valid)
                {
                    foreach (var line in usage)
                        Console.WriteLine(line);
                    Console.ReadKey();
                    return -1;
                }

                if (s[0] == "-p" && profile != "" || s[0] == "-d" && data != "" || s[0] == "-o" && output != "")
                {
                    Console.WriteLine("Duplicated parameter: " + arg);
                    foreach (var line in usage)
                        Console.WriteLine(line);
                    valid = false;
                }
                if (!valid)
                {
                    foreach (var line in usage)
                        Console.WriteLine(line);
                    Console.ReadKey();
                    return -1;
                }

                switch (s[0][1])
                {
                    case 'p':
                        profile = s[1];
                        break;
                    case 'd':
                        data = s[1];
                        break;
                    case 'o':
                        output = s[1];
                        break;
                }
            }

            if (profile == "")
            {
                Console.WriteLine("Missing parameter: -p");
                valid = false;
            }
            
            if (data == "")
            {
                Console.WriteLine("Missing parameter: -d");
                valid = false;
            }
            
            if (!valid)
            {
                foreach (var line in usage)
                    Console.WriteLine(line);
                Console.ReadKey();
                return -1;
            }

            if (!File.Exists(profile))
            {
                Console.WriteLine("The profile (-p) does not exist: " + profile);
                valid = false;
            }

            if (!File.Exists(data))
            {
                Console.WriteLine("The parameter data (-d) does not exist: " + data);
                valid = false;
            }

            if (!valid)
            {
                foreach (var line in usage)
                    Console.WriteLine(line);
                Console.ReadKey();
                return -1;
            }

            GenDataLoader.Register();

            GenParameters d;
            using (var dataStream = new FileStream(data, FileMode.Open))
                d = new GenParameters(dataStream) {DataName = Path.GetFileNameWithoutExtension(data)};
            var p = new GenCompactProfileParser(d, profile, "");
            using (var writer = new GenWriter(null) { FileName = output })
            {
                //try
                {
                    GenFragmentGenerator.Generate(p, null, d, writer);
                }
                //catch (Exception e)
                //{
                //    Console.WriteLine(e);
                //    return 2;
                //}
            }

            return 0;
        }
    }
}
