using LogicGamer.Core.Tool;

namespace LogicGamer.Core.Engine
{
    public interface IEngine
    {
        void OnStart(Userdata data = null);
        
        void OnUpdate(float logicTime,float deltaTime);
        
        void ShutDown();
    }
}