namespace LogicGamer.Core.Tool.ObjectPool
{
    public interface IObjectPool<T>where T:IObject
    {
        int CurrentSize { get; }

        /// <summary>
        /// 获取一个对象。如果池中有对象，则从池中获取；如果没有，则创建一个新的对象。
        /// </summary>
        T Get(Userdata data = null);

        /// <summary>
        /// 将对象归还到池中。如果池未达到最大大小，则归还；否则不操作。
        /// </summary>
        void Return(T obj);
 
        
        //提供清理内存的方法 参数 MaxSize  保留对象可占用的最大内存
        /// <summary>
        /// 清理池中对象，仅保留指定数量的对象。
        /// </summary>
        /// <param name="maxSize">要保留的最大对象数量</param>
        void Clean(int maxSize);
    }
}