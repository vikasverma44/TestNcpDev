using SQLDataMaskingConfigurator.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SQLDataMaskingConfigurator.Helpers
{
   public class Logger
    {
        private const string FILE_EXT = ".log";
        private readonly string datetimeFormat;
        private string logFilename;
        readonly object objLock;
        private ConfigHelper ConfigHelper;

        private DateTime _fileDate = DateTime.Now.Date;
        private int _fileCounter = 1;

        /// <summary>
        /// Initiate an instance of SimpleLogger class constructor.
        /// If log file does not exist, it will be created automatically.
        /// </summary>
        public Logger(ConfigHelper _ConfigHelper)
        {
            this.ConfigHelper = _ConfigHelper;
            objLock = new object();
            datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            CreateNewLogFile();
        }

        /// <summary>
        /// Log a DEBUG message
        /// </summary>
        /// <param name="_Text">Message</param>
        public void Debug(string _Text)
        {
            WriteFormattedLog(Enums.LogLevel.DEBUG, _Text);
        }

        /// <summary>
        /// Log an ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Error(Exception _Ex, string _MethodName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("===ERROR STARTED====================================================" + Environment.NewLine);
            sb.Append("METHOD NAME: " + _MethodName + Environment.NewLine);
            sb.Append("EXCEPTION: " + (_Ex.Message.Length <= 1000 ? _Ex.Message : _Ex.Message.Substring(0, 1000) + "...") + Environment.NewLine);
            sb.Append("INNER EXCEPTION: " + (_Ex.InnerException != null ? _Ex.InnerException.Message : string.Empty) + Environment.NewLine);
            sb.Append("STACKTRACE: " + _Ex.StackTrace + Environment.NewLine);
            sb.Append("==========================================================================================ERROR END===");

            WriteFormattedLog(Enums.LogLevel.ERROR, sb.ToString());

            //WriteFormattedLog(LogLevel.ERROR, 
            //"===================================================================" + Environment.NewLine +
            //"METHOD NAME: " + MethodName + "::" + Environment.NewLine +
            //"EXCEPTION: " + ex.Message + Environment.NewLine +
            //"INNER EXCEPTION: " + ex.InnerException.Message + Environment.NewLine +
            //"STACKTRACE: " + ex.StackTrace + Environment.NewLine +
            //"===================================================================");
            sb = null;
        }

        /// <summary>
        /// Log a FATAL ERROR message
        /// </summary>
        /// <param name="_Text">Message</param>
        public void Fatal(string _Text)
        {
            WriteFormattedLog(Enums.LogLevel.FATAL, _Text);
        }

        /// <summary>
        /// Log an INFO message
        /// </summary>
        /// <param name="_Text">Message</param>
        public void Info(string _Text)
        {
            WriteFormattedLog(Enums.LogLevel.INFO, _Text);
        }

        /// <summary>
        /// Log a TRACE message
        /// </summary>
        /// <param name="_Text">Message</param>
        public void Trace(string _Text)
        {
            WriteFormattedLog(Enums.LogLevel.TRACE, _Text);
        }

        /// <summary>
        /// Log a WARNING message
        /// </summary>
        /// <param name="_Text">Message</param>
        public void Warning(string _Text)
        {
            WriteFormattedLog(Enums.LogLevel.WARNING, _Text);
        }

        private bool WriteLine(string _Text, bool _Append = true)
        {
            try
            {
                if (ConfigHelper.Model.IsLoggingEnabled)
                {
                    FileInfo file_info = new FileInfo(logFilename);
                    if (file_info.Exists)
                    {
                        var IsNewFileRequired = false;
                        if (DateTime.Now.Date > _fileDate)
                        {
                            _fileDate = DateTime.Now.Date;
                            _fileCounter = 1;
                            IsNewFileRequired = true;
                        }
                        else if (file_info.Length > (ConfigHelper.Model.LoggingFileMaxSizeInMB * (1024 * 1024)))
                        {
                            _fileCounter = _fileCounter + 1;
                            IsNewFileRequired = true;
                        }
                        if (IsNewFileRequired) { CreateNewLogFile(); }
                    }
                    ExecuteWriteFileOperation(logFilename, _Text, _Append);
                }
            }
            catch { return false; }
            return true;
        }

        public bool WriteInFile(string _FullFilenameWithPath, string _Text, bool _Append = true)
        {
            try
            {
                ExecuteWriteFileOperation(_FullFilenameWithPath, _Text, _Append);
            }
            catch { return false; }
            return true;
        }

        private void ExecuteWriteFileOperation(string _FullFilenameWithPath, string _Text, bool _Append)
        {
            LogFileModel fm = new LogFileModel(_FullFilenameWithPath, _Append, Encoding.UTF8, _Text);
            PrintJob(fm);
            //Thread workerThread = new Thread(PrintJob)
            //{
            //    IsBackground = true
            //};
            //workerThread.Start(fm);
        }

        public void PrintJob(object _Data)
        {
            if (_Data != null)
            {
                lock (objLock)
                {
                    LogFileModel fm = (LogFileModel)_Data;
                    using (StreamWriter writer = new StreamWriter(path: fm.FilePath, append: fm.IsAppend, encoding: fm.EncodingType))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(fm.TextString)))
                        {
                            writer.WriteLine(fm.TextString);
                        }
                    }
                }
            }
        }

        private void WriteFormattedLog(Enums.LogLevel _Level, string _Text)
        {
            string pretext;
            switch (_Level)
            {
                case Enums.LogLevel.TRACE:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ";
                    break;
                case Enums.LogLevel.INFO:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [INFO]    ";
                    break;
                case Enums.LogLevel.DEBUG:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ";
                    break;
                case Enums.LogLevel.WARNING:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [WARNING] ";
                    break;
                case Enums.LogLevel.ERROR:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ";
                    break;
                case Enums.LogLevel.FATAL:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ";
                    break;
                default:
                    pretext = "";
                    break;
            }

            WriteLine(pretext + _Text);
        }

        private void CreateNewLogFile()
        {
            string logPath = string.Empty;
            if (!string.IsNullOrEmpty(ConfigHelper.Model.LoggingPath))
            {
                logPath = ConfigHelper.Model.LoggingPath.Trim();
            }
            else
            {
                string assemblyLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Logs");
                if (!Directory.Exists(assemblyLocation))
                { Directory.CreateDirectory(assemblyLocation); }
                logPath = assemblyLocation;
            }

            logFilename = logPath + Path.DirectorySeparatorChar +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name +
                "_" + _fileDate.ToString("ddMMMyyyy") + "_" + _fileCounter + FILE_EXT;

            // Log file header line
            if (!System.IO.File.Exists(logFilename))
            {
                string logHeader = logFilename + " is created.";
                WriteLine(System.DateTime.Now.ToString(datetimeFormat) + " " + logHeader, false);
            }
        }
    }
}