using System;
using System.Collections.Generic;
using LogicGamer.Core.Attributes;
using LogicGamer.Core.Engine;
using LogicGamer.Core.Tool;
using LogicGamer.Core.Tool.Log;
using LogicGamer.Core.Utilities;

namespace LogicGamer.Core
{
    public static class Logic
    {
        private static Dictionary<Type,IEngine> _engines = new Dictionary<Type,IEngine>();

        private static ILog printer;
        public static void Init(ILog logger)
        {
            printer = logger;
            if (_engines==null)
            {
                _engines = new Dictionary<Type, IEngine>();
            }
            var types = Utility.Type.GetTypesImplementing<IEngine>();
            foreach (var item in types)
            {
                IEngine engine =(IEngine) Activator.CreateInstance(item);
                _engines.Add(item,engine);
            }
        }

        public static void Start(Dictionary<Type,Userdata> args)
        {
            foreach (var item in _engines)
            {
                args.TryGetValue(item.Key, out var userdata);
                item.Value.OnStart(userdata);
            }
        }
        
        public static void Update(float logicTime,float deltaTime)
        {
            foreach (var item in _engines)
            {
                item.Value.OnUpdate(logicTime,deltaTime);
            }
        }
        
        public static void ShutDown()
        {
            foreach (var item in _engines)
            {
                item.Value.ShutDown();
            }
        }

        public static T GetEngine<T>() where T : class, IEngine
        {
            return GetEngine(typeof(T)) as T;
        }

        public static IEngine GetEngine(Type type)
        {
            if (!_engines.TryGetValue(type,out var value))
            {
                throw new Exception($"engine不存在: {type}");
            }

            return value;
        }

        #region log

        [QuicklyEntry(Constants.QuicklyGroup.LOG_ROOT,"Error","错误输出")]
        public static void Error(string message)
        {
            printer.Print(LogLevel.Error,message);
        }
        [QuicklyEntry(Constants.QuicklyGroup.LOG_ROOT,"Warning","警告输出")]
        public static void Warning(string message)
        {
            printer.Print(LogLevel.Warning,message);
        }
        [QuicklyEntry(Constants.QuicklyGroup.LOG_ROOT,"Info","信息输出")]
        public static void Info(string message)
        {
            printer.Print(LogLevel.Info,message);
        }
        [QuicklyEntry(Constants.QuicklyGroup.LOG_ROOT,"Debug","调试输出")]
        public static void Debug(string message)
        {
            printer.Print(LogLevel.Debug,message);
        }

        #endregion
    }
}

