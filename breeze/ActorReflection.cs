using System;
using System.Collections.Generic;

namespace Breeze.Game
{
    public partial class Actor
    {
        /// <summary>
        /// Creates an instance of the specified actor type and sets specified parameters
        /// </summary>
        /// <param name="type">Type of instance</param>
        /// <param name="par">Parameters to set</param>
        public static Actor Build(Type type, BuildParams par)
        {
            if (type.IsSubclassOf(typeof(Actor)) == false)
                throw new ReflectionException("Passed an non-actor type to the Actor.Build method");
            Actor tmp = (Actor)Activator.CreateInstance(type);
            foreach (KeyValuePair<String, object> pair in par)
            {
                ReflectionHelper.SetAnything(type, tmp, pair.Key, pair.Value);
            }
            return tmp;
        }
    }
}

