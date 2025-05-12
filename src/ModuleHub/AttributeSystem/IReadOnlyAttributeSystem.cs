using System;

namespace LogicGamer.Core.ModuleHub.AttributeSystem
{
    public interface IReadOnlyAttributeSystem
    {
        /// <summary>
        /// 获取指定属性节点的值。
        /// </summary>
        /// <param name="nodeKey">要查询的属性主键（如 "HP_Base"、"MP_Rate" 等）。</param>
        /// <returns>计算后的属性值。</returns>
        public int GetValue(string nodeKey);
        /// <summary>
        /// 节点数据变化时触发的事件。
        /// </summary>
        event Action<string, int> OnNodeValueChanged;
    }
}