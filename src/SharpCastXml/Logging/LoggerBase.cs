﻿// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpCastXml.Logging
{
    /// <summary>
    /// <see cref="ILogger"/> base implementation.
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        /// <summary>
        /// Exits the process with the specified reason.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="exitCode">The exit code</param>
        public abstract void Exit(string reason, int exitCode);

        /// <summary>
        /// Logs the specified log message.
        /// </summary>
        /// <param name="logLevel">The log level</param>
        /// <param name="logLocation">The log location.</param>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="parameters">The parameters.</param>
        public abstract void Log(LogLevel logLevel, LogLocation logLocation, string context, string code, string message, Exception exception, params object[] parameters);


        /// <summary>
        /// Formats the message.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="logLocation">The log location.</param>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static string FormatMessage(LogLevel logLevel, LogLocation logLocation, string context, string message, Exception exception, params object[] parameters)
        {
            var lineMessage = new StringBuilder();

            if (logLocation != null)
                lineMessage.AppendFormat("{0}({1},{2}): ", logLocation.File, logLocation.Line, logLocation.Column);

            // Write log parsable by Visual Studio
            var levelName = Enum.GetName(typeof (LogLevel), logLevel).ToLower();
            lineMessage.AppendFormat("{0}:{1}{2}", levelName , (context != null) ? $" in {context} " : "", message != null ? string.Format(message, parameters) : "");

            return lineMessage.ToString();
        }
    }
}