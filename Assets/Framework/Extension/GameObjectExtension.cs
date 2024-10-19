using UnityEngine;

namespace Framework
{
    public static class GameObjectExtension
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T: Component
        {
            T comp = go.GetComponent<T>();
            if (comp is null)
            {
                comp = go.AddComponent<T>();
            }

            return comp;
        }
    }
}