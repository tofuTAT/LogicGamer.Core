using System;
using System.Collections.Generic;

namespace LogicGamer.Core.ModuleHub.AttributeSystem
{
    public class AttributeSystem:IAttributeSystem
    {
        private Dictionary<string, List<AttributeNode>> _allNode = new();
        
      
        
        /// <summary>
        /// 自定义属性计算器
        /// </summary>
        public IAttributeCalculator AttributeCalculator { get; private set; }
        
        public AttributeSystem(IAttributeCalculator attributeCalculator = null)
        {
            //默认叠加
            AttributeCalculator = attributeCalculator ?? new SumAttributeCalculator();
        }

        public int GetValue(string nodeKey)
        {
            List<AttributeNode> temp;
            if (!_allNode.TryGetValue(nodeKey, out temp))
            {
                temp = new List<AttributeNode>();
            }
      
            return AttributeCalculator.GetValue(temp);
        }

        public event Action<string, int> OnNodeValueChanged;

        public void AddNode(string nodeKey, string functionKey, int value)
        {
            if (!_allNode.TryGetValue(nodeKey, out var list))
            {
                list = new List<AttributeNode>();
                _allNode[nodeKey] = list;
            }

            AttributeNode node = new AttributeNode(nodeKey,functionKey,value);
            // 检查是否重复（可选）
            if (list.Contains(node))
            {
                throw new InvalidOperationException($" 重复 NodeKey: '{nodeKey}'已持有 FunctionKey: '{functionKey}'");
            }
            list.Add(node);
            OnNodeValueChanged?.Invoke(nodeKey,GetValue(nodeKey));
        }

        public void ClearNode(string nodeKey)
        {
            if (_allNode.ContainsKey(nodeKey))
            {
                _allNode.Remove(nodeKey);
            }
        }


        public void RemoveNode(string nodeKey, string functionKey)
        {
            if (_allNode.TryGetValue(nodeKey, out var list))
            {
                AttributeNode node = new AttributeNode(nodeKey,functionKey);
                //重写了hash 直接remove即可
                list.Remove(node);
                OnNodeValueChanged?.Invoke(nodeKey,GetValue(nodeKey));
                // 可选：如果列表为空，则清理 key
                if (list.Count == 0)
                    _allNode.Remove(nodeKey);
            }
        }

        public void ClearAll()
        {
            _allNode.Clear();
            OnNodeValueChanged?.Invoke(string.Empty,0);
        }
    }
}