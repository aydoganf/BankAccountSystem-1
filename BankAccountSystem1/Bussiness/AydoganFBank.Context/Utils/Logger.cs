using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Context.Utils
{
    internal enum LogType
    {
        Error,
        Warning,
        Info
    }

    public class Logger : ILogger
    {
        private string errorFilePath;
        private string warningFilePath;
        private string infoFilePath;

        public Logger()
        {   
        }

        internal void SetFilePaths(string logFileDirectory)
        {
            errorFilePath = string.Format("{0}/{1}-{2}.txt",
                logFileDirectory, DateTime.Now.ToString("dd-MM-yyyy"), "error");

            warningFilePath = string.Format("{0}/{1}-{2}.txt",
                logFileDirectory, DateTime.Now.ToString("dd-MM-yyyy"), "warning");

            infoFilePath = string.Format("{0}/{1}-{2}.txt",
                logFileDirectory, DateTime.Now.ToString("dd-MM-yyyy"), "info");
        }

        private void WriteToFile(List<string> texts, LogType logType)
        {
            try
            {
                for (int i = 0; i < texts.Count; i++)
                {
                    texts[i] = string.Format("[{0}] - {1}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), texts[i]);
                }

                texts.Add("");

                switch (logType)
                {
                    case LogType.Error:
                        File.AppendAllLines(errorFilePath, texts);
                        break;
                    case LogType.Warning:
                        File.AppendAllLines(warningFilePath, texts);
                        break;
                    case LogType.Info:
                        File.AppendAllLines(infoFilePath, texts);
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void WriteToFile(string text, LogType logType)
        {
            WriteToFile(new List<string>() { text }, logType);
        }


        public void Error(System.Exception ex)
        {
            List<string> error = new List<string>()
            {
                ex.Message,
                ex.InnerException?.Message
            };
            WriteToFile(error, LogType.Error);
        }

        public void Error(string message)
        {
            WriteToFile(message, LogType.Error);
        }

        public void Info(string message)
        {
            WriteToFile(message, LogType.Info);
        }

        public void Info(string message, object obj)
        {
            List<string> info = new List<string>()
            {
                message,
                Newtonsoft.Json.JsonConvert.SerializeObject(obj)
            };
            WriteToFile(info, LogType.Info);
        }

        public void Warning(string message)
        {
            WriteToFile(message, LogType.Warning);
        }

        public void Warning(string message, object obj)
        {
            List<string> info = new List<string>()
            {
                message,
                Newtonsoft.Json.JsonConvert.SerializeObject(obj)
            };
            WriteToFile(info, LogType.Warning);
        }
    }
}
