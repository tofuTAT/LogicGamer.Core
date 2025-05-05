using System;
using System.Collections.Generic;
using System.Reflection;
using LogicGamer.Core.Attributes;
using LogicGamer.Core.Tool;
using LogicGamer.Core.Utilities;

namespace LogicGamer.Core.Engine.ResourceProvider
{
    public class ResourceProviderManager : IEngine
    {
        private readonly Dictionary<Type, IResourceProvider> _providers = new();

        #region 外部接口
        /// <summary>
        /// 获取已注册的资源提供器（可用于调试或手动调用）
        /// </summary>
        public IReadOnlyCollection<IResourceProvider> GetAllProviders() => _providers.Values;

        /// <summary>
        /// 通过类型获取特定 Provider
        /// </summary>
        public T GetProvider<T>() where T : class, IResourceProvider
        {
            return _providers.TryGetValue(typeof(T), out var provider)
                ? provider as T
                : throw new InvalidOperationException($"未找到IResourceProvider:{typeof(T).FullName}");
        }

        /// <summary>
        /// 手动注册资源提供器（可用于非自动发现）
        /// </summary>
        public void RegisterProvider(IResourceProvider provider)
        {
            var type = provider.GetType();
            if (_providers.ContainsKey(type))
            {
                Logic.Warning($"IResourceProvider 已被注册: {type.FullName}");
                return;
            }

            _providers.Add(type, provider);
            Logic.Debug($"手动注册IResourceProvider完毕: {type.FullName}");
        }

        /// <summary>
        /// 注销指定类型的资源提供器
        /// </summary>
        public void UnregisterProvider<T>() where T : IResourceProvider
        {
            var type = typeof(T);
            
            if (_providers.TryGetValue(type,out var provider))
            {
                provider.Release();
                _providers.Remove(type);
                Logic.Debug($"IResourceProvider注销: {type.FullName}");
            }
            Logic.Warning($"注销了一个不存在的IResourceProvider: {type.FullName}");
        }

        /// <summary>
        /// 获取资源加载器（会自动匹配支持路径的 Provider）
        /// </summary>
        public IResourceLoader GetLoader(string location, Userdata userdata = null)
        {
            foreach (var provider in _providers.Values)
            {
                if (provider.CheckLocation(location))
                {
                    return provider.GetLoader(location, userdata);
                }
            }
            throw new InvalidOperationException($"未找到该资源 location: {location}");
        }

        #endregion

        #region 生命周期
        public void OnStart(Userdata data = null)
        {
            var types = Utility.Type.GetTypesImplementing<IResourceProvider>();

            foreach (var type in types)
            {
                var enable = type.GetCustomAttribute<EnableAttribute>();
                if (enable == null) continue;
                try
                {
                    var instance = (IResourceProvider)Activator.CreateInstance(type);
                    _providers.Add(type, instance);
                    Logic.Debug($"IResourceProvider 自动注册成功: {type.FullName}");
                }
                catch (Exception ex)
                {
                    Logic.Warning($"IResourceProvider 自动注册实例化时出现未知错误: {type.FullName}: {ex}");
                }
            }
        }
        public void OnUpdate(float logicTime, float deltaTime)
        {
        }


        public void ShutDown()
        {
            foreach (var item in _providers)
            {
                item.Value.Release();
            }
            _providers.Clear();
            Logic.Debug("ResourceProviderManager关闭");
        }
        

        #endregion

        [QuicklyEntry(Constants.QuicklyGroup.MANAGER_ROOT,"Resource","资源管理器")]
        public static ResourceProviderManager GetInstance()
        {
            return Logic.GetEngine<ResourceProviderManager>();
        }

    }
}
