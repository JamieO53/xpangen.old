// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;

namespace gen
{
    public class Args
    {
        public string Profile { get; private set; }
        public string Data { get; private set; }
        public string Definition { get; private set; }
        public string Output { get; private set; }
        public string Delimiter { get; private set; }
        public bool Valid { get; private set; }

        private static readonly string[] Usage = {
            "Usage:",
            "gen -p:\"profile path\" -d:\"parameter path\" [-f:\"definition path\"] [-o:\"output path\"] [-m:\"delimiter\"]"
        };

        public static Args Parse(string[] args)
        {
            var a = new Args
            {
                Profile = "",
                Data = "",
                Definition = "",
                Output = "",
                Delimiter = "",
                Valid = true
            };

            a.ParseLocal(args);
            return a;
        }

        private void ParseLocal(string[] args)
        {
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
                    Valid = false;
                }
                if (!Valid)
                {
                    ShowUsage();
                    return;
                }

                if (s[0] == "-p" && Profile != "" ||
                    s[0] == "-d" && Data != "" ||
                    s[0] == "-f" && Definition != "" ||
                    s[0] == "-o" && Output != "" ||
                    s[0] == "-m" && Delimiter != "")
                {
                    Console.WriteLine("Duplicated parameter: " + arg);
                    foreach (var line in Usage)
                        Console.WriteLine(line);
                    Valid = false;
                }
                if (!Valid)
                {
                    ShowUsage();
                    return;
                }

                switch (s[0][1])
                {
                    case 'p':
                        Profile = s[1];
                        break;
                    case 'd':
                        Data = s[1];
                        break;
                    case 'f':
                        Definition = s[1];
                        break;
                    case 'o':
                        Output = s[1];
                        break;
                    case 'm':
                        Delimiter = s[1];
                        break;
                }
            }

            if (Profile == "")
            {
                Console.WriteLine("Missing parameter: -p");
                Valid = false;
            }

            if (Data == "")
            {
                Console.WriteLine("Missing parameter: -d");
                Valid = false;
            }

            if (!Valid)
            {
                ShowUsage();
                return;
            }

            if (!File.Exists(Profile))
            {
                Console.WriteLine("The profile (-p) does not exist: " + Profile);
                Valid = false;
            }

            if (!File.Exists(Data))
            {
                Console.WriteLine("The parameter data (-d) does not exist: " + Data);
                Valid = false;
            }

            if (Definition != "" && !File.Exists(Definition))
            {
                Console.WriteLine("The parameter definition (-f) does not exist: " + Definition);
                Valid = false;
            }

            if (Delimiter.Length > 1 || Delimiter.Length == 1 &&
                ('a' <= Delimiter[0] && Delimiter[0] <= 'z' ||
                'A' <= Delimiter[0] && Delimiter[0] <= 'Z' ||
                '0' <= Delimiter[0] && Delimiter[0] <= '9'))
            {
                Console.WriteLine("The delimiter (-m) must be a single character, and cannot be alphanumeric");
            }

            if (!Valid)
            {
                ShowUsage();
                return;
            }

            if (Delimiter == "") Delimiter = "`";

        }

        private static void ShowUsage()
        {
            foreach (var line in Usage)
                Console.WriteLine(line);
            Console.ReadKey();
        }
    }
}
