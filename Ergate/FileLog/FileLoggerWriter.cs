using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging.File
{
    public class FileLoggerWriter
    {
        static object _locker = new object();
        internal ConcurrentQueue<string> LogQueue = new ConcurrentQueue<string>();
        internal ConcurrentQueue<string> errorQueue = new ConcurrentQueue<string>();

        public CancellationTokenSource CancellationToken => new CancellationTokenSource();
        string logDir = $"{AppDomain.CurrentDomain.BaseDirectory}logs";


        public static FileLoggerWriter _fileLoggerWriter;

        public FileLoggerWriter()
        {
            Task.Run(() =>
            {
                string fileName = $"{DateTime.Now.ToString("yyyyMMdd")}-all";
                Log(fileName, LogQueue);
            });

            Task.Run(() => 
            {
                string fileName = $"{DateTime.Now.ToString("yyyyMMdd")}-error";
                Log(fileName, errorQueue);
            });
        }

        public void Log(string fileName, ConcurrentQueue<string> queue)
        {
            CreateLogDir();
            var logBuilder = new StringBuilder();
            while (!CancellationToken.IsCancellationRequested || queue.Count > 0)
            {
                logBuilder.Clear();

                int nowCount = queue.Count;

                if (nowCount == 0)
                {
                    Thread.Sleep(50);
                    continue;
                }

                nowCount = nowCount > 30 ? 30 : nowCount;

                for (int i = 0; i < nowCount; i++)
                {
                    queue.TryDequeue(out var log);
                    logBuilder.Append(log);
                }

                string logs = logBuilder.ToString();

                try
                {
                    WriteLog(fileName, logs);
                }
                catch (DirectoryNotFoundException)
                {
                    CreateLogDir();
                    WriteLog(fileName, logs);
                }
                catch (Exception)
                {

                }
            }
        }

        private void WriteLog(string fileName, string log)
        {
            var path = $"{logDir}\\{fileName}.txt";
            if ((int)System.Environment.OSVersion.Platform > 3)
            {
                path = path.Replace(@"\", "/");
            }

            System.IO.File.AppendAllText(path, string.Format("{0}\r\n", log));
        }

        /// <summary>
        /// 单例FileLoggerWriter
        /// </summary>
        public static FileLoggerWriter Instance
        {
            get
            {
                if (_fileLoggerWriter == null)
                {
                    lock (_locker)
                    {
                        _fileLoggerWriter = _fileLoggerWriter ?? new FileLoggerWriter();
                    }
                }
                return _fileLoggerWriter;
            }
        }

        public void WriteLine(LogLevel level, string message, string name, Exception exception)
        {
            var logBuilder = new StringBuilder();

            logBuilder.Append($"-----{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}-----\r\n");
            logBuilder.Append($"{level.ToString()}:{name}\r\n");
            logBuilder.Append($"{message}\r\n");
            if (exception != null)
            {
                logBuilder.Append($"{exception.ToString()}\r\n");
            }
            logBuilder.Append("-----End-----\r\n");
            logBuilder.Append("\r\n");
            LogQueue.Enqueue(logBuilder.ToString());
            if (level == LogLevel.Error)
            {
                errorQueue.Enqueue(logBuilder.ToString());
            }
        }

        void CreateLogDir()
        {
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
        }
    }
}
