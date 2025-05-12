namespace LogicGamer.Core.ModuleHub.AttributeSystem
{
    /// <summary>
    /// 可写的属性系统接口，支持添加、移除及清除属性节点。
    /// </summary>
    public interface IAttributeSystem : IReadOnlyAttributeSystem
    {
        /// <summary>
        /// 添加一个属性节点。
        /// </summary>
        /// <param name="nodeKey">主属性键（如 "HP_Base"）。</param>
        /// <param name="functionKey">功能键，用于标识来源或作用域（如 "Equipment_Sword"、"Buff_1"）。</param>
        /// <param name="value">属性值。</param>
        void AddNode(string nodeKey, string functionKey, int value);

        /// <summary>
        /// 移除指定的属性节点。
        /// </summary>
        /// <param name="nodeKey">主属性键。</param>
        /// <param name="functionKey">功能键。</param>
        void RemoveNode(string nodeKey, string functionKey);

        /// <summary>
        /// 清除所有属性节点。
        /// </summary>
        void ClearAll();
    }
}