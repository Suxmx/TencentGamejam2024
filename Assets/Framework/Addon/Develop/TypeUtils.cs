using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework
{
    public static class TypeUtils
    {
        private static readonly string[] RuntimeAssemblyNames =
        {
#if UNITY_2017_3_OR_NEWER
            "UnityGameFramework.Runtime",
#endif
            "Assembly-CSharp",
        };

        private static readonly string[] RuntimeOrEditorAssemblyNames =
        {
#if UNITY_2017_3_OR_NEWER
            "UnityGameFramework.Runtime",
#endif
            "Assembly-CSharp",
#if UNITY_2017_3_OR_NEWER
            "UnityGameFramework.Editor",
#endif
            "Assembly-CSharp-Editor",
        };

        private static readonly System.Reflection.Assembly[] s_Assemblies = null;

        private static readonly Dictionary<string, Type> s_CachedTypes =
            new Dictionary<string, Type>(StringComparer.Ordinal);

        public static string[] GetRuntimeTypeNames(System.Type typeBase)
        {
            return GetTypeNames(typeBase, RuntimeAssemblyNames);
        }

        private static string[] GetTypeNames(System.Type typeBase, string[] assemblyNames)
        {
            List<string> typeNames = new List<string>();
            foreach (string assemblyName in assemblyNames)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch
                {
                    continue;
                }

                if (assembly == null)
                {
                    continue;
                }

                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();
            return typeNames.ToArray();
        }

        // <summary>
        /// 获取已加载的程序集中的指定类型。
        /// </summary>
        /// <param name="typeName">要获取的类型名。</param>
        /// <returns>已加载的程序集中的指定类型。</returns>
        public static Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new Exception("Type name is invalid.");
            }

            Type type = null;
            if (s_CachedTypes.TryGetValue(typeName, out type))
            {
                return type;
            }

            type = Type.GetType(typeName);
            if (type != null)
            {
                s_CachedTypes.Add(typeName, type);
                return type;
            }

            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                type = Type.GetType(string.Format("{0}, {1}", typeName, assembly.FullName));
                if (type != null)
                {
                    s_CachedTypes.Add(typeName, type);
                    return type;
                }
            }

            return null;
        }
    }
}