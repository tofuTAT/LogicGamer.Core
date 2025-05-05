using System;
using LogicGamer.Core.Tool;

namespace LogicGamer.Core.Engine
{
    public interface IEngineData
    {
        //对应的IEngine类型
        Type Type { get; }

        Userdata Data { get; }
    }
}