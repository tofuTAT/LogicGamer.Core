using System;

namespace LogicGamer.Core.Engine.ResourceProvider
{
    
    public interface IResourceLoader
    {
        string Location { get; }
        
        IResourceProvider Provider { get; }
        void GetAsset<T>(Action<LoaderState,T> loadFinish);
        void GetAsset(Type type,Action<LoaderState,object> loadFinish);
        void UnLoad();
    }
}