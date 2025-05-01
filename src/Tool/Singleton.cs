namespace LogicGamer.Core.Tool
{
    using System;

    public abstract class Singleton<T> where T : class, new()
    {
        // 用于存储单例实例
        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        // 公开访问实例的方法
        public static T Instance => _instance.Value;
 
        // 禁止外部实例化
        protected Singleton() { }
    }
}