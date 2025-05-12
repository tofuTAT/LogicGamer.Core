using LogicGamer.Core.Tool;
using LogicGamer.Core.Tool.ObjectPool;
using System;
using System.Collections.Generic;

namespace LogicGamer.Core.Engine.Fsm
{
    public class Fsm:IObject
    {
        public static ObjectPool<Fsm> Pool => ObjectPool<Fsm>.Instance;
        private Dictionary<Type, StateBase> states = new Dictionary<Type, StateBase>();
        public StateBase CurrentStateBase { get; private set; }
        public DateTime StateStartTime { get; private set; }
        public bool Running { get; private set; }
        public Userdata Userdata { get; private set; }
        public string Name { get; private set; }

        public IReadOnlyDictionary<Type, StateBase> States => states;

        //前状态，后状态
        public event Action<StateBase,StateBase> OnStateChange;

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="stateBase">状态实例</param>
        /// <exception cref="ArgumentNullException">state为null时抛出</exception>
        /// <exception cref="ArgumentException">同类型状态已存在时抛出</exception>
        public void AddState(StateBase stateBase)
        {
            if (stateBase == null) throw new ArgumentNullException(nameof(stateBase));
            
            var stateType = stateBase.GetType();
            if (states.ContainsKey(stateType))
            {
                throw new ArgumentException($"State type {stateType} already exists in FSM");
            }

            states[stateType] = stateBase;
            stateBase.OnInit(this);
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
            if (CurrentStateBase?.GetType() == type) 
                return;
            if (!Running)
            {
                Running = true;
            }
            
            CurrentStateBase?.OnExit();
            var oldState = CurrentStateBase;
            CurrentStateBase = registeredState;
            CurrentStateBase.OnEnter(userdata);
            if (userdata!=null)
            {
                Userdata.GetObjectPool().Return(userdata);
            }
            OnStateChange?.Invoke(oldState,CurrentStateBase);
            StateStartTime = DateTime.Now;
        }

        /// <summary>
        /// 通过泛型类型切换状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        public void ChangeState<T>(Userdata userdata = null) where T : StateBase 
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
            CurrentStateBase?.OnUpdate(logicTime,deltaTime);
        }

        public void OnReset(Userdata data)
        {
            Userdata = Userdata.GetObjectPool().Get();
            Name = data.Get<string>(FsmManager.NAME_KEY);
        }

        public void OnReturn()
        {
            OnStateChange = null;
            CurrentStateBase?.OnExit();
            CurrentStateBase = null;
            Running = false;
            states.Clear();
            Userdata.GetObjectPool().Return(Userdata);
            Userdata.OnReturn();
        }
    }
}