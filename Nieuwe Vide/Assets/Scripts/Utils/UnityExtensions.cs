using UnityEngine;

namespace ProefProeve
{
    public static class UnityExtensions
    {
        public static T RequireComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }
    }
}
