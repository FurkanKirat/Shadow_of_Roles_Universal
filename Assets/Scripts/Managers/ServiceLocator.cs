using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();
        
        public static void Register<T>(T service)
        {
           if (!Services.ContainsKey(typeof(T))) 
               Services[typeof(T)] = service;
        }

        public static bool Unregister<T>(T service)
        {
            if (!Services.ContainsKey(typeof(T))) return false;
            return Services.Remove(typeof(T));
        }

        public static T Get<T>()
        {
            return (T)Services[typeof(T)];
        }
    }
}