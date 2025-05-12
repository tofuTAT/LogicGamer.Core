using System;

namespace LogicGamer.Core.ModuleHub.AttributeSystem
{
    /// <summary>
    /// 一个数据节点
    /// </summary>
    public struct AttributeNode
    {
        public AttributeNode(string propKey, string functionKey,int value)
        {
            Value = value;
            PropKey = propKey;
            FunctionKey = functionKey;
        }
        public AttributeNode(string propKey, string functionKey)
        {
            Value = 0;
            PropKey = propKey;
            FunctionKey = functionKey;
        }
        /// <summary>
        /// 功能名
        /// </summary>
        public string FunctionKey { get; private set; }
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropKey { get; private set; }
        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; private set; }
        
        public override bool Equals(object obj)
        {
            return obj is AttributeNode node && FunctionKey == node.FunctionKey && PropKey==node.PropKey;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FunctionKey, PropKey);
        }

        public static bool operator ==(AttributeNode left, AttributeNode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AttributeNode left, AttributeNode right)
        {
            return !(left == right);
        }
    }
}