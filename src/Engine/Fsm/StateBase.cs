using LogicGamer.Core.Tool;

namespace LogicGamer.Core.Engine.Fsm
{
    // 状态接口
    public abstract class StateBase
    {
        protected Fsm _fsm;

        public virtual void OnInit(Fsm sender)
        {
            _fsm = sender;
        }

        public virtual void OnEnter(Userdata args)
        {
            
        }

        public virtual void OnUpdate(float logicTime, float deltaTime)
        {
            
        }
        public virtual void OnExit()
        {
            
        }
    }
}