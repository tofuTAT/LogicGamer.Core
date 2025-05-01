using LogicGamer.Core.Tool;
using System;
using System.Collections.Generic;

namespace LogicGamer.Core.Engine.Fsm
{
    public class FsmManager:IEngine
    {
        private Dictionary<string, Fsm> _allAgent;

        public const string NAME_KEY = "NAME";
        
        public event Action<Fsm> OnFsmCreate;
        
        public event Action<Fsm> OnFsmRelease;
        
        public Fsm CreateFsm(string fsmName)
        {
            if (_allAgent.ContainsKey(fsmName))
            {
                throw new Exception($"状态机 {fsmName} 已存在");
            }
            Userdata args = Userdata.Pool.Get();
            args.Set(NAME_KEY,fsmName);
            var fsm = Fsm.Pool.Get(args);
            _allAgent.Add(fsmName,fsm);
            OnFsmCreate?.Invoke(fsm);
            return fsm;
        }

        public Fsm GetFsm(string fsmName)
        {
            return _allAgent[fsmName];
        }
        public void Release(string name)
        {
            // 检查状态机是否存在
            if (!_allAgent.TryGetValue(name, out var value))
            {
                throw new Exception($"找不到该状态机： {name}");
            }
            _allAgent.Remove(name);
            Fsm.Pool.Return(value);
            OnFsmRelease?.Invoke(value);
        }

        public void OnUpdate(float logicTime, float deltaTime)
        {
            foreach (var item in _allAgent.Values)
            {
                item.OnUpdate(logicTime,deltaTime);
            }
        }
        public void OnStart(Userdata data)
        {
            _allAgent = new Dictionary<string, Fsm>();
        }
        public void ShutDown()
        {
            foreach (var item in _allAgent)
            {
                Release(item.Key);
            }
            _allAgent.Clear();
        }
    }
}
