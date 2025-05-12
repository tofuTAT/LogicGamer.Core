using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicGamer.Core.ModuleHub.Track
{
    /// <summary>
    /// 泛型 TrackSystem 类，用于异步处理任务队列中的用户数据和结果。
    /// </summary>
    /// <typeparam name="TArgs">用户数据的类型，必须实现 IReadOnlyTrackArgs 接口。</typeparam>
    /// <typeparam name="TResult">任务结果的类型，必须实现 IReadOnlyTrackResult 接口。</typeparam>
    /// <typeparam name="TSender">总控</typeparam>
    public class TrackSystem<TArgs, TResult,TSender> where TArgs : ITrackArgs where TResult : ITrackResult
    {
        public TSender Sender { get; }

        /// <summary>
        /// 用于存储待处理任务的队列，队列中的元素包含用户数据和异步处理函数。
        /// </summary>
        private readonly Queue<(TArgs, Func<TSender,TArgs , TResult>)> _queue = new();

        /// <summary>
        /// 标记是否有任务正在处理中。
        /// </summary>
        private bool _isProcessing = false;

        public TrackSystem(TSender sender)
        {
            Sender = sender;
        }

        /// <summary>
        /// 任务完成时触发的事件。
        /// </summary>
        public event Action<ITrackArgs, ITrackResult> OnTrackEnd;

        /// <summary>
        /// 将任务推入队列，开始执行任务队列中的任务。
        /// </summary>
        /// <param name="track">异步任务处理函数，接受用户数据和当前 TrackSystem 实例，返回任务结果。</param>
        /// <param name="userdata">任务所需的用户数据。</param>
        public void Push(Func<TSender,TArgs, TResult> track, TArgs userdata)
        {
            _queue.Enqueue((userdata, track));
            // 如果当前没有在处理任务，则启动处理
            ProcessQueue(); 
        }

        /// <summary>
        /// 同步递归
        /// </summary>
        private void ProcessQueue()
        {
            if (_queue.Count==0)
            {
                return;
            }
            var (userdata, trackFunc) = _queue.Dequeue();
            var result = trackFunc(Sender,userdata);
            OnTrackEnd?.Invoke(userdata, result); // 任务完成时触发事件
            ProcessQueue();
        }
    }
}
