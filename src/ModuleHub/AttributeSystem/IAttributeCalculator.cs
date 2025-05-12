using System.Collections.Generic;

namespace LogicGamer.Core.ModuleHub.AttributeSystem
{
    public interface IAttributeCalculator
    {
        public int GetValue(IReadOnlyList<AttributeNode> nodes);
    }
}