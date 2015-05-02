using System;
using System.Collections.Generic;
using SFML.System;

namespace Breeze.Game
{
    public delegate bool TransitionCondition(object obj);

    public struct MachineTransition
    {
        public string From;
        public string To;
        public TransitionCondition Condition;
        public int Interval;

        public MachineTransition(string from, string to, TransitionCondition condition, int interval = -1)
        {
            From = from;
            To = to;
            Condition = condition;
            Interval = interval;
        }
    }

    public class StateMachineException : Exception
    {
        public StateMachineException(string msg)
            : base(msg)
        {
        }
    }

    public class TransitionEventArgs : EventArgs
    {
        public MachineTransition Transition;

        public TransitionEventArgs(MachineTransition transition)
            : base()
        {
            Transition = transition;
        }
    }

    public class StateMachine : IUpdatable
    {
        protected uint Time;
        protected int FState = -1;
        protected List<String> States;
        protected List<List<MachineTransition>> Transitions;

        public event EventHandler<TransitionEventArgs> OnTransition;

        public String State 
        {
            get { return States[FState]; }
            protected set 
            { 
                FState = FindState(value);
                Time = 0;
            }
        }

        public StateMachine()
        {
            States = new List<String>();
            Transitions = new List<List<MachineTransition>>();
        }

        protected int FindState(string state)
        {
            if (!States.Contains(state))
                throw new StateMachineException(String.Format("State machine does not contain state \"{0}\"", state));
            return States.FindIndex(str => str == state);
        }

        protected void PerformTransition(MachineTransition transition)
        {
            if (FState > 0 && OnTransition != null)
                OnTransition(this, new TransitionEventArgs(transition));
            State = transition.To;
        }

        public void Update(uint dt)
        {
            Time += dt;

            if (FState < 0)
                return;

            foreach (var trans in Transitions[FState])
            {
                if (trans.Interval > 0 && Time >= trans.Interval)
                {
                    PerformTransition(trans);
                    break;
                }
            }
        }

        public void Input(object obj)
        {
            if (FState < 0)
                return;
                
            foreach (var trans in Transitions[FState])
            {
                if (trans.Condition != null && trans.Condition(obj))
                {
                    PerformTransition(trans);
                    break;
                }
            }
        }

        public void AddState(String state)
        {
            States.Add(state);
            Transitions.Add(new List<MachineTransition>());

            if (States.Count == 1)
                State = state;
        }

        public void AddTransition(MachineTransition transition)
        {
            Transitions[FindState(transition.From)].Add(transition);
        }
    }
}

