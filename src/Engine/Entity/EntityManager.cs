using System.Collections.Generic;
using LogicGamer.Core.Attributes;
using LogicGamer.Core.Tool;
using LogicGamer.Core.Utilities;

namespace LogicGamer.Core.Engine.Entity
{
    public class EntityManager : IEngine
    {
        public const string LocationKey = "locationKey";
        
        public const string HelperKey = "Helper";
        private IEntityHelper _entityHelper;
        #region 外部接口

        private Dictionary<string, List<IEntity>> _entities = new Dictionary<string, List<IEntity>>();
        
        public IEntity ShowEntity(string group,string location,Userdata data = null)
        {
            if (data == null)
            {
                data = new Userdata();
            }
            data.Set(LocationKey,location);
            var entity = _entityHelper.EntityPool.Get(data);
            _entityHelper.SetGroup(group,entity);
            if (!_entities.TryGetValue(location, out var list))
            {
                list = new List<IEntity>();
                _entities[location] = list;
            }
            list.Add(entity);
            return entity;
        }
        
        public object GetGroupRoot(string group)
        {
            return _entityHelper.GetGroupRoot(group);
        }
        //根据location获取entity实例
        public List<IEntity> GetEntities(string location)
        {
            if (_entities.TryGetValue(location,out var list))
            {
                return list;
            }
            return null;
        }
        
        public void HideEntity(IEntity entity)
        {
            if (_entities.TryGetValue(entity.Location,out var list))
            {
                if (list.Remove(entity))
                {
                    _entityHelper.EntityPool.Return(entity);
                    return;
                }
            }
            Logic.Error($"Location:{entity.Location}中不存在管理器列表中");
        }
        #endregion
        
        
        #region 实现
    
        public void OnStart(Userdata data)
        {
            _entityHelper= data.Get<IEntityHelper>(HelperKey);
            
        }

        public void OnUpdate(float logicTime, float deltaTime)
        {
            foreach (var item in _entities)
            {
                for (int i = item.Value.Count-1; i >=0 ; i--)
                {
                    item.Value[i].OnUpdate(logicTime,deltaTime);
                }
            }
        }

        public void ShutDown()
        {
            
        }
        

        #endregion
        
        [QuicklyEntry(Constants.QuicklyGroup.MANAGER_ROOT,"Entity","实体管理器")]
        public static EntityManager GetInstance()
        {
            return Logic.GetEngine<EntityManager>();
        }
    }
}