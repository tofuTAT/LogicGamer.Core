using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogicGamer.Core.Utilities
{
    public static class Utility
    {
        public static class Type
        {
            /// <summary>
            /// 获取当前 AppDomain 中所有加载的类型（如果没有指定程序集，则遍历所有加载的程序集）
            /// </summary>
            public static IEnumerable<System.Type> GetAllTypes(IEnumerable<Assembly> assemblies = null)
            {
                // 如果未传入程序集，则默认获取当前程序域中的所有程序集
                if (assemblies == null || !assemblies.Any())
                {
                    assemblies = AppDomain.CurrentDomain.GetAssemblies();
                }

                // 遍历每个程序集，尝试获取类型
                var types = assemblies.SelectMany(a =>
                {
                    // 直接抛出异常，而不是捕获
                    return a.GetTypes(); // 获取程序集中的所有类型
                });

                if (!types.Any())
                {
                    throw new InvalidOperationException("No types found in the specified assemblies.");
                }

                return types;
            }

            /// <summary>
            /// 查找实现了某接口的所有非抽象类
            /// </summary>
            public static IEnumerable<System.Type> GetTypesImplementing<TInterface>(IEnumerable<Assembly> assemblies = null)
            {
                var targetType = typeof(TInterface);
                return GetAllTypes(assemblies)
                    .Where(t => targetType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
            }

            /// <summary>
            /// 查找继承某个基类的所有非抽象类
            /// </summary>
            public static IEnumerable<System.Type> GetDerivedTypes<TBase>(IEnumerable<Assembly> assemblies = null)
            {
                var baseType = typeof(TBase);
                return GetAllTypes(assemblies)
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));
            }

            /// <summary>
            /// 查找具有某个特性标记的所有类
            /// </summary>
            public static IEnumerable<System.Type> GetTypesWithAttribute<TAttribute>(IEnumerable<Assembly> assemblies = null) where TAttribute : Attribute
            {
                return GetAllTypes(assemblies)
                    .Where(t => t.IsDefined(typeof(TAttribute), inherit: true));
            }

            /// <summary>
            /// 获取指定类型的类型信息
            /// </summary>
            public static System.Type Get<T>() => typeof(T);
        }
    }
}

