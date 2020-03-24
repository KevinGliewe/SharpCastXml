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
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SharpCastXml.Logging
{
    /// <summary>
    /// Root Logger class.
    /// </summary>
    public class Logger
    {
        private int _errorCount;
        private readonly List<string> ContextStack = new List<string>();
        private readonly Stack<LogLocation> FileLocationStack = new Stack<LogLocation>();

        /// <summary>
        /// Initializes the <see cref="Logger"/> class.
        /// </summary>
        public Logger(ILogger output, IProgressReport progress = null)
        {
            LoggerOutput = output;
            ProgressReport = progress;
        }

        /// <summary>
        /// Gets the context as a string.
        /// </summary>
        /// <value>The context.</value>
        private string ContextAsText
        {
            get { return HasContext ? string.Join("/", ContextStack) : null; }
        }

        /// <summary>
        ///   Gets or sets the logger output.
        /// </summary>
        /// <value>The logger output.</value>
        public ILogger LoggerOutput { get; }

        /// <summary>
        ///   Gets a value indicating whether this instance has context.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has context; otherwise, <c>false</c>.
        /// </value>
        private bool HasContext
        {
            get { return ContextStack.Count > 0; }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get { return _errorCount > 0; }
        }

        /// <summary>
        ///   Gets or sets the progress report.
        /// </summary>
        /// <value>The progress report.</value>
        public IProgressReport ProgressReport { get; }

        /// <summary>
        ///   Runs a delegate in the specified log context.
        /// </summary>
        /// <param name = "context">The context.</param>
        /// <param name = "method">The method.</param>
        public void RunInContext(string context, Action method)
        {
            try
            {
                PushContext(context);
                method();
            }
            finally
            {
                PopContext();
            }
        }

        /// <summary>
        ///   Pushes a context string.
        /// </summary>
        /// <param name = "context">The context.</param>
        public void PushContext(string context)
        {
            ContextStack.Add(context);
        }

        /// <summary>
        ///   Pushes a context location.
        /// </summary>
        /// <param name = "fileName">Name of the file.</param>
        /// <param name = "line">The line.</param>
        /// <param name = "column">The column.</param>
        public void PushLocation(string fileName, int line = 1, int column = 1)
        {
            FileLocationStack.Push(new LogLocation(fileName, line, column));
        }

        /// <summary>
        ///   Pops the context location.
        /// </summary>
        public void PopLocation()
        {
            FileLocationStack.Pop();
        }

        /// <summary>
        ///   Pushes a context formatted string.
        /// </summary>
        /// <param name = "context">The context.</param>
        /// <param name = "parameters">The parameters.</param>
        public void PushContext(string context, params object[] parameters)
        {
            ContextStack.Add(string.Format(context, parameters));
        }

        /// <summary>
        ///   Pops the context.
        /// </summary>
        public void PopContext()
        {
            if (ContextStack.Count > 0)
                ContextStack.RemoveAt(ContextStack.Count - 1);
        }

        /// <summary>
        ///   Logs the specified message.
        /// </summary>
        /// <param name = "message">The message.</param>
        public void Message(string message)
        {
            Message("{0}", message);
        }

        /// <summary>
        ///   Logs the specified message.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "parameters">The parameters.</param>
        public void Message(string message, params object[] parameters)
        {
            LogRawMessage(LogLevel.Info, null, message, null, parameters);
        }

        /// <summary>
        ///   Logs the specified progress level and message.
        /// </summary>
        /// <param name = "level">The level.</param>
        /// <param name = "message">The message.</param>
        /// <param name = "parameters">The parameters.</param>
        public void Progress(int level, string message, params object[] parameters)
        {
            Message(message, parameters);
            if (ProgressReport != null)
            {
                if (ProgressReport.ProgressStatus(level, string.Format(message, parameters)))
                    Exit("Process aborted manually");
            }
        }

        /// <summary>
        ///   Logs the specified warning.
        /// </summary>
        /// <param name = "message">The message.</param>
        public void Warning(string code, string message)
        {
            Warning(code, "{0}", message);
        }

        /// <summary>
        ///   Logs the specified warning.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "parameters">The parameters.</param>
        public void Warning(string code, string message, params object[] parameters)
        {
            LogRawMessage(LogLevel.Warning, code, message, null, parameters);
        }

        /// <summary>
        ///   Logs the specified error.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "ex">The ex.</param>
        /// <param name = "parameters">The parameters.</param>
        public void Error(string code, string message, Exception ex, params object[] parameters)
        {
            LogRawMessage(LogLevel.Error, code, message, ex, parameters);
            _errorCount++;
        }

        /// <summary>
        ///   Logs the specified error.
        /// </summary>
        /// <param name = "message">The message.</param>
        public void Error(string code, string message)
        {
            Error(code, "{0}", message);
        }

        /// <summary>
        ///   Logs the specified error.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "parameters">The parameters.</param>
        public void Error(string code, string message, params object[] parameters)
        {
            Error(code, message, null, parameters);
        }

        /// <summary>
        ///   Logs the specified fatal error.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "ex">The exception.</param>
        /// <param name = "parameters">The parameters.</param>
        public void Fatal(string message, Exception ex, params object[] parameters)
        {
            LogRawMessage(LogLevel.Fatal, null, message, ex, parameters);
            _errorCount++;
            Exit("A fatal error occured");
        }

        /// <summary>
        ///   Logs the specified fatal error.
        /// </summary>
        /// <param name = "message">The message.</param>
        public void Fatal(string message)
        {
            Fatal("{0}", message);
        }

        /// <summary>
        ///   Logs the specified fatal error.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "parameters">The parameters.</param>
        public void Fatal(string message, params object[] parameters)
        {
            Fatal(message, null, parameters);
        }

        /// <summary>
        /// Exits the process.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="parameters">The parameters.</param>
        public void Exit(string reason, params object[] parameters)
        {
            string message = string.Format(reason, parameters);
            if (ProgressReport != null)
                ProgressReport.FatalExit(message);

            if (LoggerOutput != null)
                LoggerOutput.Exit(message, 1);
        }

        /// <summary>
        ///   Logs the raw message to the LoggerOutput.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <param name = "message">The message.</param>
        /// <param name = "exception">The exception.</param>
        /// <param name = "parameters">The parameters.</param>
        private void LogRawMessage(LogLevel type, string code, string message, Exception exception, params object[] parameters)
        {
            var logLocation = FileLocationStack.Count > 0 ? FileLocationStack.Peek() : null;

            if (LoggerOutput == null)
                Console.WriteLine("Warning, unable to log error. No LoggerOutput configured");
            else
                LoggerOutput.Log(type, logLocation, ContextAsText, code, message, exception, parameters);
        }
    }
}