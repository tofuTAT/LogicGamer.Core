
using System;
using System.Collections.Generic;

namespace LogicGamer.Core.Tool.Log
{
    public class DefaultLog : ILog
    {
        public LogLevel Level { get; set; }

        private readonly Dictionary<LogLevel, ConsoleColor> _logColors;

        public DefaultLog(LogLevel level, Dictionary<LogLevel, ConsoleColor> logColors = null)
        {
            Level = level;
            // 如果没有传入颜色映射，就使用默认配置
            _logColors = logColors ?? new Dictionary<LogLevel, ConsoleColor>
            {
                { LogLevel.Error, ConsoleColor.Red },
                { LogLevel.Warning, ConsoleColor.Yellow },
                { LogLevel.Info, ConsoleColor.White },
                { LogLevel.Debug, ConsoleColor.Gray }
            };
        }

        public void Print(LogLevel level, string value)
        {
            if (level < Level)
                return;

            var message = $"[{level.ToString()}] {value}";

            if (_logColors.TryGetValue(level, out var color))
                Console.ForegroundColor = color;

            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}