using LogicGamer.Core.Tool;
using LogicGamer.Core.Tool.ObjectPool;
using System;
using System.Collections.Generic;

namespace LogicGamer.Core.Engine.Fsm
{
    public class Fsm:IObject
    {
        public static ObjectPool<Fsm> Pool => ObjectPool<Fsm>.Instance;
        private Dictionary<Type, IState> states = new Dictionary<Type, IState>();
        public IState CurrentState { get; private set; }
        public DateTime StateStartTime { get; private set; }
        public bool Running { get; private set; }
        public Userdata Userdata { get; private set; }
        public string Name { get; private set; }

        public IReadOnlyDictionary<Type, IState> States => states;

        //前状态，后状态
        public event Action<IState,IState> OnStateChange;

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">状态实例</param>
        /// <exception cref="ArgumentNullException">state为null时抛出</exception>
        /// <exception cref="ArgumentException">同类型状态已存在时抛出</exception>
        public void AddState(IState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            
            var stateType = state.GetType();
            if (states.ContainsKey(stateType))
            {
                throw new ArgumentException($"State type {stateType} already exists in FSM");
            }

            states[stateType] = state;
        }

        /// <summary>
        /// 通过状态实例切换状态
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <param name="userdata">参数</param>
        public void ChangeState(Type type,Userdata userdata = null)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            if (!states.TryGetValue(type, out var registeredState))
            {
                throw new KeyNotFoundException($"State {type} not registered in FSM");
            }

            // 相同状态不切换
            if (CurrentState?.GetType() == type) 
                return;
            if (!Running)
            {
                Running = true;
            }
            
            CurrentState?.OnExit(this);
            var oldState = CurrentState;
            CurrentState = registeredState;
            CurrentState.OnEnter(this,userdata);
            if (userdata!=null)
            {
                Userdata.GetObjectPool().Return(userdata);
            }
            OnStateChange?.Invoke(oldState,CurrentState);
            StateStartTime = DateTime.Now;
        }

        /// <summary>
        /// 通过泛型类型切换状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        public void ChangeState<T>(Userdata userdata = null) where T : class, IState
        {
            ChangeState(typeof(T),userdata);
        }

        /// <summary>
        /// 更新当前状态
        /// </summary>
        /// <param name="logicTime">逻辑时长</param>
        /// <param name="deltaTime">帧间隔时间</param>
        public void OnUpdate(float logicTime,float deltaTime)
        {
            if (!Running) return;
            CurrentState?.OnUpdate(logicTime,deltaTime,this);
        }

        public void OnInit(Userdata data)
        {
            Running = false;
            Userdata = Userdata.GetObjectPool().Get();
            Name = data.Get<string>(FsmManager.NAME_KEY);
        }

        public void OnReturn()
        {
            OnStateChange = null;
            CurrentState?.OnExit(this);
            Running = false;
            states.Clear();
            Userdata.GetObjectPool().Return(Userdata);
            Userdata = null;
        }
    }
}