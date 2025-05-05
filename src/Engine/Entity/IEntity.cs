using LogicGamer.Core.Tool;
using LogicGamer.Core.Tool.ObjectPool;

namespace LogicGamer.Core.Engine.Entity
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity:IObject
    {
        string Location { get; }
        /// <summary>
        /// 每帧逻辑更新。
        /// </summary>
        /// <param name="logicTime">游戏逻辑时间。</param>
        /// <param name="deltaTime">距上次更新经过的时间（单位：秒）。</param>
        void OnUpdate(float logicTime, float deltaTime);
    }
}