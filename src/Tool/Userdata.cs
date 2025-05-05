using LogicGamer.Core.Tool.ObjectPool;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LogicGamer.Core.Attributes;
using LogicGamer.Core.Utilities;
using Newtonsoft.Json.Linq;

namespace LogicGamer.Core.Tool
{
    /// <summary>
    /// 线程安全的通用数据容器，支持类型安全的键值存储
    /// </summary>
    public sealed class Userdata : IObject, IEnumerable<KeyValuePair<string, object>>
    {
        [QuicklyEntry(Constants.QuicklyGroup.OBJECT_POOL_ROOT, "Userdata", "Userdata对象池")]
        public static ObjectPool<Userdata> GetObjectPool() => ObjectPool<Userdata>.Instance;

        private ConcurrentDictionary<string, object> _data = new ConcurrentDictionary<string, object>();

        public event Action<string, object> OnValueChange;

        /// <summary>
        /// 设置数据（泛型版本）
        /// </summary>
        public void Set<T>(string key, T value)
        {
            _data[key] = value;
            OnValueChange?.Invoke(key, value);
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
            var args = new JObject
            {
                ["hp"] = 100,
                ["name"] = "Player1"
            };
            return default;
        }

        /// <summary>
        /// 删除指定键
        /// </summary>
        public bool Remove(string key)
        {
            var success = _data.TryRemove(key, out _);
            if (success)
            {
                OnValueChange?.Invoke(key, null);
            }

            return success;
        }

        public void OnReset(Userdata data = null)
        {
            if (data != null)
            {
                _data = new ConcurrentDictionary<string, object>(data._data);
            }
        }

        public void OnReturn()
        {
            OnValueChange = null;
            _data.Clear();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override string ToString()
        {
            return ToJson(this, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string ToJson(Userdata data, int indentLevel)
        {
            var indent = new string(' ', indentLevel * 2);
            var innerIndent = new string(' ', (indentLevel + 1) * 2);
            var lines = new List<string> { indent + "{" };

            foreach (var pair in data._data)
            {
                string key = pair.Key;
                object value = pair.Value;

                string valueStr;
                if (value is Userdata nestedUserdata)
                {
                    valueStr = ToJson(nestedUserdata, indentLevel + 1);
                }
                else if (value is string s)
                {
                    valueStr = $"\"{s}\"";
                }
                else if (value is null)
                {
                    valueStr = "null";
                }
                else
                {
                    valueStr = value.ToString();
                }

                lines.Add($"{innerIndent}\"{key}\": {valueStr}");
            }

            lines.Add(indent + "}");
            return string.Join("\n", lines);
        }

    }
}