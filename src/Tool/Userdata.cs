using LogicGamer.Core.Tool.ObjectPool;
using System;
using System.Collections.Concurrent;

namespace LogicGamer.Core.Tool
{
    /// <summary>
    /// 线程安全的通用数据容器，支持类型安全的键值存储
    /// </summary>
    public sealed class Userdata:IObject
    {
        public static ObjectPool<Userdata> Pool => ObjectPool<Userdata>.Instance;

        private ConcurrentDictionary<string, object> _data= new ConcurrentDictionary<string, object>();

        public event Action<string, object> OnValueChange;
        /// <summary>
        /// 设置数据（泛型版本）
        /// </summary>
        public void Set<T>(string key, T value)
        {      
            _data[key] = value;
            OnValueChange?.Invoke(key,value);
        }

        /// <summary>
        /// 获取数据（泛型版本），如果不存在则返回默认值
        /// </summary>
        public T Get<T>(string key)
        {
            if (_data.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }
            return default;
        }

        /// <summary>
        /// 删除指定键
        /// </summary>
        public bool Remove(string key)
        {
            var success =  _data.TryRemove(key, out _);
            if (success)
            {
                OnValueChange?.Invoke(key,null);
            }
            return success;
        }

        public void OnInit(Userdata data=null)
        {
            if (data!=null)
            {
                _data = new ConcurrentDictionary<string, object>(data._data);
            }
        }

        public void OnReturn()
        {
            OnValueChange = null;
            _data.Clear();
        }
    }
}
