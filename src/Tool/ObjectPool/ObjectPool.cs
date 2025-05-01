using System.Collections.Generic;

namespace LogicGamer.Core.Tool.ObjectPool
{
    public class ObjectPool<T> : Singleton<ObjectPool<T>> where T : class,IObject, new()
    {
        private readonly Stack<T> _pool = new Stack<T>();
        private readonly object _lock = new object(); // 锁对象

        //当前等待数量
        public int CurrentSize => _pool?.Count ?? 0;

        /// <summary>
        /// 获取一个对象。如果池中有对象，则从池中获取；如果没有，则创建一个新的对象。
        /// </summary>
        public T Get(Userdata data = null)
        {
            lock (_lock)
            {
                T obj = null;
                if (_pool.Count > 0)
                {
                    obj = _pool.Pop();
                }
                else
                {
                    obj = new T(); 
                }
                obj.OnInit(data);
                if (data!=null)
                {
                    Userdata.Pool.Return(data);
                }
                return obj;  // 池中没有对象，创建一个新的
            }
        }
        
        /// <summary>
        /// 将对象归还到池中。如果池未达到最大大小，则归还；否则不操作。
        /// </summary>
        public void Return(T obj)
        {
            lock (_lock)
            {
                obj.OnReturn();  // 清理对象
                _pool.Push(obj);  // 归还对象到池中
            }
        }
        
        //提供清理内存的方法 参数 MaxSize  保留对象可占用的最大内存
        /// <summary>
        /// 清理池中对象，仅保留指定数量的对象。
        /// </summary>
        /// <param name="maxSize">要保留的最大对象数量</param>
        public void Clean(int maxSize)
        {
            lock (_lock)
            {
                while (_pool.Count > maxSize)
                {
                    _pool.Pop(); // 如果需要释放资源，这里可以调用 Dispose
                }
            }
        }
    }
}