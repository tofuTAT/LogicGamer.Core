using System;

namespace LogicGamer.Core.Attributes
{
    
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class QuicklyEntryAttribute : Attribute
    {
       
        public QuicklyEntryAttribute(string groupName,string valueName,  string description = null)
        {
            Group = groupName;
            ValueName = valueName;
            Description = description;
        }
        
        /// <summary>
        /// 变量名
        /// </summary>
        public string ValueName { get; }
        /// <summary>
        /// 路径名
        /// </summary>
        public string Group { get; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; } 
    }

}