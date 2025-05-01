using LogicGamer.Core.Tool;

namespace LogicGamer.Core.Engine.Fsm
{
    // 状态接口
    public interface IState
    {
        void OnEnter(Fsm fsm, Userdata args);
        void OnUpdate(float logicTime, float deltaTime, Fsm fsm);
        public void OnExit(Fsm fsm);
    }
}