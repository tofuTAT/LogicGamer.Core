using System;

namespace LogicGamer.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class WeightAttribute: Attribute
    {
        public WeightAttribute(int weight)
        {
            Weight = weight;
        }

        public int Weight { get; }
    }
}