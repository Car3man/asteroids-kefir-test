using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Frontend
{
    public static class ResourcesUtility
    {
        private static readonly Dictionary<string, object> Cache = new();

        public static T Load<T>(string path) where T : Object
        {
            if (Cache.TryGetValue(path, out var cachedResource))
            {
                return (T)cachedResource;
            }

            var resource = Resources.Load<T>(path);
            Cache[path] = resource;
            return resource;
        }
    }
}