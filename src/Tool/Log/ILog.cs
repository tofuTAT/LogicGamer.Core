namespace LogicGamer.Core.Tool.Log
{
    public interface ILog
    {
        LogLevel Level { get; set; }
        void Print(LogLevel level,string value);
    }
}