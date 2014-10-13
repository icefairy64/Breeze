using System;
using System.Collections.Generic;

namespace Breeze.Game
{
    public class GameState
    {
        Dictionary<string, Actor> Actors;
        Dictionary<string, int> Counter;
        public GameState Next;

        public GameState()
        {
            Actors = new Dictionary<string, Actor>();
            Counter = new Dictionary<string, int>();
        }

        public void InsertActor(Actor actor)
        {
            if (!Counter.ContainsKey(actor.Name))
                Counter.Add(actor.Name, 0);
            
            Actors.Add(actor.Name + Counter[actor.Name].ToString(), actor);
            actor.InstanceID = Counter[actor.Name];
            Counter[actor.Name]++;
        }

        public void RemoveActor(string instance)
        {
            Actors.Remove(instance);
        }
    }
}

