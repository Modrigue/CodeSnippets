using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace CodeSamples.CSharp
{
    /// <summary>
    /// Provides static methods for logging messages and exceptions to log files.
    /// </summary>
    public static class Logger
    {
        private static readonly object writeLock_ = new object();

        /// <summary>
        /// Initializes the logger with a specific name and logs the initialization.
        /// </summary>
        /// <param name="name">The name to be used for the log file.</param>
        public static void Initialize(string name)
        {
            WriteLineTrace(name, "Logger initialized");
        }

        /// <summary>
        /// Writes a message to the log file without a timestamp.
        /// </summary>
        public static void WriteLine(string name, string message)
        {
            AppendLine(name, message);
        }

        /// <summary>
        /// Writes a message to the log file with a UTC timestamp.
        /// </summary>
        public static void WriteLineTrace(string name, string message)
        {
            AppendLine(name, DateTime.UtcNow + " " + message);
        }

        /// <summary>
        /// Writes an error message to the log file with a UTC timestamp and ERROR prefix.
        /// </summary>
        public static void WriteLineException(string name, string message)
        {
            AppendLine(name, DateTime.UtcNow + " ERROR " + message);
        }

        /// <summary>
        /// Writes a WebException and its details to the log file with a UTC timestamp.
        /// </summary>
        public static void WriteLineException(string name, WebException ex)
        {
            string info = "";
            if (ex.Response != null)
            {
                using (Stream s = ex.Response.GetResponseStream())
                {
                    if (s != null)
                    {
                        using (var reader = new StreamReader(s))
                            info = reader.ReadToEnd();
                    }
                }
            }

            lock (writeLock_)
            {
                string path = getLogPath(name);
                File.AppendAllText(path, DateTime.UtcNow + " ERROR " + ex.Message + ", " + info + Environment.NewLine);
                File.AppendAllText(path, ex.StackTrace + Environment.NewLine);
            }
        }

        /// <summary>
        /// Writes a general Exception and its stack trace to the log file with a UTC timestamp.
        /// </summary>
        public static void WriteLineException(string name, Exception ex)
        {
            lock (writeLock_)
            {
                string path = getLogPath(name);
                File.AppendAllText(path, DateTime.UtcNow + " ERROR " + ex.Message + Environment.NewLine);
                File.AppendAllText(path, ex.StackTrace + Environment.NewLine);
            }
        }

        private static void AppendLine(string name, string line)
        {
            lock (writeLock_)
            {
                File.AppendAllText(getLogPath(name), line + Environment.NewLine);
            }
        }

        /// <summary>
        /// Generates a log file path based on the provided name and current UTC date.
        /// </summary>
        private static string getLogPath(string name)
        {
            return "log_" + name + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log";
        }
    }
}
