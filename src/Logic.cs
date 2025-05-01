using System;
using System.Collections.Generic;
using LogicGamer.Core.Engine;
using LogicGamer.Core.Tool;
using LogicGamer.Core.Tool.Log;
using LogicGamer.Core.Utilities;

namespace LogicGamer.Core
{
    public static class Logic
    {
        private static Dictionary<Type,IEngine> _engines = new Dictionary<Type,IEngine>();

        public static Logger Logger { get; private set; }

        public static void Init(ILog logger,LogLevel level)
        {
            Logger = new Logger(logger,level);
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
                args.TryGetValue(item.Key, out var data);
                item.Value.OnStart(data);
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
    }
}

