using System;
using System.Collections.Generic;

namespace Breeze.Game
{
    public partial class Entity
    {
        /// <summary>
        /// Creates an instance of the specified entity type and sets provided parameters
        /// </summary>
        /// <param name="type">Type of instance</param>
        /// <param name="par">Parameters to set</param>
        public static Entity Build(Type type, BuildParams par)
        {
            if (type.IsSubclassOf(typeof(Entity)) == false)
                throw new ReflectionException("Passed an non-entity type to the Entity.Build method");
            Entity tmp = (Entity)Activator.CreateInstance(type);
            foreach (KeyValuePair<String, object> pair in par)
            {
                ReflectionHelper.SetAnything(type, tmp, pair.Key, pair.Value);
            }
            return tmp;
        }
    }
}

