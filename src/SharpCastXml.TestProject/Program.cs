using Autofac;
using Newtonsoft.Json;
using SharpCastXml.Config;
using SharpCastXml.CppModel;
using SharpCastXml.Logging;
using SharpCastXml.Logging.Impl;
using SharpCastXml.Parser;
using SharpCastXml.Rendering;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCastXml.TestProject {
    class Program {
        static void Main(string[] args) {
            var consoleLogger = new ConsoleLogger();
            var logger = new Logger(consoleLogger, consoleLogger);

            string executablePath = null;
//            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
//                executablePath = Path.GetFullPath("../../../../../bin/CastXML/bin/castxml.exe");

            var castXml = new CastXml(logger, new IncludeDirectoryResolver(logger), executablePath);
            var cppParser = new CppParser(logger, castXml);

            var config = new ContextConfig() {
                Id = Path.GetFullPath("test.c"),
                Process = new string[] {
                    Path.GetFullPath("test.c"),
//                    Path.GetFullPath("test.h"),
                }.ToList(),
                Include = new string[] {
                    Path.GetFullPath("test.h"),
                }.ToList(),
                Macros = new Dictionary<string, string>()
                {
                    {"ARRSIZE", "42 * 3"}
                }
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

            Autofac.ContainerBuilder builder = new Autofac.ContainerBuilder();
            builder.RegisterType<TestView>();
            var container = builder.Build();

            var renderingContext = new RenderingContext(container);
            renderingContext.RegisterView<TestView, CppModel.CppStruct, TestAnnotation>();

            StringBuilder stringBuilder = new StringBuilder();
            TextWriter textWriter = new StringWriter(stringBuilder);
            IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textWriter);

            CppStruct str = module.Includes.First().Structs.First();

            renderingContext.Render(str, indentedTextWriter);

            Console.WriteLine(stringBuilder.ToString());

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
