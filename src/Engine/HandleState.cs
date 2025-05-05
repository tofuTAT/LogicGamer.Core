namespace LogicGamer.Core.Engine
{
    /// <summary>
    /// 表示 IHandle 的处理状态。
    /// </summary>
    public enum HandleState
    {
        /// <summary>
        /// 未处理。
        /// </summary>
        Wait = 0,
        /// <summary>
        /// 正在处理中。
        /// </summary>
        Doing = 1,

        /// <summary>
        /// 成功完成。
        /// </summary>
        Success = 2,

        /// <summary>
        /// 处理失败。
        /// </summary>
        Fail = 3
    }
}