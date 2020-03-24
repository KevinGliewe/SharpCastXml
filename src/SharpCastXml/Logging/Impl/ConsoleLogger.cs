using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCastXml.Logging.Impl {
    public class ConsoleLogger : ILogger, IProgressReport {
        public void Exit(string reason, int exitCode) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[System] Exited({exitCode}): resason=\"{reason}\"");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void FatalExit(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[System] FatalExit: message=\"{message}\"");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Log(LogLevel logLevel, LogLocation logLocation, string context, string code, string message, Exception exception, params object[] parameters) {

            var sb = new StringBuilder();

            sb.Append($"[{context ?? "System"}] ");

            if (message != null)
                sb.Append(string.Format(message, parameters));

            if (exception != null)
                sb.Append($"exception =\"{exception}\"");

            Console.ForegroundColor = exception is null ? ConsoleColor.Cyan : ConsoleColor.Yellow;
            Console.WriteLine(sb.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public bool ProgressStatus(int level, string message) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[System] Progress({level}%): message=\"{message}\"");
            Console.ForegroundColor = ConsoleColor.White;
            return false;
        }
    }
}
