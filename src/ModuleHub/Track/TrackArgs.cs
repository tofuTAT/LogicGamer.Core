using System.Collections.Generic;

namespace LogicGamer.Core.ModuleHub.Track
{
    public class TrackArgs:ITrackArgs
    {
        // 内部数据字典
        private Dictionary<string, object> _data;

        public TrackArgs()
        {
            _data = new Dictionary<string, object>();
        }

        // 获取指定键的数据，如果没有找到会抛出异常
        public T Get<T>(string key)
        {
            if (_data.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            throw new KeyNotFoundException($"Key '{key}' not found in TrackArgs.");
        }
        
        public T GetOrDefault<T>(string key)
        {
            if (_data.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default;
        }

        // 设置数据到字典中
        public void Set<T>(string key, T value)
        {
            _data[key] = value;
        }

        // 移除指定键的数据
        public bool Remove(string key)
        {
            return _data.Remove(key); // 移除字典中对应的键值对，返回是否成功
        }
    }
}