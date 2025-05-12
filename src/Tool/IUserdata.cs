using System;
using System.Collections;

namespace LogicGamer.Core.Tool
{
    public interface IReadOnlyUserdata:IEnumerable
    {
        T Get<T>(string key);

        bool TryGet<T>(string key, out T value);
    }
    
    public interface IUserdata:IReadOnlyUserdata
    {
        event Action<string, object> OnValueChange;

        void Set<T>(string key, T value);
        
        bool Remove(string key);

    }
}