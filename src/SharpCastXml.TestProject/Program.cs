using Newtonsoft.Json;
using SharpCastXml.Config;
using SharpCastXml.Logging;
using SharpCastXml.Logging.Impl;
using SharpCastXml.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace SharpCastXml.TestProject {
    class Program {
        static void Main(string[] args) {
            var consoleLogger = new ConsoleLogger();
            var logger = new Logger(consoleLogger, consoleLogger);

            string executablePath = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                executablePath = Path.GetFullPath("../../../../../bin/CastXML/bin/castxml.exe");

            var castXml = new CastXml(logger, new IncludeDirectoryResolver(logger), executablePath);
            var cppParser = new CppParser(logger, castXml);

            var config = new ContextConfig() {
                Id = Path.GetFullPath("test.c"),
                Includes = new string[] {
                    Path.GetFullPath("test.c"),
//                    Path.GetFullPath("test.h"),
                }.ToList()
            };


            cppParser.Initialize(config);

            var module = cppParser.Run(new CppModel.CppModule());

            foreach(var inc in module.Includes) {
                Console.WriteLine(inc.Name);
                foreach(var struct_ in inc.Structs) {
                    Console.WriteLine("    " + struct_.Name + " " +struct_.Size);
                    foreach(var field in struct_.Fields) {
                        Console.WriteLine("        " + field.Name + " " + field.Offset);
                    }
                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(module, Formatting.Indented, new JsonSerializerSettings {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.All
            }));

            foreach (var f in typeof(CastXml).Assembly.GetManifestResourceNames())
                Console.WriteLine(f);

            Console.WriteLine(module);

            Environment.Exit(module is null ? 1 : 0);
        }
    }
}
