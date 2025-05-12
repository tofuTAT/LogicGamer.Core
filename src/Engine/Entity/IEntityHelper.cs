using LogicGamer.Core.Tool.ObjectPool;

namespace LogicGamer.Core.Engine.Entity
{
    public interface IEntityHelper
    {
        IObjectPool<IEntity> EntityPool { get; }

        void SetGroup(string group, IEntity entity);

        object GetGroupRoot(string rootName);
    }
}