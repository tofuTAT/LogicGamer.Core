using LogicGamer.Core.Attributes;
using LogicGamer.Core.Utilities;

namespace LogicGamer.Core.Tool.Log
{
    public class Logger
    {
        public LogLevel Level { get; set; }

        private ILog _log;
        internal Logger(ILog log,LogLevel level)
        {
            _log = log;
            Level = level;
        }
        public void Error(string message)
        {
            if (Level >= LogLevel.Error)
                _log.Print(LogLevel.Error,message);
        }
        public void Warning(string message)
        {
            if (Level >= LogLevel.Warning)
                _log.Print(LogLevel.Warning,message);
        }
        public void Info(string message)
        {
            if (Level >= LogLevel.Info)
                _log.Print(LogLevel.Info,message);
        }
        public void Debug(string message)
        {
            if (Level >= LogLevel.Debug)
                _log.Print( LogLevel.Debug,message);
        }

        [QuicklyEntry(Constants.QuicklyGroup.LOG_ROOT,"Error","错误输出")]
        public static void ErrorQuickly(string message)
        {
            Logic.Printer().Error(message);
        }
        [QuicklyEntry(Constants.QuicklyGroup.LOG_ROOT,"Warning","警告输出")]
        public static void WarningQuickly(string message)
        {
            Logic.Printer().Warning(message);
        }
        [QuicklyEntry(Constants.QuicklyGroup.LOG_ROOT,"Info","信息输出")]
        public static void InfoQuickly(string message)
        {
            Logic.Printer().Info(message);
        }
        [QuicklyEntry(Constants.QuicklyGroup.LOG_ROOT,"Debug","调试输出")]
        public static void DebugQuickly(string message)
        {
            Logic.Printer().Error(message);
        }
        
    }
}