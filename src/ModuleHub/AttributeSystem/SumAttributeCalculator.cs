using System.Collections.Generic;
using System.Linq;

namespace LogicGamer.Core.ModuleHub.AttributeSystem
{
    public class SumAttributeCalculator:IAttributeCalculator
    {
        public int GetValue(IReadOnlyList<AttributeNode> nodes)
        {
            return nodes.Sum((a) => a.Value);
        }
    }
}