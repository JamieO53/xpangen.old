using System;
using System.IO;
using org.xpangen.Generator.Parameter;
using org.xpangen.Generator.Profile;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace gen
{
    class Program
    {
        static int Main(string[] args)
        {
            var usage = new[]{"Usage:", "gen -p:\"profile path\" -d:\"parameter path\" [-f:\"definition path\"] [-o:\"output path\"] [-m:\"delimiter\"]"};
            var profile = "";
            var data = "";
            var definition = "";
            var output = "";
            var delimiter = "";
            var valid = true;

            foreach (var arg in args)
            {
                var s = arg.Split(':');
                //                if (s[0] != "-p" && s[1] == "" || s[0] != "-d" && s[1] == "")

                if (s.Length != 2 ||
                    s[0] != "-p" && s[0] != "-d" && s[0] != "-f" && s[0] != "-o" && s[0] != "-m" ||
                    s[0] == "-p" && s[1] == "" ||
                    s[0] == "-d" && s[1] == "")
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

                if (s[0] == "-p" && profile != "" ||
                    s[0] == "-d" && data != "" ||
                    s[0] == "-f" && definition != "" ||
                    s[0] == "-o" && output != "" ||
                    s[0] == "-m" && delimiter != "")
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
                    case 'f':
                        definition = s[1];
                        break;
                    case 'o':
                        output = s[1];
                        break;
                    case 'm':
                        delimiter = s[1];
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

            if (definition != "" && !File.Exists(definition))
            {
                Console.WriteLine("The parameter definition (-f) does not exist: " + definition);
                valid = false;
            }

            if (delimiter.Length > 1 || delimiter.Length == 1 &&
                ('a' <= delimiter[0] && delimiter[0] <= 'z' ||
                'A' <= delimiter[0] && delimiter[0] <= 'Z' ||
                '0' <= delimiter[0] && delimiter[0] <= '9'))
            {
                Console.WriteLine("The delimiter (-m) must be a single character, and cannot be alphanumeric");
            }
            
            if (!valid)
            {
                foreach (var line in usage)
                    Console.WriteLine(line);
                Console.ReadKey();
                return -1;
            }

            if (delimiter == "") delimiter = "`";

            GenDataLoader.Register();

            GenParameters d;
            if (definition != "")
            {
                GenParameters f;
                using (var definitionStream = new FileStream(definition, FileMode.Open))
                    f = new GenParameters(definitionStream) {DataName = Path.GetFileNameWithoutExtension(definition)};
                using (var dataStream = new FileStream(data, FileMode.Open))
                    d = new GenParameters(f.AsDef(), dataStream) {DataName = Path.GetFileNameWithoutExtension(data)};
            }
            else
                using (var dataStream = new FileStream(data, FileMode.Open))
                    d = new GenParameters(dataStream) {DataName = Path.GetFileNameWithoutExtension(data)};
            var p = new GenCompactProfileParser(d.GenDataDef, profile, "", delimiter[0]) {GenObject = d.Root};
            using (var writer = new GenWriter(null) { FileName = output })
            {
                try
                {
                    p.Generate(d, writer);
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
