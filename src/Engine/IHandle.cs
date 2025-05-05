using System;
using LogicGamer.Core.Tool;

namespace LogicGamer.Core.Engine
{
    /// <summary>
    /// 非泛型基础接口，用于标识 Handle 类型、释放资源等基本操作。
    /// </summary>
    public interface IHandle
    {
        /// <summary>
        /// 状态
        /// </summary>
        HandleState State { get; }

        /// <summary>
        /// 参数
        /// </summary>
        Userdata Args { get; }

        event Action<IHandle> OnCompleted;
        /// <summary>
        /// 释放或卸载持有的对象。
        /// </summary>
        void Release();
    }

    /// <summary>
    /// 泛型版本的句柄，持有目标对象引用。
    /// </summary>
    public interface IHandle<out T> : IHandle
    {
        T Result { get; }
    }
}