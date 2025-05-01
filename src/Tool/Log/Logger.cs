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
    }
}